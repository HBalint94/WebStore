using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public static class DbInitializer
    {
        private static WebStoreContext context;
        public static void Initialize(WebStoreContext _context)
        {


            context = _context;
            context.Database.EnsureCreated();
            

            if (context.Categories.Any())
            {
                return; // Az adatbázist már inicializáltnak vesszük, ha létezik kategoria
            }

            SeedCategories();
            SeedProducts();
            SeedAdministrators();
            SeedRents();
            SeedRentProductConnections();
        }

        private static void SeedCategories()
        {
            var categories = new Category[]
            {
                new Category {Name = "Technical"},
                new Category {Name = "Domestic"},
            };
            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();
        }

        private static void SeedProducts()
        {
            var products = new Product[]
            {
                new Product
                {
                    Producer = "Apple",
                    ModellNumber = 124,
                    Description = "Iphone8",
                    CategoryId = 1,
                    Price = 200000,
                    Inventory = 10,
                    Available = true,
                },
                new Product
                {
                    Producer = "Apple",
                    ModellNumber = 125,
                    Description = "IphoneX",
                    CategoryId = 1,
                    Price = 300000,
                    Inventory = 5,
                    Available = true,
                },
                new Product
                {
                    Producer = "Apple",
                    ModellNumber = 126,
                    Description = "Iphone6S",
                    CategoryId = 1,
                    Price = 140000,
                    Inventory = 15,
                    Available = true,
                },
                new Product
                {
                    Producer = "Apple",
                    ModellNumber = 127,
                    Description = "IphoneSE",
                    CategoryId = 1,
                    Price = 90000,
                    Inventory = 20,
                    Available = true,
                },
                new Product
                {
                    Producer = "Indesit",
                    ModellNumber = 10,
                    Description = "IWSC 51051 C ECO mosógép",
                    CategoryId = 2,
                    Price = 56000,
                    Inventory = 11,
                    Available = true,
                },
                new Product
                {
                    Producer = "Indesit",
                    ModellNumber = 11,
                    Description = "IWSC 10000 C ECO mosógép",
                    CategoryId = 2,
                    Price = 45000,
                    Inventory = 12,
                    Available = true,
                }

            };
            foreach(Product p in products)
            {
                context.Products.Add(p);
            }

            context.SaveChanges();
        }

        private static void SeedAdministrators()
        {
            var administrators = new Administrator[]
            {
                new Administrator
                {
                    FullName = "Kovács Péter",
                    UserName = "Petike100",
                    Password = "kovika1992",
                },
                new Administrator
                {
                    FullName = "Pető Sándor",
                    UserName = "Sanyi11",
                    Password = "sanyika122",
                }
            };
            foreach(Administrator a in administrators)
            {
                context.Administrators.Add(a);
            }
        }
        private static void SeedRents()
        {
            var rents = new Rent[]
            {
                new Rent
                {
                    ClientName="Kis Pista",
                    ClientEmail="kis@freemail.hu",
                    ClientAddress= "Budapest , Kiserdő utca 15.",
                    ClientPhoneNumber = "06708231239",
                    Performed = true,
                }
            };
            foreach(Rent r in rents)
            {
                context.Rents.Add(r);
            }
        }
        private static void SeedRentProductConnections()
        {
            var rentProctConnections = new RentProductConnection[]
            {
                new RentProductConnection
                {
                    RentId = 1,
                    ProductModellNumber = 11,
                    CountProduct = 5,
                },
                new RentProductConnection
                {
                    RentId = 1,
                    ProductModellNumber = 127,
                    CountProduct = 2,
                }
            };
            foreach(RentProductConnection r in rentProctConnections)
            {
                context.RentProductConnections.Add(r);
            }
        }

    }
}
