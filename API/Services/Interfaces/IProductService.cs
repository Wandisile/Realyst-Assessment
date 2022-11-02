using API.Models;
using API.Models.Product;
using DAL.Models;

namespace API.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<List> Get();
        DefaultResponse Create(Create model);
        DefaultResponse Update(int id, Edit model);
        DefaultResponse AddComments(int productId, List<Models.Comment.Create> comments);
    }
}
