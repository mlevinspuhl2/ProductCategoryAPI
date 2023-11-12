using AutoMapper;
using ProductCategoryAPI.models;

namespace ProductCategoryAPI.Services
{
    public class BasicService
    {
        public  MongoDBContext _context;
        public IMapper _mapper;

        public BasicService(MongoDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
