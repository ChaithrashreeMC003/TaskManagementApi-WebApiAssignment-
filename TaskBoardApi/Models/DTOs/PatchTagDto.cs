using System.ComponentModel.DataAnnotations;

namespace TaskBoardApi.Models.DTOs
{
    public class PatchTagDto
    {
        [StringLength(50, MinimumLength = 2)]
        public string? Name { get; set; }

        [RegularExpression("^#([A-Fa-f0-9]{6})$", ErrorMessage = "Color must be a valid hex code like #FFFFFF")]
        public string? ColorHex { get; set; }
    }
}
