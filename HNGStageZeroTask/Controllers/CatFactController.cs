using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HNGStageZeroTask.Controllers
{
    [Route("me")]
    [ApiController]
    public class CatFactController : ControllerBase
    {
        private readonly ICatFactService _catFactService;

        public CatFactController(ICatFactService catFactService) 
        {
            _catFactService = catFactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileAsync()
        {
            var catFact = await _catFactService.GetRandomCatFactAsync();

            var result = new
            {
                status = "success",
                user = new
                {
                    email = "abdulwaheedmahmoodahmad6@gmail.com",
                    name = "AbdulWaheed MahmoodAhmad",
                    stack = "C#/.NET Core"
                },
                timestamp = DateTime.UtcNow.ToString("o"),
                fact = catFact
            };

            return Ok(result);
        }
    }
}
