using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventCartApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventCartApi.Controllers
{
    // Yesterday: CartApi start

    // What kind of apis to write?
    // what to call from webmvc?

    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        // set up dependency injection
        // specifies the data store interface for the cart api
        private readonly ICartRepository _repository;
        public CartController(ICartRepository repository)
        {
            _repository = repository;
        }


        // option: httpget and route combined together
        // (as did with piccontroller)
        [HttpGet("{id}")]
        // you can say what type of data is being serialized and sent over
        // note that the return type of each api method is always an IActionResult
        // (wrapped in a Task<> if it is an async method)
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        // this api produces an object of type Cart and a 200 status
        // this is an added benefit (what uses this attribute)
        public async Task<IActionResult> Get(string id)
            // get cart by user id
        {
            // call my repository
            var basket = await _repository.GetCartAsync(id);
            return Ok(basket);

            // controller simply a passthru
        }



        // webmvc if already have a cart
        // think about sending only the deltas?
        // what data to pass to microservice
        // if only pass delas, then ms has a lot of work to do
        // better to send a snapshot
        // replace old cart with new cart
        // better performance, cleaner code
        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        // pass back new cart with 200 code
        public async Task<IActionResult> Post([FromBody]Cart value)
            // cart data comes from body
        {
            var basket = await _repository.UpdateCartAsync(value);
            return Ok(basket);
        }



        // here combining route with parameter in attribute
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _repository.DeleteCartAsync(id);
        }

        // done with cart conroller

        // first to dockerfile, then to startup
    }
}