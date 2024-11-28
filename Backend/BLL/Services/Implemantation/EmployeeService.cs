using BLL.Services.Interfaces;
using DAL.Repositories.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implemantation
{

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeRepository.GetEmployeeWithInfoAsync();
        }

        public async Task<Employee> GetEmployeeById(string id)
        {
            return await _employeeRepository.GetEmployeeWithInfoAsync(id);
        }

        public async Task CreateEmployee(Employee employee)
        {
            await _employeeRepository.CreateAsync(employee);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DeleteEmployee(string id)
        {
            var employee = await _employeeRepository.GetEmployeeWithInfoAsync(id);
            if (employee != null)
            {
                await _employeeRepository.DeleteAsync(id);
            }
        }
    }
}
