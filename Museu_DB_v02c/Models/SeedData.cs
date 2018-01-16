using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Museu_DB_v02c.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Museu_DB_v02cContext(
                serviceProvider.GetRequiredService<DbContextOptions<Museu_DB_v02cContext>>()))
            {
                // Look for any movies.
                if (context.Visitor.Any())
                {
                    return;   // DB has been seeded
                }

                DateTime dateTotal = DateTime.Now;
                DateTime dateOnly = dateTotal.Date;

                context.Visitor.AddRange(
                    new Visitor
                    {
                        Age_Group = 45,
                        Nationality = "Finish",
                        Gender = "Female",
                        Date = dateOnly
                    },

                    new Visitor
                    {
                        Age_Group = 25,
                        Nationality = "Spanish",
                        Gender = "Female",
                        Date = dateOnly
                    },

                    new Visitor
                    {
                        Age_Group = 33,
                        Nationality = "Portuguese",
                        Gender = "Male",
                        Date = dateOnly
                    },

                    new Visitor
                    {
                        Age_Group = 29,
                        Nationality = "Irish",
                        Gender = "Male",
                        Date = dateOnly
                    },

                new Visitor
                {
                    Age_Group = 45,
                    Nationality = "Japanese",
                    Gender = "Female",
                    Date = dateOnly
                },

                new Visitor
                {
                    Age_Group = 25,
                    Nationality = "Italian",
                    Gender = "Female",
                    Date = dateOnly
                },

                new Visitor
                {
                    Age_Group = 33,
                    Nationality = "British",
                    Gender = "Male",
                    Date = dateOnly
                },

                new Visitor
                    {
                        Age_Group = 29,
                        Nationality = "French",
                        Gender = "Female",
                        Date = dateOnly
                    }
                );
                context.SaveChanges();
            }
        }

    }
}
