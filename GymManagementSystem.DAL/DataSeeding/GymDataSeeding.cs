using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext context, string SeedFolderPath,ILogger logger ,CancellationToken ct = default)
        {
            try
            {
                if(!context.Plans.Any())
                {
                    var plans = LoadDataFromJsonFile<Plan>(SeedFolderPath, "plans.json");
                    if(plans.Any())
                    {
                        await context.Plans.AddRangeAsync(plans);
                        await context.SaveChangesAsync(ct);
                        logger.LogInformation($"Seeding Plans With Count {plans.Count}");
                    }                  
                }

            } 
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }


        public static List<T> LoadDataFromJsonFile<T>(string folderPath, string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))

                throw new FileNotFoundException($"Seed Data File Not Found: {filePath}");
            var data = File.ReadAllText(filePath);
            var option = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<List<T>>(data, option) ?? [];
            return result;
        }

    }
}
