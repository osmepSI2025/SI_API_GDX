using SME_API_GDX.Entities;
using SME_API_GDX.Models;

namespace SME_API_GDX.Repository
{
    public interface IApiInformationRepository
    {
        Task<IEnumerable<MApiInformation>> GetAllAsync(MapiInformationModels param);
        Task<MApiInformation> GetByIdAsync(int id);
        Task AddAsync(MApiInformation service);
        Task UpdateAsync(MApiInformation service);
        Task DeleteAsync(int id);
        Task UpdateAllBearerTokensAsync(string newBearerToken);
        Task UpdateGDXTokensAsync(string newBearerToken);
    }
}
