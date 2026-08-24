using System.ComponentModel.DataAnnotations;

namespace IronGridApi.Dtos;

public class UpdateAssetDto
{
    public int Id { get; set; }
    [Required]
    public int UnitId { get; set; }
    [Required]
    public string AssetSerial { get; set; } = string.Empty;
    [RegularExpression("^(UAV|PerimeterSensor)$")]
    public string AssetType { get; set; } = "GenericAsset";
}
