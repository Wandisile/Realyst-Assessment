using API.Models;
using API.Models.Comment;

namespace API.Services.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<List> GetByProductId(int productId);
        DefaultResponse Create(Create model);

    }
}
