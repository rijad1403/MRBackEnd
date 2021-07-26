using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingBackend.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int RatingNum { get; set; }
        public Media Media { get; set; }
    }
}
