using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRatingBackend.Contexts;
using MovieRatingBackend.DTOs;
using MovieRatingBackend.Factories;
using MovieRatingBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly Context context;
        public ActorsController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var actors = await context.Actors.ToListAsync();
                if (actors is null)
                {
                    return BadRequest();
                }
                return Ok(actors);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var actor = await this.context.Actors.Where(a => a.ID == id).Include(a => a.MediaActors).FirstAsync();
                if (actor is null)
                {
                    return BadRequest();
                }
                return Ok(actor);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NewActorDTO newActorDTO)
        {
            try
            {
                var actor = ActorsFactory.Create(newActorDTO);
                await this.context.Actors.AddAsync(actor);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
