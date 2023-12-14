using Dudoan.Dbcontext;
using Dudoan.MML;
using Dudoan.Model;
using Microsoft.AspNetCore.Mvc;

namespace Dudoan.Controller;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    public VideoController(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    private readonly AppDbContext appDbContext;

    [HttpPost]
    public IActionResult create([FromForm] CreateDTO dto)
    {

        var path = Path.Combine("video", dto.Video.FileName);
        using (var stream = new FileStream(path, FileMode.CreateNew))
        {
            dto.Video.CopyTo(stream);
        }

        var nonExistingTags = dto.Tags.Where(tag => !appDbContext.Tags.Any(t => t.Name == tag)).ToList();
        nonExistingTags.ForEach(x =>
        {
            appDbContext.Tags.Add(new()
            {
                Name = x
            });
        });

        if (nonExistingTags.Count != 0)
        {
            appDbContext.SaveChanges();
        }

        appDbContext.Videos.Add(new()
        {
            Title = dto.Title,
            Path = path
        });

        appDbContext.SaveChanges();

        return Ok("ok");
    }
}

public class CreateDTO
{
    public string Title { get; set; }
    public List<string> Tags { get; set; } = new();
    public IFormFile Video { get; set; }
}