using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class IdentityService : IIdentityService<ApplicationUser>
    {
        // Recall what the About page did when we were first testing the token service

            // Get your current logged in user
        public ApplicationUser Get(IPrincipal principal)
        {
            // If the user is logged in
            if (principal is ClaimsPrincipal claims)
            {
                // QUESTION:
                // I suspect that this entire user declaration can be deleted
                // otherwise, simply return user below...
                // TODO:
                // Test that this works well without
                var user = new ApplicationUser()
                {

                    Email = claims.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? "",

                };

                // Create a new ApplicationUser
                return new ApplicationUser
                {
                    // Grab thier preferred username
                    Email = claims.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "",
                    Id = claims.Claims.FirstOrDefault(x => x.Type == "sub")?.Value ?? "",
                    // "sub" contains their id
                };

            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
