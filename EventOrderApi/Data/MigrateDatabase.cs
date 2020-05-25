using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Need to insall the nuget packages here in the OrderApi project
// Microsoft.EntityFrameworkCore
//                              .Relational
//                              .Tools
//                              .SqlServer
// Use 3.1.0
// Need to use the same version for everything


// Now must AddMigration from powershell
// Need to change the active project to me this project, EventOrderApi
// Also must be startup project
// (right click on project in solution explorer "Set as StartUp Project")
// Then after done adding migration, set the startup project back to being
// whatever it was before (probably docker-compose)

// Yes, both of these are necessary:
// Default project above the Package Manager Console
// StartUp project
// The project that you want to add migration to must be set for both of these

// Whoops. Forgot to set up the dependency injection
// PM> Add-Migration Initial
//Build started...
//Build succeeded.
//Unable to create an object of type 'OrdersContext'. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728

// The dependency injection was not set up for the OrdersContext class:
// public OrdersContext(DbContextOptions options)

// Go look there now, the Startup.cs for OrderApi

// After that, head to Program.cs to set up the UpdateDatabase automation


namespace EventOrderApi.Data
{
    public static class MigrateDatabase
    {
        public static void EnsureCreated(OrdersContext context)
        {
            context.Database.Migrate();
        }
    }
}
