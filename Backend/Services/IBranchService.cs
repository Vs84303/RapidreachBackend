using RapidReachNET.Models;
using RapidReachNET.Repositories;

namespace RapidReachNET.Services
{
    using RapidReachNET.Models;

 
        public interface IBranchService
        {
            Task<Branch> AddBranchAsync(Branch branch);
            Task<IEnumerable<Branch>> GetAllBranchesAsync();
            Task<Branch?> GetBranchByIdAsync(long id);
        }
    

}
