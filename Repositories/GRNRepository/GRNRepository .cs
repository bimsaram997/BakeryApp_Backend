using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Data.Stock;
using System;
using System.Threading.Tasks;

namespace Repositories.GRNRepository
{
    public interface IGRNRepository
    {
        Task<string> GenerateGRNAsync();
        Task<bool> DeleteGRNAsync(string grnNumber);

    }

    public class GRNRepository : IGRNRepository
    {
        private readonly AppDbContext _context;

        public GRNRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateGRNAsync()
        {
            // Ensure the database connection is open
            await _context.Database.OpenConnectionAsync();

            try
            {
                // Create and configure the SQL command to get the next value from the sequence
                using var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT NEXT VALUE FOR GRNSequence;";
                command.CommandType = System.Data.CommandType.Text;

                // Execute the command and get the next ID value
                var nextId = (int)await command.ExecuteScalarAsync();

                // Format the GRN number with leading zeros
                var grnNumber = $"GRN{nextId:D3}";

                // Create and save the GRN entity
                var grn = new GRN
                {
                    GRNNumber = grnNumber,
                    CreatedAt = DateTime.UtcNow
                };

                _context.GRNs.Add(grn);
                await _context.SaveChangesAsync();

                return grnNumber;
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, rethrowing, etc.)
                // For simplicity, we rethrow the exception here
                throw new InvalidOperationException("An error occurred while generating the GRN number.", ex);
            }
            finally
            {
                // Ensure the database connection is closed
                if (_context.Database.GetDbConnection().State == System.Data.ConnectionState.Open)
                {
                    await _context.Database.GetDbConnection().CloseAsync();
                }
            }

            public async Task<bool> DeleteGRNAsync(string grnNumber)
            {
                // Find the GRN entity by its GRN number
                var grn = await _context.GRNs.FirstOrDefaultAsync(g => g.GRNNumber == grnNumber);

                if (grn == null)
                {
                    // If the GRN is not found, return false
                    return false;
                }

                // Remove the GRN entity
                _context.GRNs.Remove(grn);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return true to indicate that the GRN was successfully deleted
                return true;
            }
        }

    }
}
