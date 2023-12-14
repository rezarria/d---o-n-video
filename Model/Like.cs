using System.ComponentModel.DataAnnotations.Schema;
using Dudoan.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dudoan.Model;

[PrimaryKey(nameof(UserId), nameof(VideoId))]
public class Like
{
    public Guid UserId { get; set; }
    public Guid VideoId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    [ForeignKey(nameof(VideoId))]
    public Video Video { get; set; } = null!;
    public DateTime Time { get; set; }
    public LikeStatus LikeStatus { get; set; }

}

public enum LikeStatus { None, Like, Dislike }
