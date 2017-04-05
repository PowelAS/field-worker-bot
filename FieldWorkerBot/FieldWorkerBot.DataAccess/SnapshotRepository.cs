using System;
using System.Threading.Tasks;
using FieldWorkerBot.DataAccess.Interfaces;

namespace FieldWorkerBot.DataAccess
{
    public class SnapshotRepository : ISnapshotRepository
    {
        public Task<string> Create(string base64Content)
        {
            return Task.FromResult(GetUrlMock());
        }

        private string GetUrlMock()
        {
            var guid = Guid.NewGuid().ToString();
            return $"https://www.example.com/{guid}.jpg";
        }
    }
}
