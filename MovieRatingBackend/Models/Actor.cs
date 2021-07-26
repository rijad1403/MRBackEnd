using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingBackend.Models
{
    public class Actor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<MediaActor> MediaActors { get; set; }

    }
}
