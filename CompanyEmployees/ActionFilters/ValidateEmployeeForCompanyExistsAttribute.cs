using Application.Services;
using ClassLibrary;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.ActionFilters
{
    public class ValidateEmployeeForCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICompanyService _companyService;

        public ValidateEmployeeForCompanyExistsAttribute( ILoggerManager loggerManager, IEmployeeService employeeService, ICompanyService companyService)
        {
            _employeeService = employeeService;
          
            _logger = loggerManager;
            _companyService = companyService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH"));
            var companyId = (Guid)context.ActionArguments["companyId"];
            var company = _companyService.GetCompanyByIdAsync(companyId);
            if (company == null) 
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            var id = (Guid)context.ActionArguments["id"];
            var employee = _employeeService.GetEmployee(companyId, id);
            if (employee == null) { _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult(); 
            }
            else
            {
                
                await next();
            }
        }
    }
 }

