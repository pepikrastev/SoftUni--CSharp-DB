using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ImportProjectsDto
    {
        //[XmlElement("Name")]
        [MinLength(2), MaxLength(40), Required]
        public string Name { get; set; }

        [Required]
      //  [XmlElement("OpenDate")]
        public string OpenDate { get; set; }

      //  [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("Tasks")]
        public ImportTaskForProjectDto[] Tasks { get; set; } 
    }
}
