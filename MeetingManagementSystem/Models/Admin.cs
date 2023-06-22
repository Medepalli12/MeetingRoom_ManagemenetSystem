using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingManagementSystem.Models
{
    [BsonIgnoreExtraElements]
    public class Admin
    {
        [Required]
        public string adminid { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}