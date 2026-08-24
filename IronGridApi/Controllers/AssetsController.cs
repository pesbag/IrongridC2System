using IronGridApi.Dtos;
using IronGridApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Formats.Tar;
namespace IronGridApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetRepository _repo;
        public AssetsController(IAssetRepository repo)
        {
            _repo=repo;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAssetByIdAsync(int id)
        {
            var asset = await _repo.GetByAssetIdAsync(id);

            if (asset == null) { return NotFound(); }
            return Ok(asset);
        }
        [HttpGet("unit{id}")] //add extra controller for create new unit
        public async Task<IActionResult> GetUnitById(int id)
        {
            var unit = await _repo.GetByUnitId(id);

            if (unit == null) { return NotFound(); }
            return Ok(unit);
        }

        [HttpPost]
        public async Task<ActionResult<CreateUnitDto>> CreateNewUnitAsync(CreateUnitDto newUnit)
        {
            var result = await _repo.CreateUnitAsync(newUnit);
            return CreatedAtAction(nameof(GetUnitById), new { id = result.Id }, result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAssetAsync(int id)
        {
        var asset = await _repo.GetByAssetIdAsync(id);
        if (asset == null)
        {
            return NotFound();
        }

        await _repo.DeleteAsync(asset);

        return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(int id, UpdateAssetDto updateAsset)
        {
           
            var result = await _repo.GetByAssetIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            result.UnitId = updateAsset.UnitId;
            result.AssetSerial = updateAsset.AssetSerial;
            result.AssetType = updateAsset.AssetType;

            await _repo.UpdateAsync(result);

            return NoContent();
        }
    }

}
