using MovieRatingBackend.DTOs;
using MovieRatingBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingBackend.Factories
{
    public static class ActorsFactory
    {
        public static Actor Create(NewActorDTO newActorDTO)
        {
            return new Actor
            {
                Name = newActorDTO.Name,
                Surname = newActorDTO.Surname,
                MediaActors = new List<MediaActor>()
                
            };
        }
    }
}
