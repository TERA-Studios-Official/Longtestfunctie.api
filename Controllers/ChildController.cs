using Longfunctie.api.Data;
using Longfunctie.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Longfunctie.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/child")]
    public class ChildController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChildController(AppDbContext context)
        {
            _context = context;
        }

        private string GetParentName()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedAccessException("JWT missing NameIdentifier claim");
            return claim.Value;
        }

        // CREATE child
        [HttpPost]
        public async Task<IActionResult> CreateChild([FromBody] ChildRequest request)
        {
            var parentName = GetParentName();

            var child = new Child
            {
                ParentName = parentName,
                AnonymousId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Name = request.Name,
                Age = request.Age,
                Avatar = request.Avatar,
                DoctorName = request.DoctorName,
                TreatmentType = request.TreatmentType,
                TreatmentDate = request.TreatmentDate
            };

            _context.Children.Add(child);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(child);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to save child: {ex.Message}");
            }
        }

        // GET all children for parent
        [HttpGet]
        public async Task<IActionResult> GetChildren()
        {
            var parentName = GetParentName();
            var children = await _context.Children
                .Where(c => c.ParentName == parentName)
                .ToListAsync();

            return Ok(children);
        }

        // GET single child by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChild(int id)
        {
            var parentName = GetParentName();
            var child = await _context.Children
                .FirstOrDefaultAsync(c => c.Id == id && c.ParentName == parentName);

            if (child == null)
                return NotFound();

            return Ok(child);
        }

        // UPDATE child by Id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChild(int id, [FromBody] ChildRequest request)
        {
            var parentName = GetParentName();
            var child = await _context.Children
                .FirstOrDefaultAsync(c => c.Id == id && c.ParentName == parentName);

            if (child == null)
                return NotFound();

            child.Name = request.Name;
            child.Age = request.Age;
            child.Avatar = request.Avatar;
            child.DoctorName = request.DoctorName;
            child.TreatmentType = request.TreatmentType;
            child.TreatmentDate = request.TreatmentDate;

            await _context.SaveChangesAsync();
            return Ok(child);
        }
    }
}