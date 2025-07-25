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

    // INDEX ACTION:
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Photo>>> Index()
    {
      var photos = await _context.Photos.ToListAsync();
      return Ok(photos);
    }

    // CREATE ACTION:
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

      return CreatedAtAction(nameof(Show), new { id = photo.Id }, photo);
    }

    // SHOW ACTION:
    [HttpGet("{id}")]
    public async Task<ActionResult<Photo>> Show(int id)
    {
      var photo = await _context.Photos.FindAsync(id);
      
      if (photo == null)
      {
          return NotFound();
      }

      return Ok(photo);
    }

    // UPDATE ACTION:
    [HttpPut("{id}")]
    public async Task<ActionResult<Photo>> Update(int id, [FromBody] UpdatePhotoRequest request)
    {
      var photo = await _context.Photos.FindAsync(id);
      
      if (photo == null)
      {
          return NotFound();
      }

      // We use PUT here to replace the full resource; switch to PATCH if you teach JSON-Patch later
      photo.Name = request.Name ?? photo.Name;
      photo.Url = request.Url ?? photo.Url;
      photo.Width = request.Width ?? photo.Width;
      photo.Height = request.Height ?? photo.Height;

      await _context.SaveChangesAsync();
      
      return Ok(photo);
    }

    // DESTROY ACTION:
    [HttpDelete("{id}")]
      public async Task<ActionResult> Destroy(int id)
      {
        var photo = await _context.Photos.FindAsync(id);
        
        if (photo == null)
        {
            return NotFound();
        }

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync();
        
        return NoContent(); // HTTP 204 - successful deletion with no content
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

    // ADD THIS REQUEST CLASS at the bottom with the other request classes:
    public class UpdatePhotoRequest
    {
      public string Name { get; set; } = "";
      public string Url { get; set; } = "";
      public int? Width { get; set; }
      public int? Height { get; set; }
    }
  }
}