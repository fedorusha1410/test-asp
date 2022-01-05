using AutoMapper;
using ClassLibrary;
using Contracts;
using Dtos.DataTransferObjects;
using Dtos.RequestFeatures;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class EmployeeService : IEmployeeService
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




        public  EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee)
        {
            var company =  _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return null;
            }
            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
           _repository.SaveAsync();
            EmployeeDto employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;



        }



        public async void DeleteEmployeeForCompanyAsync(Guid companyId, Employee employee)
        {

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");

            }
            else
            {
                _repository.Employee.DeleteEmployee(employee);
                await _repository.SaveAsync();
            }
 
        }

        public  EmployeeDto GetEmployee(Guid companyId, Guid id)
        {
            var company =  _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return null;
            }
            var employeeDb = _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);
            if (employeeDb == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return null;
            }
            EmployeeDto employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public async IEnumerable<EmployeeDto> GetEmployeesForCompanyAsync(Guid companyId, EmployeeParameters employeeParameters)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                //TO DO
                return new List<EmployeeDto>();
            }

            var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);


            IEnumerable<EmployeeDto> employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

            return employeesDto;


        }


        public async void UpdateEmployeeForCompanyAsync(EmployeeForUpdateDto employeeDto, Employee employee)
        {
            _mapper.Map(employeeDto, employee);
            await _repository.SaveAsync();
        }



    }
}
