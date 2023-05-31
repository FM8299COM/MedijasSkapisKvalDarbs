using MovieScribe.Data.Base;
using MovieScribe.Models;

namespace MovieScribe.Data.Services
{
    public class ProducerService : EntityBaseRepo<ProducerModel>, IProducerService
    {
        public ProducerService(DBContext context) : base(context)
        {
            
        }
    }
}
