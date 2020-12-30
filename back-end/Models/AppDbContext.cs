using Microsoft.EntityFrameworkCore;

namespace Pumpkinmovies.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<TotalInfo> TotalInfo { get; set; }
        public DbSet<MovieTag> MovieTag { get; set; }
        public DbSet<MoviePicture> MoviePicture { get; set; }
        public DbSet<PersonPicture> PersonPicture { get; set; }
        public DbSet<Director> Director { get; set; }
        public DbSet<Star> Star { get; set; }
        public DbSet<LikeComment> LikeComment { get; set; }
        public DbSet<UserPrefer> UserPrefer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieTag>().HasKey(t => new { t.m_id, t.tag_name });
            modelBuilder.Entity<MoviePicture>().HasKey(t => new { t.m_id, t.pic_id });
            modelBuilder.Entity<PersonPicture>().HasKey(t => new { t.person_id, t.pic_id });
            modelBuilder.Entity<Director>().HasKey(t => new { t.m_id, t.person_id });
            modelBuilder.Entity<Star>().HasKey(t => new { t.m_id, t.person_id });
            modelBuilder.Entity<LikeComment>().HasKey(t => new { t.u_id, t.c_id });
            modelBuilder.Entity<UserPrefer>().HasKey(t => new { t.u_id, t.tag_name });

            base.OnModelCreating(modelBuilder);
        }
    }
}
