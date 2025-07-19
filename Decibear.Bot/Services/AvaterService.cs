using Decibear.Bot.Extensions;
using Decibear.Bot.Models;

namespace Decibear.Bot.Services;

public class AvaterService(
    HttpClient httpClient,
    IConfiguration configuration) : IAvaterService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    public async Task<byte[]> GetAsync(Request request)
    {
        try
        {
            _httpClient.BaseAddress = new Uri($"{_configuration["Decibear:BaseUrl"]}/{request.Style.UrlStyle()}/png?seed={request.Seed}");

            var response = await _httpClient.GetAsync(_httpClient.BaseAddress);

            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsByteArrayAsync();
            else
                throw new Exception($"So'rov bajarilmadi! ex: {response.StatusCode}");
        }
        catch(Exception ex)
        {
            throw new Exception("So'rov yuborishda xatolik yuz berdi", ex);
        }
    }
}
