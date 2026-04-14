using Longfunctie.api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Longfunctie.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/parent")]
    public class ParentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ParentController(AppDbContext context)
        {
            _context = context;
        }

        private string GetParentName()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedAccessException();

            return claim.Value;
        }

        [HttpGet("children")]
        public async Task<IActionResult> GetChildren()
        {
            var parentName = GetParentName();

            var children = await _context.Children
                .Where(c => c.ParentName == parentName)
                .ToListAsync();

            return Ok(children);
        }
    }
}