
using AutoMapper;
using ClassLibrary;
using Contracts;
using Dto.Dto;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    //TO DO
    public  class CompanyService : ICompanyService
    {

        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            _repository = repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();
            CompanyDto companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }

        public async Task DeleteCompanyAsync(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, false);
            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
           
        }

        public async Task<IEnumerable<CompanyDto>> GetCompanyAsync()
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges: false);


            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);


            return companiesDto;

        }

        public async Task<CompanyDto> GetCompanyByIdAsync(Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                //TO DO
                return null;
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(company);
                return companyDto;
            }
        }


        public async Task<IEnumerable<CompanyDto>> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
        {

            if (companyCollection == null)
            {
                _logger.LogError("Company collection sent from client is null.");
                // TO DO 
                return null;
            }
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();
            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companyCollectionToReturn;
        }

        public async Task UpdateCompanyAsync(CompanyForUpdateDto companyDto)
        {
            Company company = _mapper.Map<Company>(companyDto);
           _mapper.Map(companyDto, company);
            await _repository.SaveAsync();
            
        }

       

        public bool HasCompany(Guid id)
        {
            var company = _repository.Company.GetCompanyAsync(id, false);
            if (company != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
