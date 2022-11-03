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

        public DefaultResponse Create(Create model)
        {
            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                ReleaseDate = model.ReleaseDate,
            };

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return new DefaultResponse { 
                IsSuccess = true,
                Message = "Product created successfully"
            };
        }

        public IEnumerable<List> Get()
        {
            var products = _dbContext.Products.Select(a => new List { 
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
    }
}
