using System.Collections.Generic;
using System.Threading.Tasks;
using FieldWorkerBot.DataAccess.Interfaces;
using FieldWorkerBot.Domain;
using FieldWorkerBot.Services.Interfaces;

namespace FieldWorkerBot.Services
{
    public class DiscrepancyTypeService : IDiscrepancyTypeService
    {
        private IDiscrepancyTypeRepository _discrepancyTypeRepository;

        public DiscrepancyTypeService(IDiscrepancyTypeRepository discrepancyTypeRepository)
        {
            _discrepancyTypeRepository = discrepancyTypeRepository;
        }

        public async Task<IEnumerable<DiscrepancyType>> Get(string assetId, string inspectionId)
        {
            return await _discrepancyTypeRepository.Get(assetId, inspectionId);
        }
    }
}
