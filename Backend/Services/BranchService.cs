using Microsoft.EntityFrameworkCore;
using RapidReachNET.Models;
using RapidReachNET.Repositories;

namespace RapidReachNET.Services
{
   



    
        public class BranchService : IBranchService
        {
            private readonly IBranchRepository _branchRepository;
            private readonly IEmployeeRepository _userRepository;

            public BranchService(IBranchRepository branchRepository, IEmployeeRepository userRepository)
            {
                _branchRepository = branchRepository;
                _userRepository = userRepository;
            }

            public async Task<Branch> AddBranchAsync(Branch branch)
            {
               
                //var employee = await _userRepository.GetUserByIdAsync(employeeId);

                //if (employee == null || employee.Role != Role.ROLE_EMPLOYEE.ToString())
                  //  throw new InvalidOperationException("User must be an Employee to assign to a branch.");

                //var existingBranch = await _branchRepository.GetBranchByEmployeeIdAsync(employeeId);
               // if (existingBranch != null)
                   // throw new InvalidOperationException("Employee is already assigned to another branch.");

                //branch.UserId = employee.UserId;


                
                var addedBranch = await _branchRepository.AddBranchAsync(branch);

                return addedBranch;
            }






            public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _branchRepository.GetAllBranchesAsync();
        }

        public async Task<Branch?> GetBranchByIdAsync(long id)
        {
            return await _branchRepository.GetBranchByIdAsync(id);
        }
    }
}
