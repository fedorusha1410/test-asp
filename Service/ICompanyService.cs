using Entities.DataTransferObjects;

namespace ServiceLayer
{
    public interface ICompanyService
    {
        public IEnumerable<CompanyDto> GetCompany();

        public CompanyDto  GetCompanyById(Guid id);

        public void CreateCompany(CompanyForCreationDto company);

        public void DeleteCompany(Guid id);

        //TO DO



    }
}