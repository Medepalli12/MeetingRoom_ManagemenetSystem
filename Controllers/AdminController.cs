using MeetingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeetingManagementSystem.Controllers
{
    public class AdminController : Controller
    {

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(string adminid, string password)
        {
            if (Models.Dblayer.authenticateadmin(adminid, password))
            {
                Session.Clear();
                Session["adminid"] = adminid;
                return View("Home");
            }
            else
            {
                ViewBag.error = "Invalid Credentials";
                return View();
            }

        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("AdminLogin");
        }




        [HttpGet]
        public ActionResult AddRoom()
        {

            return View();

        }

        [HttpPost]
        public ActionResult AddRoom(MeetingRoom D, HttpPostedFileBase file)
        {
            if (Dblayer.RoomExist(D.name))
            {
                ViewBag.error = "Room with same  Name is already exist";
                return View();
            }
            else if (D.name.ToString() == "")
            {
                ViewBag.error = "Please Enter Name ofthe Property";
                return View();
            }
            else if (D.venue.ToString() == "")
            {
                ViewBag.error = "Please Enter Venue";
                return View();
            }
            else if (D.costperday.ToString() == "")
            {
                ViewBag.error = "Please Enter Cost";
                return View();
            }

            else if (file.ToString() == "")
            {
                ViewBag.error = "Please Sect the photo of the Property";
                return View();
            }

            else
            {
                var allowedExtensions = new[] {
               ".Jpg", ".png", ".jpg", "jpeg"
                };
                var filename = Path.GetFileName(file.FileName);
                var ext = Path.GetExtension(file.FileName);
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    Guid guid = Guid.NewGuid();
                    string str = guid.ToString();
                    D.roomid = str;
                    var path = Path.Combine(Server.MapPath("~/photos/"), filename);
                    file.SaveAs(path);
                    D.imageurl = filename;
                    D.status = "NotBooked";
                    Dblayer.AddRoom(D);
                    return RedirectToAction("ManageRoom");
                }
                else
                {
                    ViewBag.error = "Please choose only Image file";
                    return View();
                }
            }
        }


        public ActionResult ManageRoom()
        {

            List<MeetingRoom> rooms = Dblayer.GetRooms();
            return View(rooms);
        }

        public ActionResult DeleteRoom(string roomid)
        {
            Dblayer.DeleteRoom(roomid);
            return RedirectToAction("ManageRoom");
        }
        [HttpGet]
        public ActionResult EditRoom(string roomid)
        {

            Session["selectedroom"] = roomid;
          
            return View();
        }
        [HttpPost]
        public ActionResult EditRoom(MeetingRoom m )
        {
            Dblayer.UpdateRoom(m, Session["selectedroom"].ToString());
           
            return RedirectToAction("ManageRoom");
        }

        public ActionResult ViewBookings()
        { 
            List<Bookings> res = Dblayer.GetExistingBookings();
            return View(res);
        }


        [HttpGet]
        public ActionResult UpdatePassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UpdatePassword(string oldpass, string newpass, string confirmpass)
        {
            if (newpass != confirmpass)
            {
                string msg = "Password MisMatched";
                ViewBag.error = msg;
                return View();
            }

            if (oldpass == confirmpass)
            {
                string msg = "Old and New password  is same";
                ViewBag.error = msg;
                return View();
            }
            if (!(Dblayer.updatepasswordadmin(Session["adminid"].ToString(), oldpass, newpass)))
            {
                string msg = "Current Password is Wrong";
                ViewBag.error = msg;
                return View();
            }
            else
                return RedirectToAction("AdminLogin");
        }


        [HttpGet]
        public ActionResult AddFoodCombo()
        {

            

            return View();
        }
        [HttpPost]
        public ActionResult AddFoodCombo(FoodCombo m)
        {
            m.quantity = 0;
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            m.foodid = str;
            Dblayer.AddFoodCombo(m);

            return RedirectToAction("ManageFoodCombo");
        }

        [HttpGet]
        public ActionResult EditFoodCombo(string foodcombo)
        {
            Session["foodcombo"] = foodcombo;
            return View();
        }
        [HttpPost]
        public ActionResult EditFoodCombo(FoodCombo f)
        {
            Dblayer.EditFoodCombo( Session["foodcombo"].ToString(),f);

            return RedirectToAction("ManageFoodCombo");
        }

        public ActionResult ManageFoodCombo()
        {
            List<FoodCombo> foods = Dblayer.Getallfoodcombos();
            return View(foods);
        }
        public ActionResult DeleteFoodCombo(string foodid)
        {
            Dblayer.DeleteFood(foodid);
            return RedirectToAction("ManageFoodCombo");
        }
    }
}