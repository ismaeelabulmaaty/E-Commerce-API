using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Errors;
using Talabat.Repository.Data;

namespace Talabat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product= _dbcontext.Products.Find(100);

            if(product == null)
            {
                return NotFound( new ApiResponse(404));
            }
            return Ok(product);
        }



        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = _dbcontext.Products.Find(100);
            var productDto=product.ToString();
            return Ok(productDto);
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest( new ApiResponse(400));

        }

        [HttpGet("unauthriozed")]
        public ActionResult GetUnAuthriozed()
        {
            return Unauthorized(new ApiResponse(401));

        }


        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }


    }
}
