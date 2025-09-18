using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class RemoveResponseContentFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var response in operation.Responses.Values)
        {
            // ❌ Clear out content (application/json, text/plain, etc.)
            response.Content.Clear();
        }
    }
}
