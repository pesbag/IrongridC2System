using IronGridApi.Data;
using IronGridApi.Dtos;
using IronGridApi.Models;
namespace IronGridApi.Repository;


public class AssetRepository : IAssetRepository
{
    private readonly IronGridDbContext _context;
    public AssetRepository(IronGridDbContext context)
    {
        _context = context;
    }
    public async Task<Asset?> GetByAssetIdAsync(int id)
    {
        return _context.Assets.FirstOrDefault(a => a.Id == id);
    }
    public async Task<Unit?> GetByUnitId(int id)
    {
        return _context.Units.FirstOrDefault(a => a.Id == id);
    }
    public async Task<CreateUnitDto> CreateUnitAsync(CreateUnitDto newUnit)
    {
        var CreateNewUnit = new Unit
        {
            Id = newUnit.Id,
            UnitName = newUnit.UnitName,
            Sector=newUnit.Sector
        };
        _context.Units.Add(CreateNewUnit);
        await _context.SaveChangesAsync();

        return new CreateUnitDto
        {
            Id = newUnit.Id,
            UnitName = newUnit.UnitName,
            Sector = newUnit.Sector
        };
    }
    public async Task DeleteAsync(Asset asset)
    {
        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Asset asset)
    {
        _context.Assets.Update(asset);
        await _context.SaveChangesAsync();
    }
}

