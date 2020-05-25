using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventOrderApi.Models;

// For DbContext (used local version)
using Microsoft.EntityFrameworkCore;

namespace EventOrderApi.Data
{
    public class OrdersContext : DbContext
    {   
        // WHERE:
        // Required by Startup
        // Startup sets the context
        // When the framework constructs this OrdersContext, it will
        // inject a DbContextOptions specifying the options that we specified
        // in Startup:
        //services.AddDbContext<OrdersContext>(options =>
        //        options.UseSqlServer(connectionString));
        public OrdersContext(DbContextOptions options) : base(options)
        // Recall the purpose of this constructor is to pass along the
        // options specifying the connection string, etc. to the DbContext
        // base class constructor
        // (Pass-thru from Startup to DbContext)
        {

        }
        // The parameterized constructor is one half of the WHERE
        // (the receiving half)
        // The other half of the were is in Startup when we set up the 
        // dependency injection.
        // Kal forgot this when we were writing this class
        // We did this later, when the attempt to Add-Migration Initial failed



        //WHAT:
        // Only two tables need are Orders table and OrderItems table
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        // HOW:
        // Don't need to write special how instructions here
        // Those were provided in the class definitions for Orders and OrderItems
        // Used DataAnnotation attributes to specify primary key (and UseHiLo):
        // [Key]

        // Also autogen and unique:
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        // So, with EF, you can do the HOW either here with lambda statements
        // or in the definitions for the class types that are contained in the
        // DbSet<> tables

        // Now on to migrations
        // Note that we aren't seeding the order db with dummy data
        // But, recall one thing that we had in the catalog seed file
        // was to kick off the migrations if they were prepared and ready to go
        // "UpdateDatabase" if migrations already added and migrations just
        // need to be updated in the database...that's what "Migrate" means...
        // Migrate means Updtate, not AddMigration

        // Recall two things need do with Entity Framework EF:
        // After bulid your models
    }
}
