using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PensionManagementTrial.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.IO;

namespace PensionManagementTrial.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Create()
        {
            ViewData["Sqid"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions");
            ViewData["CatId"] = new SelectList(InMemoryRepo.categories.Where(c => c.CatId != 1), "CatId", "CategoryType");
            return View();

        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Dob,ContactNumber,Email,Password,CatId,SqId,Answer")] User user)
        {

            if (ModelState.IsValid)
            {
                List<User> users = new List<User>();
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:46612/api/Users");
                if (response.IsSuccessStatusCode)
                {
                    var results = response.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<User>>(results);
                }
                if (users.Any(x => x.Email == user.Email))
                {
                    ViewBag.Message = "User with this email-id already exists";
                    ViewData["Sqid"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions", user.SqId);
                    return View(user);
                }
                else
                {
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
                    response = await client.PostAsJsonAsync("http://localhost:46612/api/Users", user);
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction(nameof(Index));
                }
            }

            ViewData["Sqid"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions", user.SqId);

            return View(user);
        }
        public ActionResult Index()
        {
            ViewData["CatId"] = new SelectList(InMemoryRepo.categories, "CatId", "CategoryType");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index (User user)
        {
            Token t = new Token();
            HttpClient client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://localhost:44373/api/Token",user);
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                t = JsonConvert.DeserializeObject<Token>(results);
            }
                if (t.token == null)
                {
                ViewData["CatId"] = new SelectList(InMemoryRepo.categories, "CatId", "CategoryType");
                ViewBag.Message = "Invalid Email or Password";
                    return View();

                }
                else
                {
                    HttpContext.Session.SetInt32("UserID", t.Id);
                    HttpContext.Session.SetString("token", t.token);

                    if (user.CatId == 1)
                    {
                        return RedirectToAction(nameof(Index), "PensionerDetails");
                    }
                    else if (user.CatId == 2)
                    {
                        return RedirectToAction(nameof(Index), "Pensioner");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }

                }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}