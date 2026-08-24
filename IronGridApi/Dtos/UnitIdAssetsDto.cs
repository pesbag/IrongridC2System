using IronGridApi.Models;
using System.ComponentModel.DataAnnotations;

namespace IronGridApi.Dtos;

public class UnitIdAssetsDto
{
    public int AssetId { get; set; }
    [RegularExpression("^(UAV|PerimeterSensor)$")]
    public string AssetType { get; set; } = string.Empty;
    [RegularExpression("^(Stable|Warning)$")]
    public string ProcessedStatus { get; set; } = string.Empty;
    [Required]
    public string AssetSerial { get; set; } = string.Empty;
    [Required]
    public bool IsVerified { get; set; }
    [Required]
    public DateTime LastUpdate { get; set; }
    public Asset Asset { get; set; } = null!;
}
