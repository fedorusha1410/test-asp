using Application.Services;
using AutoMapper;
using ClassLibrary;
using CompanyEmployees.ActionFilters;
using CompanyEmployees.ModelBinders;
using Contracts;
using Dto.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CompanyEmployees.Controllers
{
    [Route("api/companies")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class CompaniesController : Controller
    {

        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;
   


        public CompaniesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, ICompanyService companyService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _companyService = companyService; 
        }

        [HttpGet(Name = "GetCompanies"), Authorize(Roles = "Manager")]
        public IActionResult GetCompanies()
        {
            //throw new Exception("Exception"); for testing 
            var companies = _companyService.GetCompanyAsync();
            return Ok(companies);

        }

        [HttpGet("{id}", Name = "CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(company);
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public  IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var companyToReturn = _companyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
            //check in postman

        }


        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        //TO DO
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        { 
            if (ids == null)
            { 
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return Ok(companiesToReturn);
        }


        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var companyCollectionToReturn = _companyService.CreateCompanyCollectionAsync(companyCollection) as IEnumerable<CompanyDto>;
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CompanyCollection", new { ids }, companyCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public IActionResult DeleteCompany(Guid id)
        {
            _companyService.DeleteCompanyAsync(id);
            return NoContent();
        }


        [HttpPut("{id}")]

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateCompanyAsync(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _companyService.UpdateCompanyAsync(company);
            return NoContent();
        }

    }
}
