using RapidReachNET.DTO;

namespace RapidReachNET.Repositories
{
    public interface IFeedbackRepository
    {
        Task<long> AddFeedbackAsync(long userId, string comment);
        Task<List<FeedbackDTO>> GetAllFeedbackAsync();
    }
}
