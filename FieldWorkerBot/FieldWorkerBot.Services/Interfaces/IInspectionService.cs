using System.Threading.Tasks;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.Services.Interfaces
{
    public interface IInspectionService
    {
        Task<Inspection> GetNotPerformedByAssetId(string assetId);
        Task CommitOk(Inspection inspection);
    }
}