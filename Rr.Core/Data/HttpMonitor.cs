using System.ComponentModel.DataAnnotations;

namespace Rr.Core.Data;

public class HttpMonitor
{
    public int Id { get; set; }
    
    [MinLength(4), MaxLength(50)]
    public string Name { get; set; } = "";
    
    [MinLength(5), MaxLength(1000), Url]
    public string Url { get; set; } = "";
}
