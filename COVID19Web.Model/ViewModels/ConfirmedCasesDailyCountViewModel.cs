using System;
using System.ComponentModel.DataAnnotations;

namespace COVID19Web.Model.ViewModels
{
    public class ConfirmedCasesDailyCountViewModel
    {
        [DataType(DataType.Date)]
        [Display (Name = "Notification Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Display(Name = "Confirmed Count")]
        public int Count { get; set; }
    }
}