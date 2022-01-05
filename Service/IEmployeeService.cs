using Dto.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IEmployeeService
    {
        public   IEnumerable<EmployeeDto> GetEmployeesForCompanyAsync(Guid companyId, EmployeeParameters employeeParameters);

        public EmployeeDto GetEmployee(Guid companyId, Guid id);

        public EmployeeDto CreateEmployeeForCompanyAsync(Guid companyIdm, EmployeeForCreationDto employee);

        public void DeleteEmployeeForCompany(Guid companyId);

    }
}
