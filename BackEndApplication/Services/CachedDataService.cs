using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEndApplication.Interfaces;
using BackEndApplication.Models;

namespace BackEndApplication.Services
{
    public class CachedDataService : ICachedDataService
    {
        private static List<Image> _images;
        private static bool _dataLoaded;

        public void CacheImages(List<Image> images)
        {
            _images = images;
            _dataLoaded = true;
        }

        public void LoadingFailed()
        {
            if (!_dataLoaded)
            {
                _images = new List<Image>();
                _dataLoaded = true;
            }
        }

        public async Task<List<Image>> SearchImagesByFilter(string filter)
        {
            do
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            while (!_dataLoaded);

            return _images
                .Where(x => (x.Author != null && x.Author.ToLower().Contains(filter.ToLower()))
                    || (x.Camera != null && x.Camera.ToLower().Contains(filter.ToLower()))
                    || (x.Tags != null && x.Tags.ToLower().Contains(filter.ToLower())))
                .ToList();
        }
    }
}
