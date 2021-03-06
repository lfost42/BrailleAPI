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
    [ApiVersion("2.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class SpecialSymbolsV2Controller : Controller
    {
        private readonly ISpecialSymbolsRepository _ssr;
        private readonly IMapper _map;

        public SpecialSymbolsV2Controller(
            ISpecialSymbolsRepository ssr,
            IMapper map)
        {
            _ssr = ssr;
            _map = map;
        }

        /// <summary>
        /// View first symbol (versioning DEMO). 
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetFirstSymbolDemo")]
        [ProducesResponseType(200, Type = typeof(List<SpecialSymbolsModelDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetSpecialSymbolsModels()
        {
            var ssObj = _ssr.GetSpecialSymbolModels().FirstOrDefault();
            return Ok(_map.Map<SpecialSymbolsModelDto>(ssObj));
        }

       

    }
}