using MovieScribe.Data.Base;
using MovieScribe.Models;

namespace MovieScribe.Data.Services
{
    public class WritersService : EntityBaseRepo<WriterModel>, IWritersService
    {
        public WritersService(DBContext context) : base(context)
        {
            
        }
    }
}
