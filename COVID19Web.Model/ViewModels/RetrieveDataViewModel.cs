﻿using COVID19Web.Model.ViewModels;
using HtmlAgilityPack;
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

         public string NSWCaseStatisticsVM { get; set; }  //return html content
        public List<NSWCaseStatisticsViewModel> NSWCaseStatisticsListVM { get; set; }

        public AustraliaAndWorldCaseStatisticsViewModel AustraliaAndWorldCaseStatisticsVM { get; set; }
        public List<ConfirmedCasesDetailsViewModel> DetailsResult { get; set; }
        public List<ConfirmedCasesDailyCountViewModel> DailyCountResult { get; set; }
        public List<string> Suburbs { get; set; }
    }
}