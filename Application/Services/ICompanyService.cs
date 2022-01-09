using Dto.Dto;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICompanyService
    {
        public Task<IEnumerable<CompanyDto>> GetCompanyAsync();

        public Task<CompanyDto>  GetCompanyByIdAsync(Guid id);

        public Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);

        public void DeleteCompanyAsync(Guid CompanyId);

        public Task<IEnumerable<CompanyDto>> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);

        public Task UpdateCompanyAsync(CompanyForUpdateDto companyDto);

        public bool HasCompany(Guid id);

        //TO DO



    }
}