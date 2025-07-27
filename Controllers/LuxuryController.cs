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
    public class LuxuryController : ControllerBase
    {
        private readonly ILuxuryService _luxuryService;

        public LuxuryController(ILuxuryService luxuryService)
        {
            _luxuryService = luxuryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LuxuryDto>>> GetAll()
        {
            var items = await _luxuryService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LuxuryDto>> GetById(Guid id)
        {
            var item = await _luxuryService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LuxuryDto dto)
        {
            var created = await _luxuryService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] LuxuryDto dto)
        {
            var updated = await _luxuryService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _luxuryService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
