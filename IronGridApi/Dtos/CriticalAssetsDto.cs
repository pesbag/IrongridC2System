using System.ComponentModel.DataAnnotations;

namespace IronGridApi.Dtos;

public class CriticalAssetsDto
{
    public int AssetId { get; set; }
    [Required]
    public string AssetSerial { get; set; } = string.Empty;
    [RegularExpression("^(UAV|PerimeterSensor)$")]
    public string AssetType { get; set; } = "GenericAsset";
    public string UnitName { get; set; } = "Unknown Unit";
    public string Sector { get; set; } = "General";
    [RegularExpression("^(Stable|Warning)$")]
    public string ProcessedStatus { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    [Required]
    public DateTime LastUpdate { get; set; }
}
