using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndApplication.Models;

namespace BackEndApplication.Interfaces
{
    public interface ICachedDataService
    {
        Task<List<Image>> SearchImagesByFilter(string filter);

        void CacheImages(List<Image> images);

        void LoadingFailed();
    }
}
