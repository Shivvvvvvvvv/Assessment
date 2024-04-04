using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.TopUp.Infrastructure.DataAccessAbstractions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextFactory<TopUpSystemDbContext>(options =>
                options
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution));

            return services;
        }
    }
}
