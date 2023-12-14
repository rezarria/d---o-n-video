using System.Collections.ObjectModel;
using Dudoan.ML;

namespace Dudoan.MML;

public class MLModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public String Tags { get; set; }
    public List<Like> Likes { get; set; } = new();
    public List<Watch> Watches { get; set; } = new();
    public class Like
    {
        public Guid UserId { get; set; }
        public Guid VideoId { get; set; }
        public Dudoan.Model.LikeStatus LikeStatus { get; set; }
        public DateTime Time { get; set; }
    }

    public class Watch
    {
        public Guid UserId { get; set; }
        public Guid VideoId { get; set; }
        public List<DurationType> Duration { get; set; } = new();
    }

    public class DurationType
    {
        public ulong Duration { get; set; }
        public DateTime When { get; set; }
    }
}


