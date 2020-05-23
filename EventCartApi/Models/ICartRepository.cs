using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCartApi.Models
{
    // Things that you can do on your Redis
    public interface ICartRepository
    {
        // Given cart id, get the cart back
        Task<Cart> GetCartAsync(string cartId);

        // Not meant to be used by users
        // Show me all the users who have the carts
        // For debugging
        IEnumerable<string> GetUsers();

        // You have a cart
        // Want to keep updating the cart
        Task<Cart> UpdateCartAsync(Cart basket);

        // User cleans up cart
        // Or convert to order
        Task<bool> DeleteCartAsync(string id);
    }
}
