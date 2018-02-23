using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using beltexam.Models;
using Microsoft.EntityFrameworkCore;

namespace beltexam.Controllers
{
    public class DashboardController : Controller
    {
        private SomeContext _context;
 
        public DashboardController(SomeContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Home")]
        public IActionResult dashboard()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            // List<User> notme = _context.users.Where(n =>n.UserId != HttpContext.Session.GetInt32("id")).ToList();
            List<Models.Activity> AllActivities = _context.activities
                .Include(solo => solo.Coordinator)
                .Include(group => group.Participants)
                .ThenInclude( person => person.User)
                .ToList();
            List<Dictionary<string, object>> ActivityList = new List<Dictionary<string, object>>();
            foreach (Models.Activity single in AllActivities)
            {
                bool creator = false;
                bool follower = false;
                int FollowerCount = 0;
                if (HttpContext.Session.GetInt32("id") == single.UserId)
                {
                    creator = true;
                }
                foreach (var followers in single.Participants)
                {
                    if (followers.UserId == HttpContext.Session.GetInt32("id"))
                    {
                        follower = true;
                    }
                ++FollowerCount;
                }
                Dictionary<string, object> ChangedList = new Dictionary<string, object>();
                ChangedList.Add("ActivityId", single.ActivityId);
                ChangedList.Add("Title", single.Title);
                ChangedList.Add("Time", single.Time);
                ChangedList.Add("Duration", single.Duration);
                ChangedList.Add("Date", single.Date);
                ChangedList.Add("Coordinator", single.Coordinator);
                ChangedList.Add("creator", creator);
                ChangedList.Add("FollowerCount", FollowerCount);
                ChangedList.Add("follower", follower);
                ActivityList.Add(ChangedList);
            }
            ViewBag.ActivityList = ActivityList;
            
            return View();
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "Home");
        }
        [HttpGet]
        [Route("CreateActivity")]
        public IActionResult CreateActivity()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            return View();
        }
        [HttpPost]
        [Route("SubmitActivity")]
        public IActionResult SubmitActivity(ActivityViewModel form)
        {
            if(ModelState.IsValid)
            {
                string duration = form.DurationValue+" "+form.Duration;
                
                Models.Activity thing = new Models.Activity{
                    Title = form.Title,
                    Time = form.Time,
                    Date = form.Date.ToString("M/d"),
                    Duration = duration,
                    Description = form.Description,
                    UserId = (int)HttpContext.Session.GetInt32("id"),
                };
                _context.activities.Add(thing);
                UserActivity newjoin = new UserActivity
                {
                    UserId = (int)HttpContext.Session.GetInt32("id"),
                    ActivityId = thing.ActivityId
                };
                _context.useractivities.Add(newjoin);
                _context.SaveChanges();

                
                HttpContext.Session.SetInt32("activity_id", thing.ActivityId);
                string id = HttpContext.Session.GetInt32("activity_id").ToString();
                return Redirect("activity/"+id);
            }
            return View("CreateActivity", form);
        }

        [HttpGet]
        [Route("activity/{id}")]
        public IActionResult SingleActivity(int? id)
        {
            Models.Activity single = _context.activities.SingleOrDefault(x => x.ActivityId == id);
            ViewBag.ActivityId = single.ActivityId;
            ViewBag.creator = single.UserId;
            ViewBag.Stuff = single.Title;
            ViewBag.Description = single.Description;
            List<Models.Activity> guests = _context.activities.Where(x => x.ActivityId == id).Include(y => y.Participants).ThenInclude(z => z.User).ToList();
            ViewBag.guests = guests;
            ViewBag.id = HttpContext.Session.GetInt32("id");
            return View();
        }
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult DeleteActivity(int? id)
        {
            Models.Activity single = _context.activities.SingleOrDefault(x => x.ActivityId == id);
            if((int)single.UserId == HttpContext.Session.GetInt32("id"))
            {
                _context.activities.Remove(single);
                _context.SaveChanges();
                return RedirectToAction("dashboard");
            }
            ViewBag.name = "CANT DELETE WHAT AINT YOURS";
            return View("dashboard");
        }
        [HttpGet]
        [Route("join/{id}")]
        public IActionResult JoinActivity(int? id)
        {
            //grab activity I want to join
            Models.Activity single = _context.activities.SingleOrDefault(x => x.ActivityId == id);
            //make join table
            UserActivity anotherfollower = new UserActivity
            {
                UserId = (int)HttpContext.Session.GetInt32("id"),
                ActivityId = (int)id
            };
            _context.useractivities.Add(anotherfollower);
            _context.SaveChanges();
            return RedirectToAction("dashboard");

        }
        [HttpGet]
        [Route("leave/{id}")]
        public IActionResult LeaveActivity(int? id)
        {
            Models.Activity single = _context.activities.SingleOrDefault(x => x.ActivityId == id);
            UserActivity anotherfollower = _context.useractivities.SingleOrDefault(my => my.UserId == HttpContext.Session.GetInt32("id")&& my.ActivityId == (int)id);
            _context.useractivities.Remove(anotherfollower);
            _context.SaveChanges();
            return RedirectToAction("dashboard");
        }
    }
}