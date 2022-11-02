using API.Models;
using API.Models.Product;
using API.Services.Interfaces;
using DAL;
using DAL.Models;

namespace API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public DefaultResponse AddComments(int productId, List<Models.Comment.Create> comments)
        {
            for (int i = 0; i < comments.Count; i++)
            {
                var comment = new Comment
                {
                    CommentDescription = comments[i].Comment,
                    DateOfComment = DateTime.Today,
                    Email = comments[i].Email,
                    ProductId = productId
                };
                _dbContext.Comments.Add(comment);
            }
            _dbContext.SaveChanges();

            return new DefaultResponse
            {
                IsSuccess = true,
                Message = "Comments added"
            };
        }

        public DefaultResponse Create(Create model)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                ReleaseDate = model.ReleaseDate,
            };

            _dbContext.Products.Add(product);

            List<Comment> commentsList = new List<Comment>();

            for (int i = 0; i < model.Comments.Count; i++)
            {
                var comment = new Comment {
                   CommentDescription = model.Comments[i].Comment,
                   DateOfComment = DateTime.Today,
                   Email = model.Comments[i].Email,
                   ProductId = product.ProductId
                };
                commentsList.Add(comment);
            }
            _dbContext.Comments.AddRange(commentsList);
            _dbContext.SaveChanges();

            return new DefaultResponse { 
                IsSuccess = true,
                Message = "Product created successfully"
            };
        }

        public IEnumerable<List> Get()
        {
            var products = _dbContext.Products.Select(a => new List { 
                Comments = GetComments(a.ProductId),
                Name = a.Name,
                Price = a.Price,
                ReleaseDate = a.ReleaseDate.ToShortDateString()
            }).ToList();

            return products;
        }

        public DefaultResponse Update(int id, Edit model)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return new DefaultResponse
                {
                    IsSuccess = false,
                    Message = "Product not found!"
                };
            }
            product.Name = model.Name;
            product.Price = model.Price;
            product.ReleaseDate = model.ReleaseDate;
            _dbContext.SaveChanges();

            return new DefaultResponse
            {
                IsSuccess = true,
                Message = "Product updated successfully"
            };
        }

        ProductStats IProductService.GetProductStats()
        {
            var totalProducts = _dbContext.Products.Count();

            var numberOfProducts = _dbContext.Products
                .Join(_dbContext.Comments,
                p => p.ProductId,
                c => c.ProductId,
                (p, c) => new { prod = p, comm = c })
                .GroupBy(g => g.comm.ProductId)
                .Select(s => new CommentsPerProduct
                {
                    NrOfComments = s.Count(),
                    Product = s.FirstOrDefault()!.prod.Name
                }).ToList();


            var stats = new ProductStats
            {
                TotalProducts = totalProducts,
                NrOfCommentsPerProduct = numberOfProducts
            };

            return stats;
        }

        private List<Models.Comment.List> GetComments(int productId)
        {
            return _dbContext.Comments.Where(c => c.ProductId == productId)
                .Select(a => new Models.Comment.List
                {
                    Comment = a.CommentDescription,
                    DateOfComment = a.DateOfComment.ToShortDateString(),
                    Email = a.Email
                }).ToList();
        }

    }
}
