using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingManagementSystem.Models
{
    public class Dblayer
    {
        public static string dbs = "mongodb://localhost:27017";
        public static MongoClient client = new MongoClient(dbs);
        public static IMongoDatabase db = client.GetDatabase("MeetingManagementSystem");
        public static IMongoCollection<User> allusers = db.GetCollection<User>("User");
        public static IMongoCollection<Order> allbookings = db.GetCollection<Order>("Order");
        public static IMongoCollection<MeetingRoom> allrooms = db.GetCollection<MeetingRoom>("MeetingRoom");
        public static IMongoCollection<FoodCombo> allfoods = db.GetCollection<FoodCombo>("FoodCombo");
        public static IMongoCollection<Admin> alladmins = db.GetCollection<Admin>("Admin");
        public static IMongoCollection<Bookings> allorders = db.GetCollection<Bookings>("Bookings");


        public static bool authenticateadmin(string username, string password)
        {
            var builder = Builders<Admin>.Filter;
            var filter = builder.Eq(x => x.adminid, username) & builder.Eq(x => x.password, password);
            Admin obj1 = alladmins.Find(filter).FirstOrDefault();
            if (obj1 == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool UserExists(string userid)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.userid, userid);
            User user = allusers.Find(filter).FirstOrDefault();
            if (user == null)
                return false;
            else
                return true;
        }


        public static void AddUser(User p)
        {
            allusers.InsertOne(p);
        }

        public static bool AuthenticateUser(string userid, string password)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.userid, userid) & builder.Eq(x => x.password, password);
            User obj1 = allusers.Find(filter).FirstOrDefault();
            if (obj1 == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool updatepassword(string user,string oldpass, string newpass)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.userid, user);
            User obj1 = allusers.Find(filter).FirstOrDefault();
            if (obj1.password != oldpass)
            {
                return false;
            }
            else
            {
                allusers.UpdateOne(x => x.userid == user,
                   Builders<User>.Update.Set(x => x.password, newpass));

                return true;
            }
        }
        public static bool updatepasswordadmin(string user, string oldpass, string newpass)
        {
            var builder = Builders<Admin>.Filter;
            var filter = builder.Eq(x => x.adminid, user);
            Admin obj1 = alladmins.Find(filter).FirstOrDefault();
            if (obj1.password != oldpass)
            {
                return false;
            }
            else
            {
                alladmins.UpdateOne(x => x.adminid == user,
                   Builders<Admin>.Update.Set(x => x.password, newpass));

                return true;
            }
        }


        public static void AddRoom(MeetingRoom p)
        {
            allrooms.InsertOne(p);
        }

        public static void AddFoodCombo(FoodCombo p)
        {
            allfoods.InsertOne(p);
        }

        public static List<MeetingRoom> GetRooms()
        {
            return allrooms.Find(x => true).ToList();

        }

        public static List<FoodCombo> GetFoods()
        {
            return allfoods.Find(x => true).ToList();

        }

        public static List<Order> GetBookings()
        {
            return allbookings.Find(x => true).SortBy(x => x.bookingid).ToList();

        }

        public static List<Bookings> GetExistingBookings()
        {
            return allorders.Find(x => true).SortBy(x => x.bookingid).ToList();

        }

        public static List<FoodCombo> Getallfoodcombos()
        {
            return allfoods.Find(x => true).SortBy(x => x.foodid).ToList();

        }
        public static void DeleteRoom(string roomid)
        {
            allrooms.DeleteOne(x => x.roomid == roomid);

        }

        public static void DeleteFood(string foodid)
        {
            allfoods.DeleteOne(x => x.foodid == foodid);

        }

        public static void deletebookings()
        {
            allbookings.DeleteMany(x=>true);
        }


        public static bool RoomExist(string name)
        {
            var builder = Builders<MeetingRoom>.Filter;
            var filter = builder.Eq(x => x.name, name);
            MeetingRoom r = allrooms.Find(filter).FirstOrDefault();
            if (r == null)
                return false;
            else
                return true;
        }

        public static void UpdateRoom(MeetingRoom m, string roomid)
        {
            var builder = Builders<MeetingRoom>.Filter;
            var filter = builder.Eq(x => x.roomid, roomid);
            MeetingRoom obj1 = allrooms.Find(filter).FirstOrDefault();


            allrooms.UpdateOne(x => x.roomid == roomid,
               Builders<MeetingRoom>.Update.Set(x => x.venue, m.venue));
            allrooms.UpdateOne(x => x.roomid == roomid,
                Builders<MeetingRoom>.Update.Set(x => x.facilities, m.facilities));
            allrooms.UpdateOne(x => x.roomid == roomid,
                Builders<MeetingRoom>.Update.Set(x => x.costperday, m.costperday));
            allrooms.UpdateOne(x => x.roomid == roomid,
                Builders<MeetingRoom>.Update.Set(x => x.capacity, m.capacity));
        }


        public static void EditFoodCombo(string foodid,FoodCombo m)
        {
            var builder = Builders<FoodCombo>.Filter;
            var filter = builder.Eq(x => x.foodid, foodid);
            FoodCombo obj1 = allfoods.Find(filter).FirstOrDefault();

            allfoods.UpdateOne(x => x.foodid == foodid,
             Builders<FoodCombo>.Update.Set(x => x.name, m.name));
            allfoods.UpdateOne(x => x.foodid == foodid,
               Builders<FoodCombo>.Update.Set(x => x.food1, m.food1));
            allfoods.UpdateOne(x => x.foodid == foodid,
                Builders<FoodCombo>.Update.Set(x => x.food2, m.food2));
            allfoods.UpdateOne(x => x.foodid == foodid,
                Builders<FoodCombo>.Update.Set(x => x.food3, m.food3));
            allfoods.UpdateOne(x => x.foodid == foodid,
                Builders<FoodCombo>.Update.Set(x => x.costperperson, m.costperperson));
        }


        public static bool IsoktoBook(string roomid, DateTime startdate, DateTime enddate)
        {

            var builder = Builders<Bookings>.Filter;
            var filter = builder.Eq(x => x.roomid, roomid);
            List<Bookings> lists = allorders.Find(filter).SortBy(x => x.roomid).ToList();

            foreach (Bookings res in lists)
            {
                DateTime sdate = DateTime.Parse(res.starttime);
                DateTime edate = DateTime.Parse(res.endtime);
                if ((startdate > sdate && startdate < edate) || (enddate > sdate && enddate < edate))
                {
                    return false;
                }
                else if((startdate <= sdate && startdate < edate) && (enddate > sdate && enddate >= edate))
                {
                    return false;
                }
                
            }


            return true;
        }

        public static string BookRoom(string roomid, string userid, DateTime startdate, DateTime enddate,int people,string foodneeded)
        {
            
                MeetingRoom p = GetRoom(roomid);
                Order obj = new Order();
                Guid guid = Guid.NewGuid();
                string str = guid.ToString();
                obj.bookingid = str;
                obj.roomname = p.name;
                obj.userid = (userid);
                obj.roomid = (roomid);
                obj.starttime = startdate.ToString();
                obj.endtime = enddate.ToString();
                obj.totalseats = people;
                obj.isfoodneeded = foodneeded.ToUpper();
                obj.status = "Booked";
               obj.foodcombo = new List<FoodCombo>();
            int days = 0;
            if (enddate.Date ==startdate.Date)
            {
                days = 1;
            }
             else
                days=(enddate.Date - startdate.Date).Days;
                obj.totalcost = ((days)* p.costperday)*obj.totalseats;
                allbookings.InsertOne(obj);

            return obj.bookingid;
        }

        public static MeetingRoom GetRoom(string roomid)
        {
            var builder = Builders<MeetingRoom>.Filter;
            var filter = builder.Eq(x => x.roomid, roomid);
            MeetingRoom ap = allrooms.Find(filter).FirstOrDefault();
            return ap;
        }


        public static FoodCombo GetFood(string foodid)
        {
            var builder = Builders<FoodCombo>.Filter;
            var filter = builder.Eq(x => x.foodid, foodid);
            FoodCombo ap = allfoods.Find(filter).FirstOrDefault();
            return ap;
        }

        public static Order Getbookingbyid(string bookingid)
        {
            var builder = Builders<Order>.Filter;
            var filter = builder.Eq(x => x.bookingid, bookingid);
            Order ap = allbookings.Find(filter).FirstOrDefault();
            return ap;
        }

        public static Bookings Getbookingforpayment(string bookingid)
        {
            var builder = Builders<Bookings>.Filter;
            var filter = builder.Eq(x => x.bookingid, bookingid);
            Bookings ap = allorders.Find(filter).FirstOrDefault();
            return ap;
        }

        public static List<Order> GetBookings(string userid)
        {
            var builder = Builders<Order>.Filter;
            var filter = builder.Eq(x => x.userid, userid);
            List<Order> lists = allbookings.Find(filter).SortBy(x => x.userid).ToList();
            foreach(Order b in lists)
            {
                if(b.foodcombo.Count==0)
                {
                    allbookings.UpdateOne(x => x.bookingid == b.bookingid,
                                 Builders<Order>.Update.Set(x => x.isfoodneeded, "NO"));
                }
            }
            return lists;
        }

         public static List<Bookings> GetFinalOrder(string userid)
        {
            var builder = Builders<Bookings>.Filter;
            var filter = builder.Eq(x => x.userid, userid);
            List<Bookings> lists = allorders.Find(filter).SortBy(x => x.userid).ToList();
            foreach(Bookings b in lists)
            {
                if(b.foodcombo.Count==0)
                {
                    allorders.UpdateOne(x => x.bookingid == b.bookingid,
                                 Builders<Bookings>.Update.Set(x => x.isfoodneeded, "NO"));
                }
            }
            return lists;
        }
       

        public static void DeleteBooking(string id)
        {
            Bookings m = Getbookingforpayment(id);
            var builder = Builders<Bookings>.Filter;
            var filter = builder.Eq(x => x.bookingid, id);
            List<Bookings> lists = allorders.Find(x => true).SortBy(x => x.bookingid).ToList();

            foreach (Bookings r in lists)
            {
                if (r != null)
                {
                  
                    if (DateTime.Parse(r.endtime) >= DateTime.Now.Date)
                    {
                        if (lists.Count == 1)
                        {
                            Bookings mn = Getbookingforpayment(r.bookingid);
                            allrooms.UpdateOne(x => x.roomid == mn.roomid,
                                 Builders<MeetingRoom>.Update.Set(x => x.status, "NotBooked"));
                        }

                    }
                    else
                    {
                        Bookings mx = Getbookingforpayment(r.bookingid);
                        allrooms.UpdateOne(x => x.roomid == mx.roomid,
                             Builders<MeetingRoom>.Update.Set(x => x.status, "Booked"));
                    }
                }
            }
            allorders.DeleteOne(x => x.bookingid == id);


           
        }

        public static void AddFood(string foodid,string roomid,string bookingid,int qty)
        {
            FoodCombo f = GetFood(foodid);
            
            var builder = Builders<MeetingRoom>.Filter;
            var filter = builder.Eq(x => x.roomid, roomid);
            MeetingRoom r = allrooms.Find(filter).FirstOrDefault();
            Order bk = Getbookingbyid(bookingid);
            int duration = 0;
            if((DateTime.Parse(bk.endtime).Date== (DateTime.Parse(bk.starttime).Date)))
            {
                duration = 1;
            }
            else
             duration=(DateTime.Parse(bk.endtime)-DateTime.Parse(bk.starttime)).Days;


            List<FoodCombo> fd = new List<FoodCombo>();
            if (qty > 0)
            {
                f.quantity = qty;
                List<FoodCombo> food = bk.foodcombo.ToList();
                if (food.Count>0)
                {
                    foreach (FoodCombo ff in food)
                    {
                        if (foodid == ff.foodid)
                        {
                            int newqty = ff.quantity + qty;
                            List<FoodCombo> nfood = bk.foodcombo.ToList();
                            FoodCombo nf= nfood.Find(x => x.foodid == foodid);
                            nf.quantity = newqty;
                            nfood.Remove(ff);
                            nfood.Add(nf);

                            allbookings.UpdateOne(x => x.bookingid == bookingid,
                          Builders<Order>.Update.Set(x => x.foodcombo, nfood));
                            goto here;
                        }
                        
                    }
                    fd = bk.foodcombo;
                    fd.Add(f);
                    allbookings.UpdateOne(x => x.bookingid == bookingid,
                     Builders<Order>.Update.Set(x => x.foodcombo, fd));
                }
                else
                {
                    fd = bk.foodcombo;
                    fd.Add(f);
                    allbookings.UpdateOne(x => x.bookingid == bookingid,
                     Builders<Order>.Update.Set(x => x.foodcombo, fd));

                }
                
            }
           
            here:
            double cost = bk.totalcost + (f.quantity * f.costperperson * duration);
            
            allbookings.UpdateOne(x => x.bookingid == bookingid,
                Builders<Order>.Update.Set(x => x.totalcost, cost));



        }


        public static void AddPayment(string orderid,double amount)
        {
            Order m = Getbookingbyid(orderid);
            Bookings f = new Bookings();
            f.bookingid = m.bookingid;
            f.userid = m.userid;
            f.roomid = m.roomid;
            f.roomname = m.roomname;
            f.starttime = m.starttime;
            f.endtime = m.endtime;
            f.isfoodneeded = m.isfoodneeded;
            f.status = m.status;
            f.totalseats = m.totalseats;
            f.totalcost = m.totalcost;
            f.advamount = amount;
            f.remainamount = f.totalcost - f.advamount;
            f.foodcombo = m.foodcombo;

            allorders.InsertOne(f);



        }

        public static void AddRemainingPayment(string orderid, double amount)
        {
            Bookings f = Getbookingforpayment(orderid);
            f.advamount = f.advamount + amount;
            allorders.UpdateOne(x => x.bookingid == orderid,
               Builders<Bookings>.Update.Set(x => x.remainamount, amount));
            allorders.UpdateOne(x => x.bookingid == orderid,
               Builders<Bookings>.Update.Set(x => x.advamount, f.advamount));
            allorders.UpdateOne(x => x.bookingid == orderid,
              Builders<Bookings>.Update.Set(x => x.remainamount, 0));


        }
    }
}