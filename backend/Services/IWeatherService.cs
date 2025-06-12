using System.Threading.Tasks;
using MyFullstackApp.Models;

namespace MyFullstackApp.Backend.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast[]> GetForecastAsync();
    }
}