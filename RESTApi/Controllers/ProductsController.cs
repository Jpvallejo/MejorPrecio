using System.Linq;
using MejorPrecio3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MejorPrecio3.API;
using System;
using MejorPrecio3.API.Services;
using MejorPrecio3.Services;

namespace MejorPrecio3.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private SearchBestPrice api = new SearchBestPrice();

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return StatusCode(200, api.GetAllProducts());
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message());
            }
        }


        //[Authorize]
        [Route("")]
        [HttpPost]
        public ActionResult Create([FromBody]Product product)
        {
            product.Id = Guid.Empty;
            if (api.SaveProduct(product))
            {
                return StatusCode(204);
            }
            return StatusCode(500);
        }


        // [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            // product.product = prod;
            api.DeleteProduct(id);
            return StatusCode(200, "Product deleted successfully");
        }
    }
}