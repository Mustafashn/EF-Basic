using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFramework
{
    public class ShopContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseLoggerFactory(MyLoggerFactory)
            .UseMySQL(@"server=localhost;port=3306;database=ShopDb;user=root;password=mysql1234;");
            //  .UseSqlite("Data Source=shop.db");
            // .UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=ShopDb;Integrated Security=SSPI; TrustServerCertificate=True;");

        }
    }
    //Entity Classes
    //Product (Id,Name-Price) => Product(Id,Name,Price)

    public class Product
    {
        //primary key(Id, <type_name>Id)
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }

    }
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime DateAdded { get; set; }
    }
    class Program
    {
        public static void Main(String[] args)
        {
            //  var products = GetProductByName("Samsung");
            // foreach (var item in products)
            // {
            //     Console.WriteLine($"{item.Name} : {item.Price}");
            // }
            // DeleteProduct(7);
            AddProducts();
        }
        static void AddProducts()
        {
            using (var db = new ShopContext())

            {
                var products = new List<Product>() {

                new Product { Name = "Samsung S6", Price = 3000 },
                new Product { Name = "Samsung S7", Price = 4000 },
                new Product { Name = "Samsung S8", Price = 5000 },
                new Product { Name = "Samsung S9", Price = 6000 },
                };
                db.Products.AddRange(products);
                db.SaveChanges();
                Console.WriteLine("Veriler eklendi");
            }
        }
        static List<Product> GetAllProducts()
        {
            using (var db = new ShopContext())
            {
                var products = db
                .Products
                // .Select(p => new
                // {
                //     p.Name,
                //     p.Price
                // })
                .ToList();

                return products;

            }
        }
        static void AddProduct()
        {
            using (var db = new ShopContext())

            {
                var p = new Product { Name = "Samsung S11", Price = 8000 };
                db.Products.Add(p);
                db.SaveChanges();
                Console.WriteLine("Veri eklendi");
            }
        }
        static Product GetProductById(int id)
        {
            using (var db = new ShopContext())
            {
                var product = db.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();
                return product;
            }
        }
        static List<Product> GetProductByName(string name)
        {
            using (var db = new ShopContext())
            {
                var products = db.Products
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToList();
                return products;
            }
        }
        static void UpdateProduct(int id)
        {
            using (var db = new ShopContext())
            {
                //change tracking
                var p = db.Products
                .Where(i => i.Id == id).FirstOrDefault();
                if (p != null)
                {
                    p.Price *= 1.2m;

                    db.SaveChanges();
                    Console.WriteLine("Update successfull");

                    // p.Price=2000;
                    // db.Products.Update(p);
                    // db.SaveChanges();
                }


            }


        }
        static void DeleteProduct(int id)
        {
            using (var db = new ShopContext())
            {
                var p = db.Products.FirstOrDefault(i => i.Id == id);

                if (p != null)
                {
                    db.Products.Remove(p);
                    db.SaveChanges();
                    Console.WriteLine("Deleted product");
                }
            }
        }
    }
}
