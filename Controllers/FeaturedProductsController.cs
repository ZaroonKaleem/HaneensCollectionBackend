using HaneensCollection.DTOs;
using HaneensCollection.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaneensCollection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturedProductsController : ControllerBase
    {
        private readonly IFeaturedProductService _featuredProductService;

        public FeaturedProductsController(IFeaturedProductService featuredProductService)
        {
            _featuredProductService = featuredProductService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeaturedProductDto>>> GetFeaturedProducts()
        {
            var products = await _featuredProductService.GetFeaturedProductsAsync();
            return Ok(products);
        }

        // GET: api/featuredproducts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FeaturedProductDto>> GetFeaturedProduct(Guid id)
        {
            var product = await _featuredProductService.GetFeaturedProductAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<FeaturedProductDto>> CreateFeaturedProduct(FeaturedProductDto productDto)
        {
            var createdProduct = await _featuredProductService.CreateFeaturedProductAsync(productDto);
            return CreatedAtAction(nameof(GetFeaturedProduct), new { id = createdProduct.ProductId }, createdProduct);
        }

        // PUT: api/featuredproducts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeaturedProduct(Guid id, FeaturedProductDto productDto)
        {
            var result = await _featuredProductService.UpdateFeaturedProductAsync(id, productDto);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // DELETE: api/featuredproducts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeaturedProduct(Guid id)
        {
            var result = await _featuredProductService.DeleteFeaturedProductAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}