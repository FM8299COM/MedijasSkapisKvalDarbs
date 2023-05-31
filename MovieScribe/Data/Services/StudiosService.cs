using MovieScribe.Data.Base;
using MovieScribe.Models;

namespace MovieScribe.Data.Services
{
    public class StudiosService : EntityBaseRepo<StudioModel>, IStudiosService
    {
        public StudiosService(DBContext context) : base(context)
        {
            
        }
    }
}
