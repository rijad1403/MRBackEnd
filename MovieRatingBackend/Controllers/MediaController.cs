using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRatingBackend.Contexts;
using MovieRatingBackend.DTOs;
using MovieRatingBackend.Factories;
using MovieRatingBackend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieRatingBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly Context context;
        private readonly IWebHostEnvironment env;
        public MediaController(Context context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "type")] MediaType type, [FromQuery] Pagination pagination, [FromQuery(Name = "search")] string search, [FromQuery(Name = "getAll")] string getAll)
        {
            try
            {

                if (getAll == "1")
                {
                    var allMedia = await this.context.Medias.ToListAsync();
                    return Ok(allMedia);
                }

                var media = this.context.Medias
                    .Where(m => m.MediaType == type);

                if (!(search is null) && search.Length > 1)
                {
                    var exatchStarsNumber = Regex.Match(search, "^[1-5] stars{1}$");
                    var atLeastStars = Regex.Match(search, "^at least [1-5] stars{1}$");
                    var lessThanStars = Regex.Match(search, "^less than [1-5] stars$");
                    var afterThatYear = Regex.Match(search, "^after \\d{4}$");
                    var beforeThatYear = Regex.Match(search, "^before \\d{4}$");
                    var olderThanYearsNumber = Regex.Match(search, "^older than \\d{1,2} years$");
                    if (exatchStarsNumber.Success)
                    {
                        var number = Convert.ToDouble(search.Split(" ")[0]);
                        media = media.Where(m => m.OverallRating == number);
                    } 
                    else if (atLeastStars.Success)
                    {
                        var number = Convert.ToDouble(search.Split(" ")[2]);
                        media = media.Where(m => m.OverallRating >= number);
                    }
                    else if (lessThanStars.Success)
                    {
                        var number = Convert.ToDouble(search.Split(" ")[2]);
                        media = media.Where(m => m.OverallRating < number);
                    }
                    else if(afterThatYear.Success)
                    {
                        var year = Convert.ToInt32(search.Split(" ")[1]);
                        media = media.Where(m => m.ReleaseDate.Year > year);
                    }
                    else if (beforeThatYear.Success)
                    {
                        var year = Convert.ToInt32(search.Split(" ")[1]);
                        media = media.Where(m => m.ReleaseDate.Year < year);
                    }
                    else if (olderThanYearsNumber.Success)
                    {
                        var numberOfYears = Convert.ToInt32(search.Split(" ")[2]);
                        var date = DateTime.Now.AddMilliseconds(-(numberOfYears * 31556952000));
                        media = media.Where(m => DateTime.Compare( m.ReleaseDate, date) < 0);
                    }
                    else
                    {
                        var mediaTemp = media;
                        media = media.Where(m => m.Title.Contains(search) || m.Description.Contains(search) ||  m.MediaActors.Where(ma => ma.Actor.Name.Contains(search) || ma.Actor.Surname.Contains(search)).Count() > 0).Include(m => m.MediaActors).ThenInclude(ma => ma.Actor);
                    }
                }
                var filteredMedia = await media.OrderByDescending(m => m.OverallRating)
               .Skip((pagination.PageNumber - 1) * Pagination.PageSize)
               .Take(Pagination.PageSize).Include(m => m.MediaActors).ThenInclude(ma => ma.Actor)
               .ToListAsync();
                if (filteredMedia is null)
                {
                    return BadRequest();
                }

                var paginatedMedia = new PaginatedMedia() {
                    Medias = filteredMedia,
                    CurrentPage = pagination.PageNumber,
                    NextPage = pagination.PageNumber + 1,
                };
                return Ok(paginatedMedia);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var media = await context.Medias.Where(m => m.ID == id)
                    .Include(m => m.MediaActors)
                    .ThenInclude(ma => ma.Actor)
                    .FirstOrDefaultAsync();
                return Ok(media);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewMediaDTO newMediaDTO)
        {
            try
            {
                var media = MediaFactory.Create(newMediaDTO);
                if (media.MediaActors.Count < 2)
                {
                    return BadRequest("Every media must have at least 2 actors in cast");
                }
                await this.context.Medias.AddAsync(media);
                await this.context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPost("{id}/Rating")]
        
        public async Task<IActionResult> MediaRating([FromRoute(Name = "id")] int id, [FromBody] int ratingNum)
        {
            try
            {
                if (ratingNum > 5 || ratingNum < 1)
                {
                    return BadRequest($"Ratings can not be lower than 1 or higher than 5");
                }
                var media = await this.context.Medias.FindAsync(id);
                if (media is null)
                {
                    return BadRequest($"Media with id {id} does not exist.");
                }
                await context.Ratings.AddAsync(new Rating { Media = media, RatingNum = ratingNum });
                await this.context.SaveChangesAsync();
                var mediaWithRatings = await this.context.Medias.Where(m => m.ID == id).Include(m => m.Ratings).FirstAsync();
                var totalSum = mediaWithRatings.Ratings.Sum(r => r.RatingNum);
                double overallRating = (double) totalSum / (double) mediaWithRatings.Ratings.Count();
                overallRating = Math.Round(overallRating, 2);
                mediaWithRatings.OverallRating = overallRating;
                await this.context.SaveChangesAsync();

                return Ok(mediaWithRatings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file.ContentType != "image/png" && file.ContentType != "image/jpeg" && file.ContentType != "image/jpg" && file.ContentType != "image/webp")
                {
                    return BadRequest("Invalid file format. Accepted file formats are jpeg, jpg, png");
                }
                var timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
                var fileName = $"{timestamp}{file.FileName}";
                var path = $"{env.ContentRootPath}/MediaImages/{fileName}";
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    return Ok(new {fileName = fileName});
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



    }
}
