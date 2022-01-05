using Dtos.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;

namespace Services
{
    public interface ICompanyService
    {
        public IEnumerable<CompanyDto> GetCompanyAsync();

        public CompanyDto  GetCompanyByIdAsync(Guid id);

        public CompanyDto CreateCompanyAsync(CompanyForCreationDto company);

        public void DeleteCompanyAsync(Company company);

        public IEnumerable<CompanyDto> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);

        public void UpdateCompanyAsync(CompanyForUpdateDto companyDto, Company company);

        //TO DO



    }
}