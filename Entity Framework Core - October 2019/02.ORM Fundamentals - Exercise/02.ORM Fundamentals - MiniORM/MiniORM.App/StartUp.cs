using MiniORM.App.Data;
using MiniORM.App.Data.Entities;
using System.Linq;

namespace MiniORM.App
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var connectionString = "Server=.\\SQLEXPRESS;Database=MiniORM;Integrated Security=True";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Pesho",
                LastName = "Peshev",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true,
            });

            Employee employee = context.Employees.Last();
            employee.FirstName = "Modified";

            context.SaveChanges();
        }
    }
}