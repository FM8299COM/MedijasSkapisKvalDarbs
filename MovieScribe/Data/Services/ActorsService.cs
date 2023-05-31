using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data.Base;
using MovieScribe.Models;

namespace MovieScribe.Data.Services
{
    public class ActorsService : EntityBaseRepo<ActorModel>, IActorsService
    {
        public ActorsService(DBContext context) : base(context) { }

    }
}
