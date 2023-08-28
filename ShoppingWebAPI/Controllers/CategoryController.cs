using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingWebAPI.Config;

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
            CategoryList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "Products");
            List<CategoryDTO> listDTO = _mapper.Map<List<CategoryDTO>>(CategoryList);

            return Ok(listDTO);
        }

        [HttpGet("getAllForAdmin")]
        public async Task<ActionResult<CategoryDTO>> GetAllCategorysForAdmin()
        {
            IEnumerable<Category> CategoryList;
            CategoryList = await repository.GetAllAsync(includeProperties: "Products");
            List<CategoryDTO> listDTO = _mapper.Map<List<CategoryDTO>>(CategoryList);

            return Ok(listDTO);
        }

        [HttpGet("{category_id:int}", Name = "getCategory")]
        public async Task<ActionResult<CategoryDTO>> GetOneCategory(int category_id)
        {

            if (category_id == 0)
            {

                return BadRequest();
            }
            var Category = await repository.GetOneAsync(x => x.CategoryId == category_id, includeProperties: "Products");

            if (Category == null)
            {

                return NotFound();
            }

            CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(Category);

            return Ok(categoryDTO);
        }

        [HttpPut("{Category_id:int}", Name = "UpdateCategory")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int Category_id, [FromBody] CategoryDTO categoryDTO)
        {

            if (categoryDTO == null || Category_id != categoryDTO.CategoryId)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<CategoryDTO, Category>();
            var categoryCreate = mapper.Map<Category>(categoryDTO);

            // Thực hiện tạo mới Movie
            await repository.UpdateAsync(categoryCreate);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {

            if (categoryDTO == null)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<CategoryDTO, Category>();
            var categoryCreate = mapper.Map<Category>(categoryDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(categoryCreate);

            return Ok();
        }
    }
}
