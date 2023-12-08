using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace dudoan.model;

[Index(nameof(Name))]
public class Tag
{
    [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
}