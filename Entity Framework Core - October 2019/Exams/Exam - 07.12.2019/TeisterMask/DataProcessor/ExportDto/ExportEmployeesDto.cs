using System;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class ExportEmployeesDto
    {
        public string Username { get; set; }

        public ExportTasksEmpDto[] Tasks { get; set; }
    }
}
