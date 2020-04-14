using System.Collections.Generic;
using System.Threading.Tasks;
using BackEndApplication.Models;

namespace BackEndApplication.Interfaces
{
    public interface ISearchService
    {
        Task<List<Image>> FindImages(string filter);
    }
}
