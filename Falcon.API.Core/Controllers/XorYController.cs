namespace Falcon.API.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Falcon.XorYDatabase;
    using Falcon.XorYDatabase.Models.Json;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class XorYController(XorYDBContext context) : ControllerBase
    {
        [HttpPost("Question")]
        public async Task<IActionResult> GetQuestion(QuestionRequest request)
        {
            if (request.Categories == null || request.Categories.Length < 2)
            {
                return BadRequest("Categories must contain at least two elements.");
            }

            request.SeenOptions ??= [];

            return Ok(await context.Options
                .Where(o => !request.SeenOptions.Any(id => o.ID == id) && request.Categories.Any(c => o.Category == c))
                .OrderBy(a => EF.Functions.Random())
                .Select(o => new QuestionResponse { ID = o.ID, Name = o.Name })
                .FirstOrDefaultAsync());
        }

        [HttpPost("CheckAnswer")]
        public async Task<AnswerCheckResponse> CheckAnswer(AnswerCheckRequest request) => await context.Options
            .Where(o => o.ID == request.OptionID)
            .Select(o => new AnswerCheckResponse { Correct = o.Category == request.Answer, Url = o.Url })
            .FirstOrDefaultAsync();
    }
}