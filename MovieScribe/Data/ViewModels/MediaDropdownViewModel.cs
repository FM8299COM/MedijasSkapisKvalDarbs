using Microsoft.AspNetCore.Mvc.Formatters;
using MovieScribe.Models;

namespace MovieScribe.Data.ViewModels
{
    public class MediaDropdownViewModel
    {
        public MediaDropdownViewModel()
        {
            Producers = new List<ProducerModel>();
            Distributors = new List<DistributorModel>();
            Studios = new List<StudioModel>();
            Actors = new List<ActorModel>();
            Genres = new List<GenreModel>();
            Languages = new List<LanguageModel>();
            Writers = new List<WriterModel>();


        }

        public List<ProducerModel> Producers { get; set; }
        public List<DistributorModel> Distributors { get; set; }
        public List<StudioModel> Studios { get; set; }
        public List<ActorModel> Actors { get; set; }
        public List<GenreModel> Genres { get; set; }
        public List<LanguageModel> Languages { get; set; }
        public List<WriterModel> Writers { get; set; }


    }
}
