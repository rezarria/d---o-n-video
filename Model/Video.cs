using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dudoan.Model;

namespace Dudoan.Model;

public class Video
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Path { get; set; }
    public ulong Count { get; set; } = 0;
    public Collection<Tag> Tags { get; } = new();
    public Collection<Watch> Watches { get; } = new();
    public Collection<Like> Likes { get; } = new();
}