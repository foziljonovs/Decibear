using Decibear.Bot.Models;

namespace Decibear.Bot.Services;

public interface IAvaterService
{
    public Task<byte[]> GetAsync(Request request);
}
