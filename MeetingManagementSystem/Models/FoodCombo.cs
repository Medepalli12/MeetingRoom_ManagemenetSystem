using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingManagementSystem.Models
{[BsonIgnoreExtraElements]
    public class FoodCombo
    {
        [Required]
        [Display(Name = "FoodID")]
        public string  foodid { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string  name { get; set; }
        [Required]
        [Display(Name = " Food-1")]
        public string  food1 { get; set; }
        [Required]
        [Display(Name = " Food-2")]
        public string food2 { get; set; }
        [Required]
        [Display(Name = " Food-3")]
        public string food3 { get; set; }
        [Required]
        [Display(Name = "Cost per Head ($)")]
        public double costperperson { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
    }
}