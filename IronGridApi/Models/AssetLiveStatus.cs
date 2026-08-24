using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace IronGridApi.Models;

public class AssetLiveStatus
{
    public int AssetId { get; set; }
    [RegularExpression("^(UAV|PerimeterSensor)$")]
    public string AssetType { get; set; } = string.Empty;
    [Required]
    public string RawValue { get; set; } = string.Empty;
    [RegularExpression("^(Stable|Warning)$")]
    public string ProcessedStatus { get; set; } = string.Empty;
    [Required]
    public bool IsVerified { get; set; }
    [Required]
    public DateTime LastUpdate { get; set; }
    public Asset Asset { get; set; } = null!;
}
