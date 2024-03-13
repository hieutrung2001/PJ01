using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PJ01.Domain.Config;
using PJ01.Domain.Entities;
using PJ01.Domain.Entities.Identity;

namespace PJ01.Domain.Context
{
    public class PJ01Context : IdentityDbContext<User>
    {
        public PJ01Context() { }
        public PJ01Context(DbContextOptions<PJ01Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ClassConfig());
            builder.ApplyConfiguration(new StudentConfig());
            builder.ApplyConfiguration(new StudentClassConfig());

            builder.Entity<StudentClass>().HasKey(sc => new
            {
                sc.StudentId,
                sc.ClassId
            });
            builder.Entity<StudentClass>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentClasses)
                .HasForeignKey(sc => sc.StudentId);


            builder.Entity<StudentClass>()
                .HasOne(sc => sc.Class)
                .WithMany(s => s.StudentClasses)
                .HasForeignKey(sc => sc.ClassId);
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<StudentClass> StudentClasses { get; set; }
    }
}
