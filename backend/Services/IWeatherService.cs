using System.Threading.Tasks;
using MonadicPilot.Models;

namespace MonadicPilot.Backend.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast[]> GetForecastAsync();
    }
}