using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoGuru.Assistant.ThirdParty.AutoData.Models;
using AutoGuru.Data.Vehicle.Services;

using CarFixed.DS.DM;

namespace CarFixed.Models
{
    public class QuoteModel
    {
        [Required]
        public string vrm { get; set; }
        [Display(Name = "Make")]
        public string make { get; set; }
        public string modeltype { get; set; }
        public string year { get; set; }
        public string fuelType { get; set; }

        [Required]
        public string Postcode { get; set; }
        public string Service { get; set; }
        public string SubService { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public List<string> list { get; set; }
        public List<string> subList { get; set; }



        public string Firstname { get; set; }

        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public bool IsNewUser { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ExistingUserEmail { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string ExistingUserPassword { get; set; }

        public QuoteModel()
        {
            this.IsNewUser = true;
        }
    }

    public class BasicSubCategoryRepairModel
    {
        public List<BasicSubCategoryRepairRef> RepairRefs { get; set; }

        public List<AutoDataRepairTime> RelevantRepairs { get; set; }
    }

    public class BasicSubCategoryServiceModel
    {
        public ServiceRegimeWrapper ServiceWrapper { get; set; }

    }
}