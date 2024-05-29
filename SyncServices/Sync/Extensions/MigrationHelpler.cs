using Microsoft.EntityFrameworkCore;
using Sync.Model;

namespace Sync.Extensions
{
    public static class MigrationHelpler
    {
        public static IApplicationBuilder MigrationDB(this IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                using(var context = scope.ServiceProvider.GetRequiredService<EFDataContext>())
                {
                    context.Database.Migrate();
                }
            }
            return app;
        }
    }
}
