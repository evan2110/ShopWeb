using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryRepository repository;

        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public CategoryController(CategoryRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<CategoryDTO>> GetAllCategorys()
        {
            IEnumerable<Category> CategoryList;
            CategoryList = await repository.GetAllAsync(e => e.Status == "Active");
            List<CategoryDTO> listDTO = _mapper.Map<List<CategoryDTO>>(CategoryList);

            return Ok(listDTO);
        }
    }
}
