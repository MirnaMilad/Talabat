using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.repository.Data;

namespace Talabat.APIS.Controllers
{
    public class BuggyController : ApiBaseController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext) 
        {
            _dbContext = dbContext;
        }

        [HttpGet("NotFound")]
        //BaseUrl/api/Buggy/NotFound
        public ActionResult GetNotFoundRequest()
        {
            var Product = _dbContext.Products.Find(100);
            if(Product is null) return NotFound(new ApiResponse(404));
            return Ok(Product);
        }

        [HttpGet("ServerError")]
        public ActionResult GetServerError() 
        {
            var Product = _dbContext.Products.Find(100);
            var ProductToReturn = Product.ToString();
            // Will throw exception (Null reference exception)
            return Ok(ProductToReturn);
        }

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}
