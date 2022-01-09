using Application.Services;
using AutoMapper;
using ClassLibrary;
using CompanyEmployees.ActionFilters;
using Contracts;
using Dto.Dto;
using Dto.RequestFeatures;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly ICompanyService _companyService;
             
       

        public EmployeesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IEmployeeService employeeService, ICompanyService companyService)
        {
            _repository = repository; 
            _logger = logger;
            _mapper = mapper;
            _employeeService = employeeService;
            _companyService = companyService;
        }

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.ValidAgeRange)
                return BadRequest("Max age can't be less than min age.");

            var company = _companyService.GetCompanyByIdAsync(companyId);
            if (company == null)
            {

                return NotFound();
            }
            var employees = _employeeService.GetEmployeesForCompanyAsync(companyId, employeeParameters);
            return Ok(employees);

        }

        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = _companyService.GetCompanyByIdAsync(companyId);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var employeeDb = _employeeService.GetEmployee(companyId, id);

            if (employeeDb == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(employeeDb);
            }
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var company = _companyService.GetCompanyByIdAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            var employeeToReturn = _employeeService.CreateEmployeeForCompany(companyId, employee);
            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = _companyService.GetCompanyByIdAsync(companyId);
            if (company == null)
            { 
                return NotFound();
            }
            _employeeService.DeleteEmployeeForCompanyAsync(companyId, id);
            await _repository.SaveAsync();
            return NoContent();
        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee) 
        {
            var employeeEntity = HttpContext.Items["employee"] as Employee;
           
            _mapper.Map(employee, employeeEntity);
             await _repository.SaveAsync();
            return NoContent(); 
        }

    }
}
