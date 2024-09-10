using System.ComponentModel.DataAnnotations;

namespace Rr.Core.Data;

public class HttpMonitor : IValidatableObject
{
    public int Id { get; set; }
    
    [MinLength(4), MaxLength(50)]
    public string Name { get; set; } = "";

    public bool IsEnabled { get; set; } = true;
    
    [MinLength(5), MaxLength(1000)]
    public string Url { get; set; } = "";
    
    [Range(1, 3_600_000)]
    public int? TimeoutMs { get; set; }

    public DateTime CheckedAt { get; set; }

    public int Status { get; set; }

    public int ResponseTimeMs { get; set; }

    [MaxLength(5000)]
    public string? ContentContains { get; set; } = "";
    
    [MaxLength(5000)]
    public string? ContentNotContains { get; set; } = "";
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Url.Length > 5 && !Uri.TryCreate(Url, UriKind.Absolute, out _))
        {
            yield return new("Invalid URL format.", new[] { nameof(Url) });
        }
    }
}
