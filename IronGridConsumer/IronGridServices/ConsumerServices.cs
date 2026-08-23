using IronGridConsumer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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

            if (report.AssetType == "UAV")
            {
                var RowToSave = await _context.AssetLiveStatuses.FindAsync(report.AssetId);

                if (report.RawValue.CompareTo("100") > 0 || report.RawValue.CompareTo("0") < 0)
                {
                    if (RowToSave is not null)
                    {
                        RowToSave.ProcessedStatus = "Warning";
                        RowToSave.IsVerified = false;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        var NewReport = new AssetLiveStatus
                        {
                            AssetId = report.AssetId,
                            AssetType = report.AssetType,
                            RawValue = report.RawValue,
                            ProcessedStatus = "Warning",
                            IsVerified = false,
                            LastUpdate = report.Timestamp
                        };
                        await _context.AssetLiveStatuses.AddAsync(NewReport);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }

                if (report.RawValue.CompareTo("100") < 0 || report.RawValue.CompareTo("20") < 0)
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
                        var NewReport = new AssetLiveStatus
                        {
                            AssetId = report.AssetId,
                            AssetType = report.AssetType,
                            RawValue = report.RawValue,
                            ProcessedStatus = "Stable",
                            IsVerified = true,
                            LastUpdate = report.Timestamp
                        };
                        await _context.AssetLiveStatuses.AddAsync(NewReport);
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
                        var NewReport = new AssetLiveStatus
                        {
                            AssetId = report.AssetId,
                            AssetType = report.AssetType,
                            RawValue = report.RawValue,
                            ProcessedStatus = "Warning",
                            IsVerified = true,
                            LastUpdate = report.Timestamp
                        };
                        await _context.AssetLiveStatuses.AddAsync(NewReport);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
        }

        catch (DbUpdateException)
        {
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing: {ex.Message}");
            return false;
        }
        return true;
    }
        public async Task<bool> ProcessPerimeterSensorModelAsync(string JsonMessage)
        {
           try
               {
            var report = JsonSerializer.Deserialize<Report>(JsonMessage);
            if (report is null) { return false; }
            else if (report.AssetType == "PerimeterSensor")
            {
                var RowToSave = await _context.AssetLiveStatuses.FindAsync(report.AssetId);
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
                    if (OptionlBad.Contains(RowToSave.RawValue))
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
                    if (OptionalGood.Contains(RowToSave.RawValue))
                    {
                        var NewReport = new AssetLiveStatus
                        {
                            AssetId = report.AssetId,
                            AssetType = report.AssetType,
                            RawValue = "Good",
                            ProcessedStatus = "Stable",
                            IsVerified = true,
                            LastUpdate = report.Timestamp
                        };
                        await _context.AssetLiveStatuses.AddAsync(NewReport);
                        await _context.SaveChangesAsync();
                        return true;
                    }

                    if (OptionlBad.Contains(RowToSave.RawValue))
                    {
                        var NewReport = new AssetLiveStatus
                        {
                            AssetId = report.AssetId,
                            AssetType = report.AssetType,
                            RawValue = "Bad",
                            ProcessedStatus = "Warning",
                            IsVerified = true,
                            LastUpdate = report.Timestamp
                        };
                        await _context.AssetLiveStatuses.AddAsync(NewReport);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                } 
            }
        }
        catch (DbUpdateException)
        {
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing: {ex.Message}");
            return false;
        }
        return true;
    }
   
}
