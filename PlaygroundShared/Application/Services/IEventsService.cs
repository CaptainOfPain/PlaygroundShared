using System.Threading.Tasks;

namespace PlaygroundShared.Application.Services
{
    public interface IEventsService : IService
    {
        Task ExecuteEventsAsync();
        void Clear();
    }
}