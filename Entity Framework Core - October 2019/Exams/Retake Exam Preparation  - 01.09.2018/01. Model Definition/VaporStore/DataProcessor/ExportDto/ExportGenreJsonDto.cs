using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.ExportDto
{
    public class ExportGenreJsonDto
    {
        public int Id { get; set; }

        [Required]
        public string Genre { get; set; }

        public ExportGamesJsonDto[] Games { get; set; }

        public int TotalPlayers { get; set; }
    }
}
