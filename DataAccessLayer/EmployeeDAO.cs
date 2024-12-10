using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessLayer
{
    public class EmployeeDAO
    {
        private readonly Team13QlbhContext _context;

        public EmployeeDAO(Team13QlbhContext context)
        {
            _context = context;
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.Set<Employee>()
                .Include(e => e.Role)
                .Include(e => e.Orders)
                .ToList();
        }

        public Employee GetEmployeeById(int employeeId)
        {
            return _context.Set<Employee>()
                .Include(e => e.Role)
                .Include(e => e.Orders)
                .FirstOrDefault(e => e.EmployeeId == employeeId);
        }

        public Employee GetEmployeeByUsername(string username)
        {
            return _context.Set<Employee>()
                .Include(e => e.Role)
                .FirstOrDefault(e => e.Username == username);
        }

        public void AddEmployee(Employee employee)
        {
            _context.Set<Employee>().Add(employee);
            _context.SaveChanges();
        }

        public void UpdateEmployee(Employee employee)
        {
            _context.Set<Employee>().Update(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(int employeeId)
        {
            var employee = GetEmployeeById(employeeId);
            if (employee != null)
            {
                _context.Set<Employee>().Remove(employee);
                _context.SaveChanges();
            }
        }

        public List<Employee> GetEmployeesByRole(int roleId)
        {
            return _context.Set<Employee>()
                .Include(e => e.Role)
                .Where(e => e.RoleId == roleId)
                .ToList();
        }

        public bool ValidateLogin(string username, string password)
        {
            return _context.Set<Employee>()
                .Any(e => e.Username == username && e.Password == password);
        }

        public List<Employee> SearchEmployees(string searchTerm)
        {
            return _context.Set<Employee>()
                .Include(e => e.Role)
                .Where(e =>
                    e.EmployeeName.Contains(searchTerm) ||
                    e.Username.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm))
                .ToList();
        }

        public int GetTotalEmployeeCount()
        {
            return _context.Set<Employee>().Count();
        }
    }
}