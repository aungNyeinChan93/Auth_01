using Auth.web_api_01.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.web_api_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [Permission("Post.Edit")]
        [HttpGet("test-one")]
        public async Task<IActionResult> TestOne()
        {
            return Ok("test one");
        }

        [Permission("Post.View")]
        [HttpGet("test-two")]
        public async Task<IActionResult> TestTwo()
        {
            return Ok("test two");
        }


        [Authorize(Policy = "PostDelete")]
        [HttpGet("test-three")]
        public async Task<IActionResult> TestThree()
        {
            return Ok("test three");
        }
    }
}
