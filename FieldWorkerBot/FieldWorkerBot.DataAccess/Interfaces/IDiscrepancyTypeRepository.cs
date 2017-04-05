using System.Collections.Generic;
using System.Threading.Tasks;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.DataAccess.Interfaces
{
    public interface IDiscrepancyTypeRepository
    {
        Task<IEnumerable<DiscrepancyType>> Get(string assetId, string inspectionId);
    }
}