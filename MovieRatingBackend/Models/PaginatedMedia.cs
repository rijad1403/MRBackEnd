using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingBackend.Models
{
    public class PaginatedMedia
    {
        public List<Media> Medias { get; set; }
        public int CurrentPage { get; set; }
        public int NextPage { get; set; }

    }
}
