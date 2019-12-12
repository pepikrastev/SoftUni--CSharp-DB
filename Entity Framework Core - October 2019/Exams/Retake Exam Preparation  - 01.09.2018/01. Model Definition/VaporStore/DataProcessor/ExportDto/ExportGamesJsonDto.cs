using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.ExportDto
{
    public class ExportGamesJsonDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [JsonProperty("Title")]
        public string Title { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Tags { get; set; }

        [Required]
        public int Players { get; set; }
    }
}
