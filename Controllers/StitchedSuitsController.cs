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
    public class StitchedSuitsController : ControllerBase
    {
        private readonly IStitchedSuitService _stitchedSuitService;

        public StitchedSuitsController(IStitchedSuitService stitchedSuitService)
        {
            _stitchedSuitService = stitchedSuitService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StitchedSuitDto>>> GetAll()
        {
            var suits = await _stitchedSuitService.GetAllAsync();
            return Ok(suits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StitchedSuitDto>> GetById(Guid id)
        {
            var suit = await _stitchedSuitService.GetByIdAsync(id);
            if (suit == null)
                return NotFound();

            return Ok(suit);
        }

        [HttpPost]
        public async Task<IActionResult> PostStitchedSuit([FromBody] StitchedSuitDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdSuit = await _stitchedSuitService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdSuit.ProductId }, createdSuit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StitchedSuitDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _stitchedSuitService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _stitchedSuitService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
