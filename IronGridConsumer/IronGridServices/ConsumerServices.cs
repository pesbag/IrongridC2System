using IronGridConsumer.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using IronGridConsumer.Models;

namespace IronGridConsumer.IronGridServices;
public class ConsumerServices
{
    private readonly IronGridDbContext _context;
    public ConsumerServices(IronGridDbContext context)
    {
        _context = context;
    }
    public async Task<bool> ProcessUAVModelAsync(string JsonMessage)
    {
        try
        {
            var report = JsonSerializer.Deserialize<Report>(JsonMessage);
            if (report is null) { return false; }

                var RowToSave = await _context.AssetLiveStatus.FindAsync(report.AssetId);
                var NewReport = new AssetLiveStatus
                {
                    AssetId = report.AssetId,
                    AssetType = report.AssetType,
                    RawValue = report.RawValue,
                    ProcessedStatus = "Warning",
                    IsVerified = false,
                    LastUpdate = DateTime.UtcNow
                };

                bool success = int.TryParse(report.RawValue, out int result);
            if (success == true)
            {
                if (result < 0 || result > 100)
                {
                    if (RowToSave is not null)
                    {
                        RowToSave.ProcessedStatus = "Warning";
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        await _context.AssetLiveStatus.AddAsync(NewReport);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }

                else if (result <= 100 && result >= 20)
                {
                    if (RowToSave is not null)
                    {
                        RowToSave.ProcessedStatus = "Stable";
                        RowToSave.IsVerified = true;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        NewReport.ProcessedStatus = "Stable";
                        NewReport.IsVerified = true;
                        await _context.AssetLiveStatus.AddAsync(NewReport);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                else
                {
                    if (RowToSave is not null)
                    {
                        RowToSave.ProcessedStatus = "Warning";
                        RowToSave.IsVerified = true;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        NewReport.IsVerified = true;
                        await _context.AssetLiveStatus.AddAsync(NewReport);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }            
        }
        catch (DbUpdateException){  return true; }
        catch (Exception ex) { Console.WriteLine($"error processing: {ex.Message}");return false; }
        return true;
    }
    public async Task<bool> ProcessPerimeterSensorModelAsync(string JsonMessage)
    {
        try
        {
            var report = JsonSerializer.Deserialize<Report>(JsonMessage);
            if (report is null) { return false; }
            var NewReport = new AssetLiveStatus
            {
                AssetId = report.AssetId,
                AssetType = report.AssetType,
                RawValue = report.RawValue,
                ProcessedStatus = "Warning",
                IsVerified = false,
                LastUpdate = DateTime.UtcNow
            };

            var RowToSave = await _context.AssetLiveStatus.FindAsync(report.AssetId);

            string[] OptionalGood = ["Good", "GOOD", "good", "gud"];
            string[] OptionlBad = ["Bad", "BAD", "bad", "bed"];

            if (RowToSave is not null)
            {
                if (OptionalGood.Contains(RowToSave.RawValue))
                {
                    RowToSave.ProcessedStatus = "Stable";
                    RowToSave.RawValue = "Good";
                    RowToSave.IsVerified = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else if (OptionlBad.Contains(RowToSave.RawValue))
                {
                    RowToSave.ProcessedStatus = "Warning";
                    RowToSave.RawValue = "Bad";
                    RowToSave.IsVerified = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    RowToSave.ProcessedStatus = "Warning";
                    RowToSave.RawValue = "illegal";
                    RowToSave.IsVerified = false;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            else
            {
                if (OptionalGood.Contains(report.RawValue))
                {
                    NewReport.RawValue = "Good";
                    NewReport.ProcessedStatus = "Stable";
                    NewReport.IsVerified = true;
                            
                    await _context.AssetLiveStatus.AddAsync(NewReport);
                    await _context.SaveChangesAsync();
                    return true;
                }

                if (OptionlBad.Contains(report.RawValue))
                {

                    NewReport.RawValue = "Bad";
                    NewReport.ProcessedStatus = "Warning";
                    NewReport.IsVerified = true;
                    await _context.AssetLiveStatus.AddAsync(NewReport);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }        
        }
        catch (DbUpdateException) { return true; }
        catch (Exception ex) { Console.WriteLine($"error processing: {ex.Message}"); return false; }
        return true;
    }
}
