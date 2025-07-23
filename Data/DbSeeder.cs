using System.Linq;
using PhotoApi.Models;  // Change "PhotoApi" to your actual project name

namespace PhotoApi.Data  // Change "PhotoApi" to your actual project name
{
    public static class DbSeeder
    {
        public static void SeedData(ApplicationDbContext context)
        {
            if (!context.Photos.Any())
            {
                context.Photos.AddRange(
                    new Photo { Name = "Winter", Url = "https://via.placeholder.com/200x150", Width = 200, Height = 150 },
                    new Photo { Name = "Family", Url = "https://via.placeholder.com/1024x768", Width = 1024, Height = 768 }
                );
                context.SaveChanges();
            }
        }
    }
}