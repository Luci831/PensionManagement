using Microsoft.AspNetCore.Mvc;
using PensionCalculationMicroservice.Models;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PensionCalculationMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {

        // POST api/<CalculationController>
        [HttpPost]
        public decimal Post(PensionerDetails pd)
        {
            decimal PensionAmount;
            if (pd.Ptid == 1)
                PensionAmount = (pd.SalaryEarned) * 80 / 100 + pd.Allowances;
            else
                PensionAmount = (pd.SalaryEarned) * 50 / 100 + pd.Allowances;
            if (InMemoryRepo.banks.SingleOrDefault(x => x.BankId == pd.BankId).BType == 1)
                PensionAmount += 500;
            else
                PensionAmount += 550;
            return PensionAmount;
        }
    }
}
