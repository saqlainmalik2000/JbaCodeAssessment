using JbaCodeAssessment.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JbaCodeAssessment.Data
{
    public class JbaContext : DbContext
    {
        public JbaContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ReportData> ReportData { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //{
        //    //builder.UseSqlite("Data Source=JbaDb.db;");
        //    builder.UseInMemoryDatabase("JbaDb");
        //}

        /// <summary>
        /// This is a limited list of declarations, and would be expanded in a production system
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ReportData>()
                .Property(x => x.XRef)
                .IsRequired();

            builder.Entity<ReportData>()
                .Property(x => x.YRef)
                .IsRequired();

            builder.Entity<ReportData>()
                .Property(x => x.Value)
                .IsRequired();
        }
    }
}
