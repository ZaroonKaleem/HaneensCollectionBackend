using HaneensCollection.DTOs;
using HaneensCollection.IServices;
using HaneensCollection.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HaneensCollection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllSuitsController : ControllerBase
    {
        private readonly IStitchedSuitService _stitchedService;
        private readonly IUnstitchedSuitService _unstitchedService;

        public AllSuitsController(IStitchedSuitService stitchedService, IUnstitchedSuitService unstitchedService)
        {
            _stitchedService = stitchedService;
            _unstitchedService = unstitchedService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ProductDto>>> GetAllSuits()
        {
            var stitchedSuits = await _stitchedService.GetAllAsync();
            var unstitchedSuits = await _unstitchedService.GetAllAsync();

            var allSuits = new List<ProductDto>();
            allSuits.AddRange(stitchedSuits);
            allSuits.AddRange(unstitchedSuits);

            return Ok(allSuits);
        }
    }
}
