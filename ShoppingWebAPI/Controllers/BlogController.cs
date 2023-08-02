using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private BlogRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public BlogController(BlogRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            _mapper = mapper;
        }
        [HttpGet("list")]
        public async Task<ActionResult<BlogDTO>> GetTopBlogs()
        {
            IEnumerable<Blog> BlogList = await repository.GetAllAsync(includeProperties: "User,Category");
            var result = BlogList.OrderByDescending(p => p.CreatedTime).Take(4).ToList();
            List<BlogDTO> listDTO = _mapper.Map<List<BlogDTO>>(result);

            return Ok(listDTO);
        }
        
    }
}
