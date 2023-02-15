using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Contracts
{
    public interface IEmployeeService
    {
        List<EmployeeModel> GetAllEmployees();
        Task<Employee> CreateEmployee(EmployeeModel model);
        Task<bool> UpdateEmployee(EmployeeModel model);
        bool DeleteEmployee(long id);
        EmployeeModel GetEmployee(long id);
        Employee GetEmployeeByUserId(string userId);
    }
}
