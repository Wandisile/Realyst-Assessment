namespace API.Models
{
    public class ProductStats
    {
        public int TotalProducts { get; set; }
        public List<CommentsPerProduct> NrOfCommentsPerProduct { get; set; }
    }

    public class CommentsPerProduct
    {
        public string Product { get; set; }
        public int NrOfComments { get; set; }
    }
}
