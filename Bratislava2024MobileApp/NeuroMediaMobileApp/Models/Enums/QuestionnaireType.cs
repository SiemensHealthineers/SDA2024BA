using System.ComponentModel.DataAnnotations;

namespace NeuroMediaMobileApp.Models.Enums
{
    public enum QuestionnaireType
    {
        Botulotoxin,

        [Display(Name = "CGI Doctor")]
        CGIDoctor,

        [Display(Name = "CGI Patient")]
        CGIPatient,

        [Display(Name = "TWSTRS Doctor")]
        TWSTRSDoctor,

        [Display(Name = "TWSTRS Patient")]
        TWSTRSPatient
    }
}
