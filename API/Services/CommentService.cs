using API.Models;
using API.Models.Comment;
using API.Services.Interfaces;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _dbContext;
        public CommentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public DefaultResponse Create(Create model)
        {
            var comment = new Comment
            {
               CommentDescription = model.Comment,
               Email = model.Email,
               ProductId = model.ProductId,
               DateOfComment = DateTime.Today,
            };
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            return new DefaultResponse
            {
                IsSuccess = true,
                Message = "Comment added successfully"
            };
        }

        public IEnumerable<List> GetByProductId(int productId)
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
