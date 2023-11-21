using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Specifications
{
    public class EmployeeWithDepartmentSpecification :BaseSpecifications<Employee>
    {
        public EmployeeWithDepartmentSpecification() {
        Includes.Add(E=>E.Department);
        }
    public EmployeeWithDepartmentSpecification(int id) :base(E=>E.Id ==id)
        {
            Includes.Add(E => E.Department);
        }
    }
}
