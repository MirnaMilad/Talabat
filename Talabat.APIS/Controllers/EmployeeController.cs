using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.core.Entities;
using Talabat.core.Repositories;
using Talabat.core.Specifications;

namespace Talabat.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeeController(IGenericRepository<Employee> EmployeeRepo) {
            _employeeRepo = EmployeeRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var Spec = new EmployeeWithDepartmentSpecification();
            var Employee = await _employeeRepo.GetAllWithSpecAsync(Spec);
            return Ok(Employee);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var Spec = new EmployeeWithDepartmentSpecification(id);
            var Employee = _employeeRepo.GetEntityWithSpecAsync(Spec);
            return Ok(Employee);
        }
    }
}
