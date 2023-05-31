using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data.Base;
using MovieScribe.Models;

namespace MovieScribe.Data.Services
{
    public class DistributorService : EntityBaseRepo<DistributorModel>, IDistributorService
    {
        public DistributorService(DBContext context) : base(context) { }
    }
}
