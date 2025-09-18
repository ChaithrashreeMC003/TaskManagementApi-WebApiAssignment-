using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskBoardApi.Data;
using TaskBoardApi.Exceptions;
using TaskBoardApi.LoggingMiddleware;
using TaskBoardApi.Mapper;
using TaskBoardApi.Mapper;
using TaskBoardApi.Repositories;
using TaskBoardApi.Services;
using TaskBoardManagement.Middleware;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //string? jwtKey = Environment.GetEnvironmentVariable("JwtKey");



        //Loger Configuration
        Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithCorrelationIdHeader() // Add correlation ID to logs
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} (CorrelationId: {CorrelationId}){NewLine}{Exception}")
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Hour)
        .CreateLogger();

        builder.Host.UseSerilog();
        builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);




        // Add services to the container.
        builder.Services.AddControllers();
       
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "TaskBoard API",
                Version = "v1"
            });
            options.OperationFilter<RemoveResponseContentFilter>();
            // ✅ Add JWT Auth to Swagger
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your valid token.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
            {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
            }
            });

        });



        //DbContext Connection string Injection
        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("TaskBoardConnectionString")));
        builder.Services.AddHttpContextAccessor();

        // ------------------- Authorization -------------------
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireSuperAdmin", policy => policy.RequireRole("SuperAdmin"));
            options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("RequireManager", policy => policy.RequireRole("Manager"));
            options.AddPolicy("RequireDeveloper", policy => policy.RequireRole("Developer"));

            // Custom example: ProjectOwnerOrManager
            options.AddPolicy("ProjectOwnerOrManager", policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole("Manager") ||
                    context.User.HasClaim(c => c.Type == "ProjectOwner" && c.Value == "true")));
        });

        //Repository Injection
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITokenRepository, TokenRepository>();
        builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();
        builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
        builder.Services.AddScoped<ITaskTagRepository, TaskTagRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();

        builder.Services.AddTransient<GlobalExceptionMiddleware>();
        //Jwt token validation
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                ),
                NameClaimType = JwtRegisteredClaimNames.Sub,
                RoleClaimType = ClaimTypes.Role
            };
        });

        //Adding CorrelationId in Logging
        builder.Services.AddDefaultCorrelationId(options =>
        {
            options.RequestHeader = "X-Correlation-ID";    // Header from client
            options.ResponseHeader = "X-Correlation-ID";   // Header returned in response
            options.UpdateTraceIdentifier = true;          // Updates HttpContext.TraceIdentifier
        });


        var app = builder.Build();

        //using (var scope = app.Services.CreateScope())
        //{
        //    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //    DbInitializer.Seed(context);
        //}


        app.UseMiddleware<GlobalExceptionMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        

        app.UseGlobalExceptionHandling();
        app.UseRequestLogging();


        app.MapControllers();
        app.Run();
    }
}