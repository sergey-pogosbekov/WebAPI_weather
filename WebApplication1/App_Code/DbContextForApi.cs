using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.App_Code
{
    public class Favorite
    {
        [Key]
        public Guid idFav { get; set; }
        public CityInfo cityInfo { get; set; }

        [ForeignKey("cityInfo")]
        public string cityInfoId { get; set; }

        [NotMapped]
        public double? metricValue { get; set; }

        [NotMapped]
        public string description { get; set; }
    }

    public class DbContextForApi: DbContext
    {
        public DbSet<WeatherInfo> WeathersDetails { get; set; }
        public DbSet<CityInfo> Cities { get; set; }

        public DbSet<Favorite> Favorites{ get; set; }

        public DbContextForApi(DbContextOptions<DbContextForApi> options)
    : base(options)
        {
        }
        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherInfo>().Property(e => e.Id)
            .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
        }
        #endregion
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=WeatherAPI_DB;User ID=sa;Password=z1ON0101");
        }
    }
}
