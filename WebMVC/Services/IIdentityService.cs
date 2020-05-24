using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebMVC.Services
{
    public interface IIdentityService<T>
    {
        // Get: To get your currently logged in user
        // Then look to implementation
        T Get(IPrincipal principal);
    }
}
