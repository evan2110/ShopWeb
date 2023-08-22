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
            IEnumerable<Blog> BlogList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "User,Category");
            var result = BlogList.OrderByDescending(p => p.CreatedTime).Take(4).ToList();
            List<BlogDTO> listDTO = _mapper.Map<List<BlogDTO>>(result);

            return Ok(listDTO);
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<BlogDTO>> GetAllBlogs([FromQuery(Name = "categoryId")] int? categoryId, [FromQuery(Name = "search")] string? search, int pageSize = 0, int pageNumber = 1)
        {
            IEnumerable<Blog> BlogList;
            BlogList = await repository.GetAllAsync(e => e.Status == "Active", includeProperties: "Category,User", pageSize: pageSize, pageNumber: pageNumber);
            if (categoryId != null && search != null)
            {
                BlogList = await repository.GetAllAsync(e => e.Status == "Active" && e.CategoryId == categoryId && e.BlogName.Contains(search), includeProperties: "Category,User", pageSize: pageSize, pageNumber: pageNumber);
            }
            if(categoryId != null && search == null)
            {
                BlogList = await repository.GetAllAsync(e => e.Status == "Active" && e.CategoryId == categoryId, includeProperties: "Category,User", pageSize: pageSize, pageNumber: pageNumber);
            }
            if(categoryId == null && search != null)
            {
                BlogList = await repository.GetAllAsync(e => e.Status == "Active" && e.BlogName.Contains(search), includeProperties: "Category,User", pageSize: pageSize, pageNumber: pageNumber);
            }
            List<BlogDTO> listDTO = _mapper.Map<List<BlogDTO>>(BlogList);

            return Ok(listDTO);
        }

        [HttpGet("{blog_id:int}", Name = "getBlog")]
        public async Task<ActionResult<BlogDTO>> GetOneBlog(int blog_id)
        {

            if (blog_id == 0)
            {

                return BadRequest();
            }
            var Blog = await repository.GetOneAsync(x => x.BlogId == blog_id, includeProperties: "Category,User");

            if (Blog == null)
            {

                return NotFound();
            }

            BlogDTO blogDTO = _mapper.Map<BlogDTO>(Blog);

            return Ok(blogDTO);
        }

        [HttpGet("lstRelate")]
        public async Task<ActionResult<BlogDTO>> GetRelatedBlogs([FromQuery(Name = "categoryId")] int? categoryId)
        {
            IEnumerable<Blog> BlogList = await repository.GetAllAsync(e => e.Status == "Active" && e.CategoryId == categoryId, includeProperties: "User,Category");
            var result = BlogList.OrderByDescending(p => p.CreatedTime).Take(3).ToList();
            List<BlogDTO> listDTO = _mapper.Map<List<BlogDTO>>(result);

            return Ok(listDTO);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BlogDTO>> CreateBlog([FromBody] BlogDTO blogDTO)
        {

            if (await repository.GetOneAsync(x => x.BlogId == blogDTO.BlogId) != null)
            {
                return BadRequest();
            }

            // Map nguoc tu DTO -> Entity thi dung AutoMapperConfig
            var mapper = AutoMapperConfig.InitializeAutomapper<BlogDTO, Blog>();
            var blogCreate = mapper.Map<Blog>(blogDTO);

            // Thực hiện tạo mới Movie
            await repository.CreateAsync(blogCreate);

            // Ánh xạ lại đối tượng Movie sau khi tạo thành công
            var resultDTIO = _mapper.Map<BlogDTO>(blogCreate);


            return Ok(resultDTIO);
        }

    }
}
