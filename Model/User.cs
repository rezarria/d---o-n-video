using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dudoan.Model;

namespace Dudoan.Model;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Collection<Watch> Watches { get; } = new();
    public Collection<Like> Likes { get; } = new();
}