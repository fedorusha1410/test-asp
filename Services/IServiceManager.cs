using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IServiceManager
    {
         ICompanyService companyService { get; }

         IEmployeeService employeeService { get; }

    }
}
