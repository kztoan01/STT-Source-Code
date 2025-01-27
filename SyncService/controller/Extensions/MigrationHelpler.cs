﻿using data.Data;
using Microsoft.EntityFrameworkCore;

namespace controller.Extensions
{
    public static class MigrationHelpler
    {
        public static IApplicationBuilder MigrationDB(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>())
                {
                    context.Database.Migrate();
                }
            }
            return app;
        }
    }
}
