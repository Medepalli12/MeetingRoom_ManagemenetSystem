using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingManagementSystem.Models
{
    [BsonIgnoreExtraElements]
    public class Bookings
    {

        [Display(Name = "BookingID")]
        public string bookingid { get; set; }

        [Required]
        [Display(Name = "UserID")]
        public string userid { get; set; }


        [Required]
        [Display(Name = "RoomID")]
        public string roomid { get; set; }
        [Required]
        [Display(Name = "Room Name")]
        public string roomname { get; set; }

        [Required]
        [Display(Name = "StartTime")]
        public string starttime { get; set; }
        [Required]
        [Display(Name = "EndTime")]
        public string endtime { get; set; }
        [Required]
        [Display(Name = "Is Food Needed")]
        public string isfoodneeded { get; set; }

        [Display(Name = "Total Seats")]
        public int totalseats { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Total Cost")]
        public double totalcost { get; set; }

        [Display(Name = "Food")]
        public List<FoodCombo> foodcombo { get; set; }

        [Display(Name = "Paid Amount")]
        public double advamount { get; set; }

        [Display(Name = "Remaining Cost")]
        public double remainamount { get; set; }
    }
}