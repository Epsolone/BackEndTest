using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BackEndApplication.Interfaces;
using BackEndApplication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BackEndApplication.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SearchController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISearchService _searchService;

        public SearchController(IMapper mapper, ISearchService searchService)
        {
            _mapper = mapper;
            _searchService = searchService;
        }

        [HttpGet("{filter}")]
        public async Task<IActionResult> Search(string filter)
        {
            var result = await _searchService.FindImages(filter);
            return Ok(_mapper.Map<List<ImageResponse>>(result));
        }
    }
}