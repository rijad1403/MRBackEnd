using MovieRatingBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MovieRatingBackend.DTOs
{
    public class NewMediaDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CoverImage { get; set; }
        public List<int> Actors { get; set; }
        public MediaType MediaType { get; set; }
    }
}
