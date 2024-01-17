using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers {

    [ApiController]
    [Route("/api/test")]
    public class TestController: ControllerBase
    {
        [HttpGet]
        public ActionResult Get(){
            return Ok("OK");
        }
    }
    
}