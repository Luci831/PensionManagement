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

namespace PensionManagementTrial.Controllers
{
    public class PensionerDetailsController : Controller
    {
        

        // GET: PensionerDetails
        public async Task<IActionResult> Index()
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
            foreach (var pension in loginlist)
            {
                pension.Banks = InMemoryRepo.banks.SingleOrDefault(p => p.BankId == pension.BankId);
                pension.PensionTypes = InMemoryRepo.pensionTypes.SingleOrDefault(p => p.Ptid == pension.Ptid);
            }
            // var appDBContext = _context.PensionerDetails.Include(p => p.Banks).Include(p => p.PensionTypes).Include(p => p.User).Where(p =>p.UserId == userId);

            return View(loginlist.ToList());
        }
        public static async Task<decimal> Calculate(PensionerDetails pd)
        {
            decimal PensionAmount = 0;
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Clear();
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.PostAsJsonAsync($"https://localhost:44390/api/Calculation", pd);
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
        public async Task<IActionResult> Create()
        {
            List<User> u = new List<User>();
            u = await GetList();
            IEnumerable<User> users = u as IEnumerable<User>;
            ViewData["UserId"] = new SelectList(users, "UserId", "Email");
            ViewData["BankId"] = new SelectList(InMemoryRepo.banks, "BankId", "BankName");
            ViewData["Ptid"] = new SelectList(InMemoryRepo.pensionTypes, "Ptid", "PensionerType");

            return View();
        }
        public static  async Task<List<User>> GetList()
        {
            List<User> users = new List<User>();
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Clear();
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("token"));
            HttpResponseMessage response = await client.GetAsync("http://localhost:46612/api/Users");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<User>>(results);
            }
            return users.ToList();
        }

        // POST: PensionerDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Prid,UserId,Name,Dob,Pan,AadharNumber,AccountNo,SalaryEarned,Allowances,Ptid,PensionAmount,BankId")] PensionerDetails pensionerDetails)
        {
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

                if (loginlist.Any(x =>x.AadharNumber == pensionerDetails.AadharNumber || x.Pan == pensionerDetails.Pan))
                {
                    ViewBag.Message = "Pensioner with this Adhaar Number or PAN Number Alreay Exists";
                    List<User> us = new List<User>();
                    us = await GetList();
                    IEnumerable<User> userss = us as IEnumerable<User>;
                    ViewData["UserId"] = new SelectList(userss, "UserId", "Email");
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
                        return RedirectToAction("Index");
                }
            }
            List<User> u = new List<User>();
            u = await GetList();
            IEnumerable<User> users = u as IEnumerable<User>;
            ViewData["UserId"] = new SelectList(users, "UserId", "Email");
            ViewData["BankId"] = new SelectList(InMemoryRepo.banks, "BankId", "BankName");
            ViewData["Ptid"] = new SelectList(InMemoryRepo.pensionTypes, "Ptid", "PensionerType");

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
        public async Task<IActionResult> Edit(int id, [Bind("Prid,UserId,Name,Dob,Pan,AadharNumber,AccountNo,SalaryEarned,Allowances,Ptid,BankId,PensionAmount")] PensionerDetails pensionerDetails)
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
                    ViewBag.Message = "Pension with this Adhaar Number or PAN Number Alreay Exists";
                    List<User> us = new List<User>();
                    us = await GetList();
                    IEnumerable<User> userss = us as IEnumerable<User>;
                    ViewData["UserId"] = new SelectList(userss, "UserId", "Email");
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
                        return RedirectToAction("Index");

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
                return RedirectToAction("Index");
            return View();
        }
        // GET: PensionerDetails/Details/5
    }
}
