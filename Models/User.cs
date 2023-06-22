using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingManagementSystem.Models
{[BsonIgnoreExtraElements]
    public class User
    {

        [Required]
        [Display(Name = "UserID")]
        public string userid { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [Display(Name = "Age")]
        public int age { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string firstname { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string lastname { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        public string mobile { get; set; }
    }
}