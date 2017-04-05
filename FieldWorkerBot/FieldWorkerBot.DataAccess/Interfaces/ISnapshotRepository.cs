using System.Threading.Tasks;

namespace FieldWorkerBot.DataAccess.Interfaces
{
    public interface ISnapshotRepository
    {
        Task<string> Create(string base64Content);
    }
}