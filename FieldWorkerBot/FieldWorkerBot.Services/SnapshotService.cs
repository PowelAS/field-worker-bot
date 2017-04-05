using System.Threading.Tasks;
using FieldWorkerBot.DataAccess.Interfaces;
using FieldWorkerBot.Services.Interfaces;

namespace FieldWorkerBot.Services
{
    public class SnapshotService : ISnapshotService
    {
        private ISnapshotRepository _snapshotRepository;

        public SnapshotService(ISnapshotRepository snapshotRepository)
        {
            _snapshotRepository = snapshotRepository;
        }

        public async Task<string> Create(string base64Content)
        {
            return await _snapshotRepository.Create(base64Content);
        }
    }
}
