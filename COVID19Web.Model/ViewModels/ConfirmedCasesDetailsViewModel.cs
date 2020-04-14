using System;
using System.ComponentModel.DataAnnotations;

namespace COVID19Web.Model.ViewModel
{
    public class ConfirmedCasesDetailsViewModel
    {
        [Display(Name = "Id")]
        public int _id { get; set; }
        [Display(Name = "Notification Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime notification_date { get; set; }
        [Display(Name = "Postcode")]
        public int postcode { get; set; }
        [Display(Name = "LHD Code")]
        public string lhd_2010_code { get; set; }
        [Display(Name = "Local Health District")]
        public string lhd_2010_name { get; set; }
        [Display(Name = "LGA Code")]
        public int lga_code19 { get; set; }
        [Display(Name = "Loacal Government Area")]
        public string lga_name19 { get; set; }
        [Display(Name = "Rank")]
        public decimal rank { get; set; }
    }
}