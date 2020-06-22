using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsharpBelt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CsharpBelt.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get;set;}
        private PasswordHasher<User> regHasher = new PasswordHasher<User>();
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser>();
        public User GetUser()
        {
            return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
        }
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult _Register()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User u)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.FirstOrDefault(usr => usr.Email == u.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email already exits!");
                    return View("_Register");
                }
                string hash = regHasher.HashPassword(u, u.Password);
                u.Password = hash;
                _context.Users.Add(u);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("userId", u.UserId);
                return Redirect("/dashboard");
            }
            return View("_Register");
        }
        [HttpGet("login")]
        public IActionResult _Login()
        {
            return View();
        }

        [HttpPost("log")]
        public IActionResult Login(LoginUser lu)
        {
            if(ModelState.IsValid)
            {
                User userInDB = _context.Users.FirstOrDefault(u => u.Email == lu.Email);
                if(userInDB == null)
                {
                    ModelState.AddModelError("Email", "Wrong Email or Password!");
                    return View("_Login");
                }
                var result = logHasher.VerifyHashedPassword(lu, userInDB.Password, lu.Password);
                if(result ==0)
                {
                    ModelState.AddModelError("Email", "Wrong Email or Password!");
                    return View("_Login");
                }
                HttpContext.Session.SetInt32("userId", userInDB.UserId);
                return Redirect("/dashboard");
            }
            return View("_Login");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/login");
            }
            ViewBag.User = current;
            List<Models.Plan> AllPlans = _context.Plans
                .Include(p => p.Planner)
                .Include(p => p.Guests)
                .ThenInclude(i => i.User)
                .Where(p => p.Date >= DateTime.Now)
                .OrderBy(p => p.Date)
                .ToList();
            return View(AllPlans);
        }

        [HttpGet("new")]
        public IActionResult NewPlan()
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/login");
            }
            return View();
        }
        
        [HttpPost("add/plan")]
        public IActionResult AddPlan(Models.Plan addPlan)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/login");
            }
            if(ModelState.IsValid)
            {
                addPlan.UserId = current.UserId;
                _context.Plans.Add(addPlan);
                _context.SaveChanges();
                return Redirect("/dashboard");

            }
            return View("NewPlan");
        }

        [HttpGet("delete/plan/{id}")]
        public IActionResult DeletePlan(int id)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/login");
            }
            Plan remove = _context.Plans
                .FirstOrDefault(p => p.PlanId == id);
            _context.Plans.Remove(remove);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }
        [HttpGet("{status}/{id}")]
        public IActionResult JoinOrLeave(int id, string status)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/login");
            }
            if (status == "join")
            {
                Invite newInvite = new Invite();
                newInvite.UserId = current.UserId;
                newInvite.PlanId = id;
                _context.Invites.Add(newInvite);
            }
            else if(status == "leave")
            {
                Invite leave = _context.Invites
                    .FirstOrDefault(i => i.UserId == current.UserId && i.PlanId == id);
                _context.Invites.Remove(leave);
            }
            _context.SaveChanges();
            return Redirect("/dashboard");
        }

        [HttpGet("plan/{id}")]
        public IActionResult PlanInfo(int id)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/login");
            }
            ViewBag.User = current;
            Plan PlanInfo = _context.Plans
                .Include(p => p.Guests)
                .ThenInclude(i => i.User)
                .Include(p => p.Planner)
                .FirstOrDefault(p => p.PlanId == id);
            return View(PlanInfo);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
