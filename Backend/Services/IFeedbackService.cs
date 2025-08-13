using RapidReachNET.DTO;

namespace RapidReachNET.Services
{
    public interface IFeedbackService
    {
        Task<long> AddFeedbackAsync(long userId, string comment);
        Task<List<FeedbackDTO>> GetAllFeedbackAsync();
    }
}
