using System.ComponentModel.DataAnnotations;

namespace GraduateWork.Enums
{
    public enum Roles
    {
        [Display(Name = "Administrator")]
        Administrator,
        [Display(Name = "Product Manager")]
        Product_Manager,
        [Display(Name = "Back-End Developer")]
        Back_end_Dev,
        [Display(Name = "Front-End Developer")]
        Front_end_Dev,
        [Display(Name = "UI/UX Designer")]
        UI_UX_designer,
        [Display(Name = "QAEngineer")]
        QAEngineer,
        [Display(Name = "Business Analytic")]
        Business_analytic
    }
}
