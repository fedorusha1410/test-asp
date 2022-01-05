
using Dtos.DataTransferObjects;
using Dtos.RequestFeatures;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IEmployeeService
    {
        public   IEnumerable<EmployeeDto> GetEmployeesForCompanyAsync(Guid companyId, EmployeeParameters employeeParameters);

        public EmployeeDto GetEmployee(Guid companyId, Guid id);

        public EmployeeDto CreateEmployeeForCompany(Guid companyIdm, EmployeeForCreationDto employee);

        public  void DeleteEmployeeForCompanyAsync(Guid companyId, Employee employee);

        public void UpdateEmployeeForCompanyAsync(EmployeeForUpdateDto employeeDto, Employee employee);
       



    }
}
