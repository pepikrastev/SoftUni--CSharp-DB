namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ImportDto;
    using System.IO;
    using TeisterMask.Data.Models;
    using System.Text;
    using AutoMapper;
    using System.Globalization;
    using System.Linq;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProjectsDto[]), new XmlRootAttribute("Projects"));

            var projectionDtos = (ImportProjectsDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            HashSet<Project> projects = new HashSet<Project>();

            StringBuilder sb = new StringBuilder();

            foreach (var projectDto in projectionDtos)
            {

                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    var project = new Project
                    {
                        Name = projectDto.Name,
                        OpenDate = DateTime.ParseExact(projectDto.OpenDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture),
                        DueDate = string.IsNullOrEmpty(projectDto.DueDate)
                                 ? (DateTime?)null
                                 : DateTime.ParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    };

                    foreach (var taskDto in projectDto.Tasks)
                    {
                        var taskOpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        var taskDueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy",
                            CultureInfo.InvariantCulture);

                        if (!IsValid(taskDto) || taskOpenDate < project.OpenDate || taskDueDate > project.DueDate)
                        {
                            sb.AppendLine(ErrorMessage);
                        }
                        else
                        {
                            var task = new Task
                            {
                                Name = taskDto.Name,
                                OpenDate = taskOpenDate,
                                DueDate = taskDueDate,
                                ExecutionType = Enum.Parse<ExecutionType>(taskDto.ExecutionType),
                                LabelType = Enum.Parse<LabelType>(taskDto.LabelType)
                               // ExecutionType = (ExecutionType)Enum.Parse(typeof(ExecutionType),taskDto.ExecutionType),
                               // LabelType = (LabelType)Enum.Parse(typeof(LabelType), taskDto.LabelType)
                            };

                            project.Tasks.Add(task);
                        }
                    }

                    projects.Add(project);

                    sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
                }
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeeDtos = JsonConvert.DeserializeObject<ImportEmployeesDto[]>(jsonString);

            HashSet<Employee> employees = new HashSet<Employee>();
            var sb = new StringBuilder();

            foreach (var employeeDto in employeeDtos)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                }
                else
                {
                    var employee = new Employee
                    {
                        Username = employeeDto.Username,
                        Email = employeeDto.Email,
                        Phone = employeeDto.Phone

                    };

                    var taskDtos = employeeDto.Tasks.Distinct();

                    foreach (var taskId in taskDtos)
                    {
                        var validTask = context.Tasks.Any(t => t.Id == taskId);

                        if (!validTask)
                        {
                            sb.AppendLine(ErrorMessage);
                        }
                        else
                        {
                            var emTask = new EmployeeTask { TaskId = taskId };
                            employee.EmployeesTasks.Add(emTask);
                        }
                    }

                    employees.Add(employee);

                    sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
                   
                }
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}