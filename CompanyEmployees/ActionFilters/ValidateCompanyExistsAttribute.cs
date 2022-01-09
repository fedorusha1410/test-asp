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
    public class ValidateCompanyExistsAttribute : IAsyncActionFilter
    { 
        private readonly ILoggerManager _logger;
        private readonly ICompanyService _companyService;

        public ValidateCompanyExistsAttribute(ILoggerManager loggerManager, ICompanyService companyService)
        {
            _logger = loggerManager;
            _companyService = companyService;

        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (Guid)context.ActionArguments["id"];
            if (!_companyService.HasCompany(id)) 
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database."); 
                context.Result = new NotFoundResult(); 
            }
            else
            {
                await next(); 
            }
        }
    }
}
