using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Braille.Data.Models;
using Braille.Data.Models.Dtos;
using Braille.Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrailleAPI.Controllers
{
    [Route("api/v{version:apiVersion}/SpecialSymbols")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class SpecialSymbolsController : Controller
    {
        private readonly ISpecialSymbolsRepository _ssr;
        private readonly IMapper _map;

        public SpecialSymbolsController(
            ISpecialSymbolsRepository ssr,
            IMapper map)
        {
            _ssr = ssr;
            _map = map;
        }

        /// <summary>
        /// Get all symbols.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllSymbols")]
        [ProducesResponseType(200, Type = typeof(List<SpecialSymbolsModelDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetSpecialSymbolsModels()
        {
            var specialSymbolsList = _ssr.GetSpecialSymbolModels();

            //using Dto to avoid exposing our objects directly to the API endpoint
            var ssmDto = new List<SpecialSymbolsModelDto>();
            foreach (var symb in specialSymbolsList)
            {
                ssmDto.Add(_map.Map<SpecialSymbolsModelDto>(symb));
            }

            return Ok(ssmDto);
        }

        /// <summary>
        /// View symbol. 
        /// </summary>
        /// <param name="id">Id of the special symbol</param>
        /// <returns></returns>
        [HttpGet("id:int", Name = "GetSpecialSymbols")]
        [ProducesResponseType(200, Type = typeof(SpecialSymbolsModelDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetSpecialSymbolsModel(int id)
        {
            var ssm = _ssr.GetSpecialSymbolsModel(id);
            if (ssm == null)
            {
                return NotFound();
            }
            var ssmDto = _map.Map<SpecialSymbolsModelDto>(ssm);
            return Ok(ssmDto);
        }

        /// <summary>
        /// Ceate new special symbol.
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "CreateSpecialSymbol")]
        [ProducesResponseType(201, Type = typeof(SpecialSymbolsModelDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateSpecialSymbolsModel([FromBody] SpecialSymbolsModelDto ssmDto)
        {
            if (ssmDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ssr.SpecialSymbolsModelExists(ssmDto.SymbolName))
            {
                ModelState.AddModelError("", "specialSymbolsModel already in system");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ssmObj = _map.Map<SpecialSymbolsModel>(ssmDto);

            if (!_ssr.CreateSpecialSymbolsModel(ssmObj))
            {
                ModelState.AddModelError("", $"Unable to add {ssmObj.SymbolName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("CreateSpecialSymbol",
                new { version = HttpContext.GetRequestedApiVersion().ToString(),
                    id = ssmObj.Id }, ssmObj);
        }

        /// <summary>
        /// Update braille symbol. 
        /// </summary>
        /// <param name="id">Id of the braille symbol</param>
        /// <param name="ssmDto">Dto for the braille symbol</param>
        /// <returns></returns>
        [HttpPatch("{id:int}", Name = "UpdateSpecialSymbol")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateSpecialSymbolsModel(int id, [FromBody] SpecialSymbolsModelDto ssmDto)
        {
            if (ssmDto == null || id != ssmDto.Id)
            {
                return BadRequest(ModelState);
            }

            var ssmObj = _map.Map<SpecialSymbolsModel>(ssmDto);

            if (!_ssr.UpdateSpecialSymbolsModel(ssmObj))
            {
                ModelState.AddModelError("", $"Unable to update {ssmObj.SymbolName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        /// <summary>
        /// Delete symbol. 
        /// </summary>
        /// <param name="id">Id of the braille symbol</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DelteSpecialSymbol")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteSpecialSymbolsModel(int id)

        {
            if (!_ssr.SpecialSymbolsModelExists(id))
            {
                return NotFound();
            }

            var ssmObj = _map.Map<SpecialSymbolsModel>(id);

            if (!_ssr.DeleteSpecialSymbolsModel(ssmObj))
            {
                ModelState.AddModelError("", $"Unable to delete {ssmObj.SymbolName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

    }
}