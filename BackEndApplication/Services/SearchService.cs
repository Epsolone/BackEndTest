using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndApplication.Interfaces;
using BackEndApplication.Models;

namespace BackEndApplication.Services
{
    public class SearchService : ISearchService
    {
        private readonly ICachedDataService _cachedDataService;

        public SearchService(ICachedDataService cachedDataService)
        {
            _cachedDataService = cachedDataService;
        }

        public async Task<List<Image>> FindImages(string filter)
        {
            return await _cachedDataService.SearchImagesByFilter(filter);
        }
    }
}
