using Dto.Dto;
using Dto.RequestFeatures;
using Entities.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IEmployeeService
    {
        public   Task<IEnumerable<EmployeeDto>> GetEmployeesForCompanyAsync(Guid companyId, EmployeeParameters employeeParameters);

        public Task<EmployeeDto> GetEmployee(Guid companyId, Guid id);

        public Task<EmployeeDto> CreateEmployeeForCompany(Guid companyIdm, EmployeeForCreationDto employee);

        public  Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id);

        public Task UpdateEmployeeForCompanyAsync(EmployeeForUpdateDto employeeDto, Employee employee);
       



    }
}
