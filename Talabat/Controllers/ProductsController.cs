using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.ProductSpecififcation;
using Talabat.DTOs;
using Talabat.Errors;
using Talabat.Helpar;

namespace Talabat.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [CacheAttribute(300)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme /*, Roles ="Admin" , Policy =""*/)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {

            var sepc = new prodWithBranAndCateSpecififcation(specParams);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(sepc);

            //JsonResult result = new JsonResult(product);
            //return result;

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var countSpec = new ProductWithFilterForCountSpec(specParams);
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
            return Ok( new Pagination<ProductToReturnDto>(specParams.PageIndex , specParams.PageSize , count, data));

        }




        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {

            var sepc = new prodWithBranAndCateSpecififcation(id);

            var products = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(sepc);
            if(products == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<Product , ProductToReturnDto>(products));

        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrand()
        {
            var brands =await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }



        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategory()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return Ok(categories);
        }


    }
}
