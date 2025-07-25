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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Photo>>> Index()
    {
      var photos = await _context.Photos.ToListAsync();
      return Ok(photos);
    }

    // ADD THE CREATE ACTION HERE (below Index):
    [HttpPost]
    public async Task<ActionResult<Photo>> Create([FromBody] CreatePhotoRequest request)
    {
      var photo = new Photo
      {
          Name = request.Name,
          Url = request.Url,
          Width = request.Width,
          Height = request.Height
      };

      _context.Photos.Add(photo);
      await _context.SaveChangesAsync();

      return Ok(photo); // Will change to CreatedAtAction once we add the Show action
    }

    // ADD THE REQUEST CLASS INSIDE THE CONTROLLER CLASS (at the bottom):
    public class CreatePhotoRequest
    {
      public string Name { get; set; } = "";
      public string Url { get; set; } = "";
      public int Width { get; set; }
      public int Height { get; set; }
      // Note: CreatedAt/UpdatedAt are set automatically, so don't include them in request bodies
    }
  }
}