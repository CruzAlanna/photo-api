using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoApi.Data;   // Change "PhotoApi" to your actual project name
using PhotoApi.Models; // Change "PhotoApi" to your actual project name

namespace PhotoApi.Controllers  // Change "PhotoApi" to your actual project name
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ADD THE INDEX ACTION HERE:
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photo>>> Index()
        {
            var photos = await _context.Photos.ToListAsync();
            return Ok(photos);
        }
    }
}