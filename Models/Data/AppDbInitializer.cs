using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Models.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();


            }
        }
    }


}
