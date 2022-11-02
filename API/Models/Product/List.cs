using DAL.Models;

namespace API.Models.Product
{
    public class List
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string ReleaseDate { get; set; }
        public List<Comment.List> Comments { get; set; }
    }
}
