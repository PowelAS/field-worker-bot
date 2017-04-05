using System.Collections.Generic;
using System.Threading.Tasks;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.Services.Interfaces
{
    public interface IDiscrepancyTypeService
    {
        Task<IEnumerable<DiscrepancyType>> Get(string assetId, string inspectionId);
    }
}