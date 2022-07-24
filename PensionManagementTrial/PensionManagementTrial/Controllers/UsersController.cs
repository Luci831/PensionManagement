using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PensionManagementTrial.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PensionManagementTrial.Controllers
{
    public class UsersController : Controller
    {
        public static async Task<List<User>> GetList()
        {
            List<User> users = new List<User>();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:46612/api/Users");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<User>>(results);
            }
            return users.ToList();
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<User> users = new List<User>();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:46612/api/Users");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<User>>(results);
            }
            foreach(var user in users)
            {
                user.Category = InMemoryRepo.categories.SingleOrDefault(p => p.CatId == user.CatId);
                user.SecurityQuestion = InMemoryRepo.securityQuestions.SingleOrDefault(p => p.Sqid == user.SqId);
            }
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = new User();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(results);
                user.Category = InMemoryRepo.categories.SingleOrDefault(p => p.CatId == user.CatId);
                user.SecurityQuestion = InMemoryRepo.securityQuestions.SingleOrDefault(p => p.Sqid == user.SqId);
                return View(user);
            }
            return NotFound();
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["CatId"] = new SelectList(InMemoryRepo.categories, "CatId", "CategoryType");
            ViewData["SqId"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Dob,ContactNumber,Email,Password,CatId,SqId,Answer")] User user)
        {
            if (ModelState.IsValid)
            {
                List<User> users = new List<User>();
                users =await GetList();
                if (users.Any(x => x.Email == user.Email))
                {
                    ViewBag.Message = "User with this email-id already exists";
                    ViewData["Sqid"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions", user.SqId);
                    ViewData["CatId"] = new SelectList(InMemoryRepo.categories.Where(c => c.CatId != 1), "CatId", "CategoryType", user.CatId);
                    return View(user);
                }
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    user.Picture = p1;
                }
                HttpClient client = new HttpClient();
                var response = await client.PostAsJsonAsync("http://localhost:46612/api/Users", user);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }
            ViewData["CatId"] = new SelectList(InMemoryRepo.categories, "CatId", "CategoryType", user.CatId);
            ViewData["SqId"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions", user.SqId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = new User();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(results);
                ViewData["CatId"] = new SelectList(InMemoryRepo.categories, "CatId", "CategoryType", user.CatId);
                ViewData["SqId"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions", user.SqId);
                return View(user);
            }

            return NotFound();
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Dob,ContactNumber,Email,Password,CatId,SqId,Answer")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                List<User> users = new List<User>();
                users = await GetList();
                if (users.Any(x => x.Email == user.Email && x.UserId != user.UserId))
                {
                    ViewBag.Message = "User with this email-id already exists";
                    ViewData["Sqid"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions", user.SqId);
                    ViewData["CatId"] = new SelectList(InMemoryRepo.categories, "CatId", "CategoryType", user.CatId);
                    return View(user);
                }
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    user.Picture = p1;
                }
                else
                {
                    HttpClient clnt = new HttpClient();
                    clnt.DefaultRequestHeaders.Clear();
                    clnt.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    HttpResponseMessage rsponse = await clnt.GetAsync($"http://localhost:46612/api/Users/{id}");
                    var results = rsponse.Content.ReadAsStringAsync().Result;
                    var objFromDb = JsonConvert.DeserializeObject<User>(results); 
                    user.Picture = objFromDb.Picture;
                }
                HttpClient client= new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                var response = await client.PutAsJsonAsync($"http://localhost:46612/api/Users/{id}", user);
                if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatId"] = new SelectList(InMemoryRepo.categories, "CatId", "CategoryType", user.CatId);
            ViewData["SqId"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions", user.SqId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = new User();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(results);
                user.Category =  InMemoryRepo.categories.SingleOrDefault(p => p.CatId == user.CatId);
                user.SecurityQuestion =  InMemoryRepo.securityQuestions.SingleOrDefault(p => p.Sqid == user.SqId);
                return View(user);
            }
            return NotFound();
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var response = await client.DeleteAsync($"http://localhost:46612/api/Users/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View();
        }
    }
}
