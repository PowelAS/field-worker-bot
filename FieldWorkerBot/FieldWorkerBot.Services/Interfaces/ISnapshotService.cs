using System.Threading.Tasks;

namespace FieldWorkerBot.Services.Interfaces
{
    public interface ISnapshotService
    {
        Task<string> Create(string base64Content);
    }
}