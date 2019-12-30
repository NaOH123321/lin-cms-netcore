using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinCms.Core;
using LinCms.Core.Entities;
using LinCms.Infrastructure.Helpers;
using Microsoft.Extensions.Logging;

namespace LinCms.Infrastructure.Database
{
    public class LinContextSeed
    {
        public static async Task SeedAsync(LinContext linContext,
            ILoggerFactory loggerFactory, int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                // TODO: Only run this if using a real database
                // linContext.Database.Migrate();

                if (!linContext.LinUsers.Any())
                {
                    linContext.LinUsers.Add(
                        new LinUser
                        {
                            Username = "super",
                            Email = "1234995678@qq.com",
                            Password = Pbkdf2Encrypt.EncryptPassword("123456"),
                            Admin = (short)UserAdmin.Admin,
                            Active = (short)UserActive.Active
                        }
                    );
                    await linContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<LinContextSeed>();
                    logger.LogError(ex.Message);
                    await SeedAsync(linContext, loggerFactory, retryForAvailability);
                }
            }
        }
    }
}
