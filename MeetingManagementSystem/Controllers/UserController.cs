using MeetingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeetingManagementSystem.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(string userid, string password)
        {
            if (Models.Dblayer.AuthenticateUser(userid, password))
            {
                Session.Clear();
                Session["userid"] = userid;
                return View("Home");
            }
            else
            {
                ViewBag.error = "Invalid Credentials";
                return View();
            }

        }

        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();

        }

        [HttpPost]
        public ActionResult RegisterUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else if (Dblayer.UserExists(user.userid))
            {
                ViewBag.error = "This User ID is already exists";
                return View();
            }

            else if (user.age < 18)
            {
                ViewBag.error = "Age is Incorrect";
                return View();
            }

            else if (!(user.mobile.All(char.IsDigit)))
            {
                ViewBag.error = "Mobile Number is Incorrect";
                return View();
            }
            else if (!(user.email.Contains("@") && user.email.Contains(".")))
            {
                ViewBag.error = "Entered Email is Incorrect";
                return View();
            }

            else
            {
                Dblayer.AddUser(user);
                return RedirectToAction("UserLogin");
            }
        }

        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("UserLogin");
        }



        public ActionResult BookMeetingRoom()
        {
            List<MeetingRoom> r = Dblayer.GetRooms();
            return View(r);
        }
       
        [HttpGet]
        public ActionResult BookRoom(string roomid)
        {
            Session["selectedroomid"] = roomid;
            return View();
        }
        [HttpPost]
        public ActionResult BookRoom(int people,DateTime startdate, DateTime enddate,string food)
        {
            MeetingRoom room = Dblayer.GetRoom(Session["selectedroomid"].ToString());
           
            if (room.capacity < people)
            {
                string msg = "You exceeded the capacity of the room";
                ViewBag.error = msg;
                return View();
            }
            else if (people<0)
            {
                string msg = "Please Selcet the proper number of seats";
                ViewBag.error = msg;
                return View();
            }
            else if (startdate > enddate)
            {
                string msg = "EndTime must be greater than StartTime";
                ViewBag.error = msg;
                return View();
            }
            if ((!Dblayer.IsoktoBook(Session["selectedroomid"].ToString(),startdate, enddate)))
            {
                string msg = "The Room is already booked in selected period, Please select different period  ";
                ViewBag.error = msg;
                return View();
            }

            Session["foodneeded"] = food;
            string Isfoodneeded = Session["foodneeded"].ToString();
            string bookingid=Dblayer.BookRoom(Session["selectedroomid"].ToString(), Session["userid"].ToString(), startdate, enddate, people,Isfoodneeded);
            Session["selectedbookingid"] = bookingid;
            if (Session["foodneeded"].ToString() == "no")
            {
                return RedirectToAction("Payment");
            }
            return RedirectToAction("AddFood");
        }

        public ActionResult ManageBookings()
        {
            List<Bookings> reservations = Dblayer.GetFinalOrder(Session["userid"].ToString());
            return View(reservations);
        }


        public ActionResult CancleBooking(string bookingid)
        {
            Dblayer.DeleteBooking(bookingid);

            return RedirectToAction("ManageBookings");
        }


        
        public ActionResult AddFood()
        {
            List<FoodCombo> food = Dblayer.GetFoods();
            if (Session["foodneeded"].ToString() == "no")
            {
                return RedirectToAction("Payment");
            }
                return View(food);
        }

       
        public ActionResult BookFood(string foodid)
        {
            Session["selectedfoodid"] = foodid;
            
                return RedirectToAction("selectQuantity",new { foodid = (Session["selectedfoodid"].ToString()) });
        }

        [HttpGet]
        public ActionResult selectQuantity(string foodid)
        {
            Session["foodid"] = foodid;
            FoodCombo f = Dblayer.GetFood(foodid);
            ViewBag.food = f;
            return View();
        }
        [HttpPost]
        public ActionResult selectQuantity(int quantity)
        {
            
             Dblayer.AddFood(Session["foodid"].ToString(), Session["selectedroomid"].ToString(), Session["selectedbookingid"].ToString(),quantity);
            
            return RedirectToAction("Payment");
        }

        [HttpGet]
        public ActionResult Payment()
        {
           Order b = Dblayer.Getbookingbyid(Session["selectedbookingid"].ToString());
            ViewBag.amount = b.totalcost / 5;
            return View();
        }
        [HttpPost]
        public ActionResult Payment(double amount, string number,string cvv)
        {
            Order m = Dblayer.Getbookingbyid(Session["selectedbookingid"].ToString());
            ViewBag.amount = m.totalcost / 5;
            if (amount < (m.totalcost / 5))
            {
                string msg = "You have to pay atleast 20% of total cost.";
                ViewBag.error = msg;
                return View();
            }
            if (amount >m.totalcost )
            {
                string msg = "You are paying more amount than total booking amount";
                ViewBag.error = msg;
                return View();
            }
            if (!(cvv.All(char.IsDigit)))
            {
                string msg = "Please enter proper CVV";
                ViewBag.error = msg;
                return View();
            }
            if (cvv.ToString().Length>3)
            {
                string msg = "Please enter proper CVV";
                ViewBag.error = msg;
                return View();
            }
            if (!(number.All(char.IsDigit)))
            {
                string msg = "Please Enter correct card Number";
                ViewBag.error = msg;
                return View();
            }

            Dblayer.AddPayment(Session["selectedbookingid"].ToString(), amount);
            Dblayer.deletebookings();
            return RedirectToAction("ManageBookings");
        }


        public ActionResult ViewFood(string bookingid)
        {
            Bookings booking = Dblayer.Getbookingforpayment(bookingid);

            List<FoodCombo> foods =booking.foodcombo;
            return View(foods);
        }

        [HttpGet]
        public ActionResult UpdatePassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UpdatePassword(string oldpass,string newpass,string confirmpass)
        {
            if (newpass!=confirmpass)
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
            if (!(Dblayer.updatepassword(Session["userid"].ToString(), oldpass,newpass)))
            {
                string msg = "Current Password is Wrong";
                ViewBag.error = msg;
                return View();
            }
            else
            return RedirectToAction("UserLogin");
        }


        [HttpGet]
        public ActionResult MakePayment(string bookingid)
        {
            Session["reaminingbooking"] = bookingid;
            Bookings b = Dblayer.Getbookingforpayment(bookingid);
            ViewBag.amount = b.remainamount;
            return View();
        }
        [HttpPost]
        public ActionResult MakePayment(double amount, string number, string cvv)
        {
           
            Bookings m = Dblayer.Getbookingforpayment(Session["reaminingbooking"].ToString());
            ViewBag.amount = m.remainamount;
            if (amount < m.remainamount)
            {
                string msg = "You have to pay all the remaining amount";
                ViewBag.error = msg;
                return View();
            }
            if (!(cvv.All(char.IsDigit)))
            {
                string msg = "Please enter proper CVV";
                ViewBag.error = msg;
                return View();
            }
            if (cvv.ToString().Length != 3)
            {
                string msg = "Please enter proper CVV";
                ViewBag.error = msg;
                return View();
            }
            if (number.ToString().Length < 10)
            {
                string msg = "Please Enter correct Card Number";
                ViewBag.error = msg;
                return View();
            }
            if (!(number.All(char.IsDigit)))
            {
                string msg = "Please enter proper Car Number";
                ViewBag.error = msg;
                return View();
            }

            Dblayer.AddRemainingPayment(m.bookingid, amount);

            return RedirectToAction("ManageBookings");
        }


    }
}