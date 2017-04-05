using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FieldWorkerBot.DataAccess.Interfaces;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.DataAccess
{
    public class InspectionRepository : IInspectionRepository
    {
        public Task<IEnumerable<Inspection>> GetByAssetId(string assetId)
        {
            return Task.FromResult(Enumerable.Repeat(GetInspectionMock(assetId), 1));
        }

        public Task<bool> Commit(Inspection inspection)
        {
            return Task.FromResult(true);
        }

        private Inspection GetInspectionMock(string assetId)
        {
            return new Inspection
            {
                Id = assetId,
                AssetId = assetId,
                InspectionDate = null,
                StatusCode = -1
            };
        }
    }
}
