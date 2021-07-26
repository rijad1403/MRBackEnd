using MovieRatingBackend.DTOs;
using MovieRatingBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingBackend.Factories
{
    public static class MediaFactory
    {
        public static Media Create(NewMediaDTO newMediaDTO)
        {
            var media =  new Media
            {
                Title = newMediaDTO.Title,
                Description = newMediaDTO.Description,
                CoverImage = newMediaDTO.CoverImage,
                OverallRating = 0,
                MediaType = newMediaDTO.MediaType,
                ReleaseDate = newMediaDTO.ReleaseDate,
                MediaActors = new List<MediaActor>()
            };

            newMediaDTO.Actors.ForEach(a =>
            {
                media.MediaActors.Add(new MediaActor { ActorId = a });
            });

            return media;
        }

    }
}
