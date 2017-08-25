using System.ComponentModel.DataAnnotations;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.Account {
    public class ResetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "\"{0}\" muss mindestens {2} Zeichen lang sein.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kennwort best�tigen")]
        [Compare("Password", ErrorMessage = "Das Kennwort stimmt nicht mit dem Best�tigungskennwort �berein.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}