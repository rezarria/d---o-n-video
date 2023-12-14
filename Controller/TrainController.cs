using Dudoan.Dbcontext;
using Dudoan.MML;
using Microsoft.AspNetCore.Mvc;

namespace Dudoan.Controller;

[ApiController]

[Route("api/[controller]")]
public class TrainController : ControllerBase
{

    public TrainController(MLService mLService, AppDbContext appDbContext)
    {
        this.mLService = mLService;
        this.appDbContext = appDbContext;
    }

    private readonly MLService mLService;
    private readonly AppDbContext appDbContext;

    [HttpGet("run")]
    public IActionResult Run()
    {
        mLService.loadFromDB();
        return Ok();
    }

    [HttpGet("ok")]
    public IActionResult check()
    {
        return Ok("ok");
    }

    [HttpPost("getRecommendations")]
    public IActionResult GetRecommendations([FromBody] List<Guid> videoIds)
    {
        if (videoIds == null || videoIds.Count == 0)
        {
            return BadRequest("Danh sách VideoId trống.");
        }

        try
        {
            var result = new Dictionary<Guid, List<Guid>>();
            appDbContext.Videos.Select(x => new MLModel()
            {
                Id = x.Id,
                Title = x.Title,
                Likes = x.Likes.Select(y => new MLModel.Like()
                {
                    UserId = y.UserId,
                    VideoId = y.VideoId,
                    Time = y.Time,
                    LikeStatus = y.LikeStatus
                }).ToList(),
                Watches = x.Watches.Select(y => new MLModel.Watch()
                {
                    UserId = y.User.Id,
                    VideoId = y.Video.Id,
                    Duration = y.Details.Select(z => new MLModel.DurationType()
                    {
                        Duration = z.Duration,
                        When = z.When
                    }).ToList()
                }).ToList()
            }).ToList().ForEach(d =>
                {
                    result.Add(d.Id, mLService.Recommendation(d));
                }
            );

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
        }
    }
}