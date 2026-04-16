using Microsoft.EntityFrameworkCore;
using TransactionApprovalSimulator.Server.Data;
using TransactionApprovalSimulator.Server.Models;

namespace TransactionApprovalSimulator.Server.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _db;

        public TransactionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Transaction>> GetApprovedAsync(CancellationToken cancellation = default)
        {
            return await _db.Transactions
                .Where(t => t.Approved)
                .AsNoTracking()
                .ToListAsync(cancellation);
        }

        public async Task<Transaction> CreateAsync(TransactionRequest request, CancellationToken cancellation = default)
        {

            // find the time zone for the selected region
            var timeZoneId = GetTimeZoneIdForRegion(request.Country);
            TimeZoneInfo targetTz;
            try
            {
                targetTz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch
            {
                // default UTC
                targetTz = TimeZoneInfo.Utc;
            }

            // server local time zone
            var serverTz = TimeZoneInfo.Local;

            // server local time now
            var serverNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, serverTz);

            var scheduledServerLocal = new DateTime(
            serverNow.Year,
            serverNow.Month,
            serverNow.Day,
            request.ScheduledHour,
            request.ScheduledMinute,
            0,
            DateTimeKind.Unspecified);

            // convert to region's time
            var scheduledRegionLocal = TimeZoneInfo.ConvertTime(scheduledServerLocal, serverTz, targetTz);

            // Approve only if region's time is in bank open hours range (08:00 - 18:00)
            var businessStart = new TimeSpan(8, 0, 0);
            var businessEnd = new TimeSpan(18, 0, 0);
            var approved = scheduledRegionLocal.TimeOfDay >= businessStart && scheduledRegionLocal.TimeOfDay <= businessEnd;

            // convert back to UTC for storage
            var scheduledUtc = TimeZoneInfo.ConvertTimeToUtc(scheduledRegionLocal, targetTz);

            var tx = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Description = request.Description,
                Country = request.Country,
                ScheduledAt = scheduledUtc,
                Approved = approved,
                CreatedAt = DateTime.UtcNow
            };

            _db.Transactions.Add(tx);

            try
            {
                await _db.SaveChangesAsync(cancellation);
            }
            catch (DbUpdateException dbEx)
            {
                throw new InvalidOperationException("Failed to write transaction to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while saving transaction.", ex);
            }

            return tx;
        }

        private static string GetTimeZoneIdForRegion(string? region)
        {
            if (string.IsNullOrWhiteSpace(region)) return TimeZoneInfo.Utc.Id;

            return region.Trim().ToLowerInvariant() switch
            {
                "israel" => "Israel Standard Time",
                "united states" => "Eastern Standard Time",
                "united kingdom" => "GMT Standard Time",
                "germany" => "W. Europe Standard Time",
                "france" => "Romance Standard Time",
                "india" => "India Standard Time",
                _ => TimeZoneInfo.Utc.Id
            };
        }
    }
}