namespace Application.Interfaces
{
    public interface IAIService
    {
        Task<string> GetResponseAsync(Guid userId, string message);
    }
}
