using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingManagementSystem.Models
{[BsonIgnoreExtraElements]
    public class MeetingRoom
    {
        [Display(Name = "RoomID")]
        public string roomid { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }


        [Required]
        [Display(Name = "Venue")]
        public string venue { get; set; }

        [Required]
        [Display(Name = "Facilities")]
        public string facilities { get; set; }
        [Required]
        [Display(Name = "Photo")]
        public string imageurl { get; set; }
        [Required]
        [Display(Name = "Cost Per Day")]
        public double costperday { get; set; }

        [Display(Name = "capacity")]
        public int capacity { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

    }
}