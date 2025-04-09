using System.ComponentModel.DataAnnotations;

using NeuroMedia.Domain.Enums;

namespace NeuroMedia.WebUI.Models
{
    public class VisitDataTransfer
    {
        [Required(ErrorMessage = "Visit date is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfVisit { get; set; }
        [Required(ErrorMessage = "Visit type is required")]
        public VisitType VisitType { get; set; }
        public string? Note { get; set; }
        public int VisitId { get; set; }
    }
}
