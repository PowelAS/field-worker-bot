using System;
using System.Linq;
using System.Threading.Tasks;
using FieldWorkerBot.DataAccess.Interfaces;
using FieldWorkerBot.Domain;
using FieldWorkerBot.Services.Interfaces;

namespace FieldWorkerBot.Services
{
    public class InspectionService : IInspectionService
    {
        private IInspectionRepository _inspectionRepository;

        public InspectionService(IInspectionRepository inspectionRepository)
        {
            _inspectionRepository = inspectionRepository;
        }

        public async Task<Inspection> GetNotPerformedByAssetId(string assetId)
        {
            return (await _inspectionRepository.GetByAssetId(assetId)).FirstOrDefault();
        }

        public async Task CommitOk(Inspection inspection)
        {
            inspection.InspectionDate = DateTime.Now;
            inspection.StatusCode = 0;
            await _inspectionRepository.Commit(inspection);
        }
    }
}
