using Fuwa.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fuwa.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly FuwaDbContext _context;

        public PostController(FuwaDbContext context)
        {
            _context = context;
        }

    }
}
