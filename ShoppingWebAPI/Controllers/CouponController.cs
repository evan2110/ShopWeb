using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO;
using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private CouponRepository repository;
        private IConfiguration configuration;
        private readonly IMapper _mapper;

        public CouponController(CouponRepository repository, IConfiguration configuration, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CouponDTO>> CheckCouponExpirate(string Coupon_serie)
        {
            if (Coupon_serie == null)
            {

                return BadRequest();
            }
            var Coupon = await repository.GetOneAsync(x => x.CouponSerie == Coupon_serie && x.ExpirateDate <= DateTime.Now && x.Status == "Active");
            var CouponDTO = _mapper.Map<CouponDTO>(Coupon);

            return Ok(CouponDTO);
        }
    }
}
