using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FieldWorkerBot.DataAccess.Interfaces;
using FieldWorkerBot.Domain;

namespace FieldWorkerBot.DataAccess
{
    public class DiscrepancyTypeRepository : IDiscrepancyTypeRepository
    {
        private readonly DiscrepancyType[] _mockDiscrepancyTypes =
        {
            new DiscrepancyType
            {
                Id = "1",
                Name = "Breaker",
                Type = "StatusCode"
            },
            new DiscrepancyType
            {
                Id = "2",
                Name = "Water damage",
                Type = "StatusCode"
            },
            new DiscrepancyType()
            {
                Id = "3",
                Name = "Ground",
                Type = "StatusCode"
            }
        };

        public Task<IEnumerable<DiscrepancyType>> Get(string assetId, string inspectionId)
        {
            return Task.FromResult(_mockDiscrepancyTypes.AsEnumerable());
        }
    }
}
