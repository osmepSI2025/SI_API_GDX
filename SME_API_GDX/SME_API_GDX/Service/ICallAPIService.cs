using SME_API_GDX.Models;

namespace SME_API_GDX.Services
{
    public interface ICallAPIService
    {
        Task<string> GetDataApiAsync(MapiInformationModels apiModels, object xdata);

    }
}
