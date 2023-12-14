using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dudoan.Model;

public class Watch
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Video Video { get; set; } = null!;
    public User User { get; set; } = null!;
    public Collection<WatchDetail> Details { get; } = new();
}

public class WatchDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Watch Watch { get; set; } = null!;
    public DateTime When { get; set; }
    public ulong Duration { get; set; }
}

