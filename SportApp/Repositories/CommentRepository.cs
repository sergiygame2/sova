using SportApp.Data;
using SportApp.Models;

namespace SportApp.Repositories
{
    public interface ICommentRepository : IModelRepository<Comment>
    {}
    
    
    public class CommentRepository : GenericModelRepository<Comment>
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
