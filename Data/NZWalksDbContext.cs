using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data
{
    public class NZWalksDbContext: DbContext 
    {
          public NZWalksDbContext(DbContextOptions <NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
          {
            
          }

          public DbSet<Difficulty> Difficulties { get; set; }
          public DbSet<Region> Regions {get; set;}
          public DbSet<Walk> Walks {get; set;}
          public DbSet<Image> Images {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed data for difficulties
            //Easy Medium , Hard

            var difficulties = new List<Difficulty>() {
                 new Difficulty() {
                    Id = Guid.Parse("394640e9-02de-4110-b24b-8a31c35a3089"),
                    Name = "Easy"
                 },
                  new Difficulty() {
                    Id = Guid.Parse("b98516aa-b353-4b41-b2ac-df9e0aa88934"),
                    Name = "Medium"
                 },
                  new Difficulty() {
                    Id = Guid.Parse("e4427790-df45-430e-8885-73b771f76611"),
                    Name = "Hard"
                 },
            };
            //Seed data into database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);


            //Seed data for regions

            var regions = new List<Region> 
            {
                new Region {
                   Id = Guid.Parse("aab91ab1-c743-4c5f-bc8d-0233d1a1de9f"),
                   Name = "Auckland",
                   Code = "AKL",
                   RegionImageUrl = "Image.jpeg"
                },
                 new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                }
            };
            modelBuilder.Entity<Region>().HasData(regions);
        }

    }
}