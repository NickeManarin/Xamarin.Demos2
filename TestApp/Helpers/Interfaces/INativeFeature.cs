using System.Threading.Tasks;

namespace TestApp.Helpers.Interfaces
{
    public interface INativeFeature
    {
        bool IsLocationEnabled();
        Task<bool> TurnOnLocationSettings();
    }
}