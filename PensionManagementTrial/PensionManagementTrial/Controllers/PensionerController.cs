using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PensionManagementTrial.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.IO;

namespace PensionManagementTrial.Controllers
{
    public class PensionerController : Controller
    {
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction(nameof(Index), "Login");
            }
            User user = new User();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            string token = HttpContext.Session.GetString("token");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/Users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(results);
                HttpContext.Session.SetString("username", user.FirstName);
                return View(user);
            }

            return RedirectToAction(nameof(Index), "Login");

        }
        public async Task<IActionResult> EditProfile(int? id)
        {
            User user = new User();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/Users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(results);
                ViewData["Sqid"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions");
                return View(user);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(int id,User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

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
                if (users.Any(x => x.Email == user.Email && x.UserId!=user.UserId))
                {
                    ViewBag.Message = "User with this email-id already exists";
                    ViewData["Sqid"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions", user.SqId);
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
                client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                response = await client.PutAsJsonAsync($"http://localhost:46612/api/Users/{id}", user);
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("username", user.FirstName);
                    return RedirectToAction("Index");
                }
            }

            ViewData["Sqid"] = new SelectList(InMemoryRepo.securityQuestions, "Sqid", "Questions");
            
            return View();

        }
        public async Task<IActionResult> Manage()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            List<PensionerDetails> loginlist = new List<PensionerDetails>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/PensionerDetails/GetPensionerDetailsByUser/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                loginlist = JsonConvert.DeserializeObject<List<PensionerDetails>>(results);
            }
            foreach(var pension in loginlist)
            {
               pension.Banks = InMemoryRepo.banks.SingleOrDefault(p => p.BankId == pension.BankId);
                pension.PensionTypes = InMemoryRepo.pensionTypes.SingleOrDefault(p => p.Ptid == pension.Ptid);
            }
            //var queryable = loginlist.All(p=>p.Banks=_context.Banks.Sin)
            //var appDBContext = queryable.Include(p => p.Banks).Include(p => p.PensionTypes).Include(p => p.User);
            return View(loginlist.ToList());
        }
        public static async Task<decimal> Calculate(PensionerDetails pd)
        {
            decimal PensionAmount=0;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync($"https://localhost:44390/api/Calculation",pd);
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                PensionAmount = JsonConvert.DeserializeObject<decimal>(results);
            }
            return PensionAmount;

        }

        // GET: PensionerDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
             if (id == null)
            {
                return NotFound();
            }

            PensionerDetails pensionerDetails = new PensionerDetails();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/PensionerDetails/GetPensionerDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                pensionerDetails = JsonConvert.DeserializeObject<PensionerDetails>(results);
                pensionerDetails.Banks = InMemoryRepo.banks.SingleOrDefault(p => p.BankId == pensionerDetails.BankId);
                pensionerDetails.PensionTypes = InMemoryRepo.pensionTypes.SingleOrDefault(p => p.Ptid == pensionerDetails.Ptid);
                return View(pensionerDetails);
            }
            return NotFound();
        }

        // GET: PensionerDetails/Create
        public IActionResult Create()
        {
            ViewData["BankId"] = new SelectList(InMemoryRepo.banks, "BankId", "BankName");
            ViewData["Ptid"] = new SelectList(InMemoryRepo.pensionTypes, "Ptid", "PensionerType");
          
            return View();
        }

        // POST: PensionerDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Prid,Name,Dob,Pan,AadharNumber,AccountNo,SalaryEarned,Allowances,Ptid,BankId")] PensionerDetails pensionerDetails)
        {
            pensionerDetails.UserId = HttpContext.Session.GetInt32("UserID");
            ModelState.Remove("UserId");
            ModelState.Remove("PensionAmount");
            if (ModelState.IsValid)
            {
                List<PensionerDetails> loginlist = new List<PensionerDetails>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/PensionerDetails/GetPensionerDetails");
                if (response.IsSuccessStatusCode)
                {
                    var results = response.Content.ReadAsStringAsync().Result;
                    loginlist = JsonConvert.DeserializeObject<List<PensionerDetails>>(results);
                }
                if (loginlist.Any(x => x.AadharNumber == pensionerDetails.AadharNumber || x.Pan == pensionerDetails.Pan))
                {
                    ViewBag.Message = "Pension with this Adhaar Number or PAN Number Alreay Exists";
                    ViewData["BankId"] = new SelectList(InMemoryRepo.banks, "BankId", "BankName", pensionerDetails.BankId);
                    ViewData["Ptid"] = new SelectList(InMemoryRepo.pensionTypes, "Ptid", "PensionerType", pensionerDetails.Ptid);
                    return View(pensionerDetails);
                }
                else
                {
                    pensionerDetails.PensionAmount = await Calculate(pensionerDetails);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    response = await client.PostAsJsonAsync("http://localhost:46612/api/PensionerDetails/PostPensionerDetails", pensionerDetails);
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("Manage");
                }
            }
            ViewData["BankId"] = new SelectList(InMemoryRepo.banks, "BankId", "BankName", pensionerDetails.BankId);
            ViewData["Ptid"] = new SelectList(InMemoryRepo.pensionTypes, "Ptid", "PensionerType", pensionerDetails.Ptid);

            return View(pensionerDetails);
        }

        // GET: PensionerDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PensionerDetails pensionerDetails = new PensionerDetails();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/PensionerDetails/GetPensionerDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                pensionerDetails = JsonConvert.DeserializeObject<PensionerDetails>(results);
                ViewData["BankId"] = new SelectList(InMemoryRepo.banks, "BankId", "BankName", pensionerDetails.BankId);
                ViewData["Ptid"] = new SelectList(InMemoryRepo.pensionTypes, "Ptid", "PensionerType", pensionerDetails.Ptid);
                return View(pensionerDetails);
            }
            return NotFound();
        }

        // POST: PensionerDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Prid,UserId,Name,Dob,Pan,AadharNumber,AccountNo,SalaryEarned,Allowances,Ptid,BankId, PensionAmount")] PensionerDetails pensionerDetails)
        {
            if (id != pensionerDetails.Prid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                List<PensionerDetails> loginlist = new List<PensionerDetails>();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/PensionerDetails/GetPensionerDetails");
                if (response.IsSuccessStatusCode)
                {
                    var results = response.Content.ReadAsStringAsync().Result;
                    loginlist = JsonConvert.DeserializeObject<List<PensionerDetails>>(results);
                }

                if (loginlist.Any(x => x.Prid != pensionerDetails.Prid && (x.AadharNumber == pensionerDetails.AadharNumber || x.Pan == pensionerDetails.Pan)))
                {
                    ViewBag.Message = "Pensioner with this Adhaar Number or PAN Number Alreay Exists";
                    ViewData["BankId"] = new SelectList(InMemoryRepo.banks, "BankId", "BankName", pensionerDetails.BankId);
                    ViewData["Ptid"] = new SelectList(InMemoryRepo.pensionTypes, "Ptid", "PensionerType", pensionerDetails.Ptid);
                    return View(pensionerDetails);
                }
                else
                {
                    pensionerDetails.PensionAmount = await Calculate(pensionerDetails);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
                    response = await client.PutAsJsonAsync($"http://localhost:46612/api/PensionerDetails/PutPensionerDetails/{id}", pensionerDetails);
                    if (response.IsSuccessStatusCode)
                        return RedirectToAction("Manage");
                }
            }
            ViewData["BankId"] = new SelectList(InMemoryRepo.banks, "BankId", "BankName", pensionerDetails.BankId);
            ViewData["Ptid"] = new SelectList(InMemoryRepo.pensionTypes, "Ptid", "PensionerType", pensionerDetails.Ptid);
            return View(pensionerDetails);
        }

        // GET: PensionerDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PensionerDetails pensionerDetails = new PensionerDetails();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync($"http://localhost:46612/api/PensionerDetails/GetPensionerDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                pensionerDetails = JsonConvert.DeserializeObject<PensionerDetails>(results);
                pensionerDetails.Banks = InMemoryRepo.banks.SingleOrDefault(p => p.BankId == pensionerDetails.BankId);
                pensionerDetails.PensionTypes = InMemoryRepo.pensionTypes.SingleOrDefault(p => p.Ptid == pensionerDetails.Ptid);
                return View(pensionerDetails);
            }
            return NotFound();
        }

        // POST: PensionerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            var response = await client.DeleteAsync($"http://localhost:46612/api/PensionerDetails/DeletePensionerDetails/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Manage");
            return View();
        }
    }
}
