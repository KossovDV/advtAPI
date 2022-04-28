using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace advtAPI
{
    public class AdsContext:DbContext
    {

        public DbSet<Ads> AdsTable { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            optionsBuilder.UseSqlServer(config["ConnectionStrings:DefaultConnection"]);
        }
    }


}
