using RapidReachNET.Models;

namespace RapidReachNET.Repositories
{
    public interface IBranchRepository
    {
        Task<Branch> AddBranchAsync(Branch branch);
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task<Branch?> GetBranchByIdAsync(long id);

        Task<Branch?> GetBranchByEmployeeIdAsync(long employeeId);
    }
}
