using System.ComponentModel.DataAnnotations;

namespace API.Models.Comment
{
    public class Create
    {
        [Required]
        public string Comment { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
