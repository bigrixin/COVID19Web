using COVID19Web.Model.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COVID19Web.Model.ViewModel
{
    public class RetrieveDataViewModel
    {
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "* Postcode must be 4 character in length.")]
        public string Postcode { get; set; }
        public string NSWCount { get; set; }

        public List<NSWCaseStatisticsViewModel> NSWCaseStatisticsVM { get; set; }

        public List<ConfirmedCasesDetailsViewModel> DetailsResult { get; set; }
        public List<ConfirmedCasesDailyCountViewModel> DailyCountResult { get; set; }
        public List<string> Suburbs { get; set; }
    }
}