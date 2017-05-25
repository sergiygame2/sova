using SportApp.Data;
using SportApp.Models;

namespace SportApp.Repositories
{
    public class CommentRepository : GenericModelRepository<Comment>
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
