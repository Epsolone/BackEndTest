using AutoMapper;
using BackEndApplication.Responses;

namespace BackEndApplication.Models
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Image, ImageResponse>();
        }
    }
}
