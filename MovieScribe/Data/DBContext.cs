using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MovieScribe.Models;

namespace MovieScribe.Data
{
    public class DBContext : IdentityDbContext<AppUser>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActorsSUBMediaModel>().HasKey(am => new
            {
                am.ActorID,
                am.MediaID
            });

            modelBuilder.Entity<ActorsSUBMediaModel>().HasOne(m => m.Media).WithMany(am => am.Actor_SUB_Media).HasForeignKey(m => m.MediaID);
            modelBuilder.Entity<ActorsSUBMediaModel>().HasOne(m => m.Actor).WithMany(am => am.Actor_SUB_Media).HasForeignKey(m => m.ActorID);

            modelBuilder.Entity<GenresSUBMediaModel>().HasKey(gm => new
            {
                gm.GenreID,
                gm.MediaID
            });

            modelBuilder.Entity<GenresSUBMediaModel>().HasOne(m => m.Media).WithMany(gm => gm.Genre_SUB_Media).HasForeignKey(m => m.MediaID);
            modelBuilder.Entity<GenresSUBMediaModel>().HasOne(m => m.Genre).WithMany(gm => gm.Genre_SUB_Media).HasForeignKey(m => m.GenreID);

            modelBuilder.Entity<LanguagesSUBMediaModel>().HasKey(lm => new
            {
                lm.LanguageID,
                lm.MediaID
            });

            modelBuilder.Entity<LanguagesSUBMediaModel>().HasOne(m => m.Media).WithMany(lm => lm.Language_SUB_Media).HasForeignKey(m => m.MediaID);
            modelBuilder.Entity<LanguagesSUBMediaModel>().HasOne(m => m.Language).WithMany(lm => lm.Language_SUB_Media).HasForeignKey(m => m.LanguageID);

            modelBuilder.Entity<WritersSUBMediaModel>().HasKey(wm => new
            {
                wm.WriterID,
                wm.MediaID
            });

            modelBuilder.Entity<WritersSUBMediaModel>().HasOne(m => m.Media).WithMany(wm => wm.Writer_SUB_Media).HasForeignKey(m => m.MediaID);
            modelBuilder.Entity<WritersSUBMediaModel>().HasOne(m => m.Writer).WithMany(wm => wm.Writer_SUB_Media).HasForeignKey(m => m.WriterID);

            modelBuilder.Entity<PlanToWatchModel>()
                .HasKey(ptw => new { ptw.UserId, ptw.MediaId });

            modelBuilder.Entity<PlanToWatchModel>()
                .HasOne(ptw => ptw.User)
                .WithMany(u => u.PlanToWatchMedia)
                .HasForeignKey(ptw => ptw.UserId);

            modelBuilder.Entity<PlanToWatchModel>()
                .HasOne(ptw => ptw.Media)
                .WithMany(m => m.PlanToWatchMedia)
                .HasForeignKey(ptw => ptw.MediaId);

            modelBuilder.Entity<WatchedModel>()
                .HasKey(w => new { w.UserId, w.MediaId });

            modelBuilder.Entity<WatchedModel>()
                .HasOne(w => w.User)
                .WithMany(u => u.WatchedMedia)
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<WatchedModel>()
                .HasOne(w => w.Media)
                .WithMany(m => m.WatchedMedia)
                .HasForeignKey(w => w.MediaId);

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<ActorModel> Actors { get; set; }
        public DbSet<MediaModel> Media { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<LanguageModel> Languages { get; set; }
        public DbSet<ProducerModel> Producers { get; set; }
        public DbSet<WriterModel> Writers { get; set; }
        public DbSet<StudioModel> Studios { get; set; }



        public DbSet<DistributorModel> Distributors { get; set; }
        public DbSet<ActorsSUBMediaModel> Actors_SUB_Media { get; set; }
        public DbSet<GenresSUBMediaModel> Genres_SUB_Media { get; set; }
        public DbSet<LanguagesSUBMediaModel> Languages_SUB_Media { get; set; }
        public DbSet<WritersSUBMediaModel> Writers_SUB_Media { get; set; }
        public DbSet<PlanToWatchModel> PlanToWatch { get; set; }
        public DbSet<WatchedModel> Watched { get; set; }

    }
}
