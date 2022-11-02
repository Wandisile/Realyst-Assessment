using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Product
{
    public class Create
    {
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Comment.Create> Comments { get; set; }
    }
}
