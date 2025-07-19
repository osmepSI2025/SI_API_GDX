using Microsoft.EntityFrameworkCore;
using SME_API_GDX.Entities;
using SME_API_GDX.Models;
using System.Text;

namespace SME_API_GDX.Repository
{
    public class ApiInformationRepository : IApiInformationRepository
    {
        private readonly GDXDBContext _context;

        public ApiInformationRepository(GDXDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MApiInformation>> GetAllAsync(MapiInformationModels param)
        {
            try
            {
                var query = _context.MApiInformations.AsQueryable();

                if (!string.IsNullOrEmpty(param.ServiceNameCode) && param.ServiceNameCode != "")
                    query = query.Where(p => p.ServiceNameCode == param.ServiceNameCode);
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<MApiInformation> GetByIdAsync(int id)
            => await _context.MApiInformations.FindAsync(id);

        public async Task AddAsync(MApiInformation service)
        {
            await _context.MApiInformations.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MApiInformation service)
        {
            try
            {
                var todo = await _context.MApiInformations.FindAsync(service.Id);

                if (todo == null)
                {
                    throw new Exception("Service not found.");
                }
  
                if (service.Token != null) 
                {
                    todo.Token = service.Token;
                }
                  

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task DeleteAsync(int id)
        {
            var service = await _context.MApiInformations.FindAsync(id);
            if (service != null)
            {
                _context.MApiInformations.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateAllBearerTokensAsync(string newBearerToken)
        {
            try
            {
                // Retrieve all records from the repository
                var allRecords = await GetAllAsync(new MapiInformationModels());

                if (allRecords == null || !allRecords.Any())
                    throw new Exception("No records found to update.");

                // Update the Bearer field for each record
                foreach (var record in allRecords)
                {
                    record.Bearer = newBearerToken;
                    await UpdateAsync(record);
                }
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error updating Bearer tokens: " + ex.Message, ex);
            }
        }

        public async Task UpdateGDXTokensAsync(string newBearerToken)
        {
            try
            {
                // Retrieve all records from the repository
                var allRecords = await GetAllAsync(new MapiInformationModels());

                if (allRecords == null || !allRecords.Any())
                    throw new Exception("No records found to update.");

                // Update the Bearer field for each record
                foreach (var record in allRecords.ToList())
                {
                    record.Token = EncodeBase64(newBearerToken);
                    await UpdateAsync(record);
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error updating Bearer tokens: " + ex.Message, ex);
            }
        }

        private static string EncodeBase64(string plainText)
        {
            if (plainText == null) return string.Empty;
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

    }
}
