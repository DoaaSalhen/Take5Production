using AutoMapper;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Take5.Models.Models.MasterModels;
using Take5.Services.Contracts;
using Take5.Services.Models.MasterModels;

namespace Take5.Services.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee, long> _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IRepository<Employee, long> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Employee> CreateEmployee(EmployeeModel model)
        {
            try
            {
                model.IsVisible = true;
                Employee employee = _mapper.Map<Employee>(model);
                Employee addedEmployee = _repository.Add(employee);
                return addedEmployee;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public bool DeleteEmployee(long id)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeModel> GetAllEmployees()
        {
            try
            {
                List<Employee> employees = _repository.Find(e => e.IsVisible == true).ToList();
                List<EmployeeModel> models = _mapper.Map<List<EmployeeModel>>(employees);
                return models;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public EmployeeModel GetEmployee(long id)
        {
            try
            {
                var employee = _repository.Find(e => e.IsVisible == true&&e.EmployeeNumber == id).FirstOrDefault();
                if(employee!= null)
                {
                    EmployeeModel model = _mapper.Map<EmployeeModel>(employee);
                    return model;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Employee GetEmployeeByUserId(string userId)
        {
            try
            {
                var employee = _repository.Find(e => e.IsVisible == true && e.UserId == userId).FirstOrDefault();
                return employee;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Task<bool> UpdateEmployee(EmployeeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
