using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public  class ServiceManager : IServiceManager
    {
        private ICompanyService _companyService;

        private IEmployeeService _employeeService;

     

        public ICompanyService companyService
        {
            get { return _companyService; }
        }



        public IEmployeeService employeeService
        {
            get { return _employeeService; }
        }
    }
}
