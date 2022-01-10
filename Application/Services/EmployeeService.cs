
using AutoMapper;
using ClassLibrary;
using Contracts;
using Dto.Dto;
using Dto.RequestFeatures;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            _repository = repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;

        }




        public async  Task<EmployeeDto> CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return null;
            }
            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
           await _repository.SaveAsync();
            EmployeeDto employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;



        }



        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id)
        {

            var company =  await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                
            }
            else
            {
                var employee =  _repository.Employee.GetEmployeeAsync(companyId, id, false) ;
  
                _repository.Employee.DeleteEmployee(employee);
                 _repository.SaveAsync();
            }
        }

        public async  Task<EmployeeDto> GetEmployee(Guid companyId, Guid id)
        {
            var company =  await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return null;
            }
            var employeeDb =  _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
            if (employeeDb == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return null;
            }
            EmployeeDto employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesForCompanyAsync(Guid companyId, EmployeeParameters employeeParameters)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return null;
            }

            var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);


            var employeesDto =  _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
          
            return employeesDto;


        }


        public async Task UpdateEmployeeForCompanyAsync(EmployeeForUpdateDto employeeDto, Employee employee)
        {
            _mapper.Map(employeeDto, employee);
            await _repository.SaveAsync();
        }



    }
}
