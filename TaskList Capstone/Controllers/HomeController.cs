using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TaskList_Capstone.Models;
namespace TaskList_Capstone.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Error = "";
            return View();
        }

        public ActionResult About()
        {
            //1. create a ORM
            TaskListCapstoneEntities ORM = new TaskListCapstoneEntities();

            ViewBag.TaskList = ORM.Tasks.ToList();

            return View();
            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Registration()//This is an action for registration
        {
            return View();
        }

        public ActionResult RegisterNewUser(User newUser)//This is an action for reister new user
        {
            //1.create ORM
            TaskListCapstoneEntities ORM = new TaskListCapstoneEntities();

            //2. Add a user and save changes
            ORM.Users.Add(newUser);

            ORM.SaveChanges();

            return View("Index");//you will form a view for index
        }

        public ActionResult SignIn(string UserName, string Password)//This is an action for sign in        
        {
            //1.create an ORM
            TaskListCapstoneEntities ORM = new TaskListCapstoneEntities();

            //2. Find the user and sign them in
            User currentUser = ORM.Users.Find(UserName);

            if (currentUser == null)
            {
                ViewBag.Error = "Username does not exist. Did you mean to register?";
                return View("Index");
            }
            else if (currentUser.Password != Password)
            {
                ViewBag.Error = "Incorrect password.";
                return View("Index");
            }

            ViewBag.Message = $"Welcome {UserName}!";
            return View("Welcome");//You will make a view for a welcome
        }

        public ActionResult Addtask()
        {
            return View();
        }

        public ActionResult Createtask(Task newtask)
        {
            //1. create the ORM 
           TaskListCapstoneEntities ORM = new TaskListCapstoneEntities();

            if (ModelState.IsValid)
            {


                //2.Insert new task into the database
                if (ORM.Tasks.ToList().Count() == 0)
                {
                    newtask.TaskNumber = "1";
                }
                else
                {
                    newtask.TaskNumber = (int.Parse(ORM.Tasks.ToList().Last().TaskNumber) + 1).ToString();
                }


                ORM.Tasks.Add(newtask);

                //3.save changes to the DB
                ORM.SaveChanges();

                ViewBag.Tasklist = ORM.Tasks.ToList();

                ViewBag.Message = $"Hello, Thank you for registering {newtask.UserName}";
                return View("result");

            }
            else
            {
                ViewBag.Address = Request.UserHostAddress;
                return View("Error");


            }
        }

        public ActionResult CheckTask(string TaskNumber)
        {
            //1. create ORM
            TaskListCapstoneEntities ORM = new TaskListCapstoneEntities();

            //2. Find the task you want to check
            Task Found = ORM.Tasks.Find(TaskNumber);

            //3.Check the task
            if (Found != null)
            {
                if (Found.Status == "Incomplete")//comparison
                {
                    Found.Status = "Completed";//assignment
                }
                else
                {
                    Found.Status = "Incomplete";
                }


                ORM.Entry(Found).State = System.Data.Entity.EntityState.Modified;//need this everytime your modifying like editing, checking or deleting

                //4. save to the DB
                ORM.SaveChanges();

                return RedirectToAction("About");

            }
            else
            {
                ViewBag.Error.Message = "Task not found";
                return View("Error");
            }

        }

        public ActionResult DeleteTask(Task TaskNumber)
        {
            {
                //1. create ORM
                TaskListCapstoneEntities ORM = new TaskListCapstoneEntities();

                //2. Find the task you want to delete
                Task Found = ORM.Tasks.Remove(TaskNumber);

                //3. Remove the task
                if (Found != null)
                {
                    ORM.Tasks.Remove(Found);

                    //4. save to the DB
                    ORM.SaveChanges();

                    return RedirectToAction("About");

                }
                else
                {
                    ViewBag.Error.Message = "Task not found";
                    return View("Error");
                }

            }
        }


    }

}
    

