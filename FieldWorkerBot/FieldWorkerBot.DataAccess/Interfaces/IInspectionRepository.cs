using System.Collections.Generic;
using System.Threading.Tasks;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.DataAccess.Interfaces
{
    public interface IInspectionRepository
    {
        Task<IEnumerable<Inspection>> GetByAssetId(string assetId);
        Task<bool> Commit(Inspection inspection);
    }
}