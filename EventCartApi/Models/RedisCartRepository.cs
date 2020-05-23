using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCartApi.Models
{
    // Didn't need this when we did catalog with EF
    // Cart Api -> Redis
    // Interface is for if we decide to use a different
    // data store than Redis

    // interface is a shim layer
    // cart api only talks to interface

    public class RedisCartRepository : ICartRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;


        // constructor:
        // set up di
        // ConnectionMultiplexer is like DbContextOptions
        // for CatalogContext in event catalog api (which was EF)

        public RedisCartRepository(ConnectionMultiplexer redis)
        { // Param: Where redis is stored 
            _redis = redis; // redis is high-level service
            _database = _redis.GetDatabase();
            // This gives you the IDatabase
        }


        // third method written
        public async Task<bool> DeleteCartAsync(string id)
        {
            // discard the cart
            // remember is dictionary
            // specify key, discard the value
            return await _database.KeyDeleteAsync(id);
        }


        // first written
        public async Task<Cart> GetCartAsync(string cartId)
        {
            // ask db, give me a string
            // what data? look up by cart id
            // find a cache with cart id
            // if finds it, store in data
            var data = await _database.StringGetAsync(cartId);
            if (data.IsNullOrEmpty) // data is a redis value
                // it's a kvp pair
            {
                return null; // no data for you
            }

            // if found something
            // deserialize back into a cart type
            // returns a cart object
            return JsonConvert.DeserializeObject<Cart>(data);
            // reuse the newtonsoft ver from other project
            // in solution
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServer();
            // remember, the server is a layer above the db

            var data = server.Keys();
            // show me all the keys in the server
            // IEnumerable<RedisKey>
            // the userids that we have been putting into our store


            //return data == null ? null : data.Select(k => k.ToString());
            return data?.Select(k => k.ToString());
            // quick null check
            // if data not null, do the thing
            // if data is null, then return null
            // shortened version of ternary operator
        }
        // compare how fast and easy to set up redis vs.
        // EF and SQL server (as we did for catalog ms)
        // now on to startup


        // redis provides feature
        // locate the nearest server to you
        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            // an endpoint would be a server in a specific place
            // mexico, dublin, india, etc.


            // First usually gives you the geographically closest
            // but not guaranteed
            return _redis.GetServer(endpoint.First());
        }



        // when add to cart
        // second method written
        public async Task<Cart> UpdateCartAsync(Cart basket)
        {
            // kvp
            // serialize into string
            // when deserialize, have to say into what type
            // but when you serialize, you don't need to say
            // what to serialize it into, it will be serialized
            // into a string

            var created = await _database.StringSetAsync(basket.BuyerId,
                JsonConvert.SerializeObject(basket));
            // redis cache lightweight compared to EF/SQL

            if (!created) // if failed
                // no point in throwing an exception
                // would crash api behind the scenes
                // how does that help, you wouldn't know
                // logging added soon
            {
                return null;
            }


            // if written successfully
            // call the other method we just wrote
            return await GetCartAsync(basket.BuyerId);
        }
    }
}
