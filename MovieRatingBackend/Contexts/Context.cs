using Microsoft.EntityFrameworkCore;
using MovieRatingBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatingBackend.Contexts
{
    public class Context : DbContext
    {
        public DbSet<Media> Medias { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MediaActor> MediaActors { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Media>().HasMany(m => m.Ratings).WithOne(r => r.Media).IsRequired();
            modelBuilder.Entity<MediaActor>().HasKey(ma => new { ma.MediaId, ma.ActorId });

        }
    }
}
