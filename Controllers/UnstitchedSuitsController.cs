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
    public class UnstitchedSuitsController : ControllerBase
    {
        private readonly IUnstitchedSuitService _unstitchedSuitService;

        public UnstitchedSuitsController(IUnstitchedSuitService unstitchedSuitService)
        {
            _unstitchedSuitService = unstitchedSuitService;
        }

        // GET: api/unstitchedsuits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnstitchedSuitDto>>> GetAll()
        {
            var suits = await _unstitchedSuitService.GetAllAsync();
            return Ok(suits);
        }

        // GET: api/unstitchedsuits/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UnstitchedSuitDto>> GetById(Guid id)
        {
            var suit = await _unstitchedSuitService.GetByIdAsync(id);
            if (suit == null)
                return NotFound();

            return Ok(suit);
        }

        // POST: api/unstitchedsuits
        [HttpPost]
        public async Task<IActionResult> PostUnstitchedSuit([FromBody] UnstitchedSuitDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdSuit = await _unstitchedSuitService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdSuit.ProductId }, createdSuit);
        }

        // PUT: api/unstitchedsuits/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UnstitchedSuitDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _unstitchedSuitService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/unstitchedsuits/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _unstitchedSuitService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
