namespace Cinema.DataProcessor.ImportDto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Ticket")]
    public class TicketCustomerImportDto
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        [XmlElement("Price")]
        public decimal Price { get; set; }

        [Required]
        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }
    }
}
