using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingBackend.Models
{
    public class Pagination
    {
        public int PageNumber { get; set; } = 1;
        public const int PageSize = 10;
    }
}
