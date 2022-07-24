using System;
using System.Collections.Generic;

namespace PensionCalculationMicroservice.Models
{
    public class InMemoryRepo
    {
        public static IReadOnlyList<Banks> banks = new List<Banks>()
        {
             new Banks() { BankId = 1, BType = 1, BankName = "Bank of Baroda" },
            new Banks() { BankId = 2, BType = 1, BankName = "Bank of India" },
            new Banks() { BankId = 3, BType = 1, BankName = "Bank of Maharastra" },
            new Banks() { BankId = 4, BType = 1, BankName = "Canara Bank" },
            new Banks() { BankId = 5, BType = 1, BankName = "Central Bank of India" },
            new Banks() { BankId = 6, BType = 1, BankName = "Indian Bank" },
            new Banks() { BankId = 7, BType = 1, BankName = "Indian Overseas Bank" },
            new Banks() { BankId = 8, BType = 1, BankName = "Punjab National Bank" },
            new Banks() { BankId = 9, BType = 1, BankName = "Punjab&Sind Bank" },
            new Banks() { BankId = 10, BType = 1, BankName = "Union Bank of India" },
            new Banks() { BankId = 11, BType = 1, BankName = "UCO Bank" },
            new Banks() { BankId = 12, BType = 1, BankName = "State Bank of India" },
            new Banks() { BankId = 13, BType = 2, BankName = "HDFC Bank " },
            new Banks() { BankId = 14, BType = 2, BankName = "Axis Bank" },
            new Banks() { BankId = 15, BType = 2, BankName = "ICICI Bank" },
            new Banks() { BankId = 16, BType = 2, BankName = "Kotak Mhindra Bank" },
            new Banks() { BankId = 17, BType = 2, BankName = "YES Bank" },
            new Banks() { BankId = 18, BType = 2, BankName = "Federal Bank" },
            new Banks() { BankId = 19, BType = 2, BankName = "Bandan Bank" },
            new Banks() { BankId = 20, BType = 2, BankName = "City Union Axis" },
            new Banks() { BankId = 21, BType = 2, BankName = "IDBI Bank" },
            new Banks() { BankId = 22, BType = 2, BankName = "CSB Bank" },
            new Banks() { BankId = 23, BType = 2, BankName = "DCB Bank" },
            new Banks() { BankId = 24, BType = 2, BankName = "Dhanlaxmi Bank" },
            new Banks() { BankId = 25, BType = 2, BankName = "IDFC First Bank" },
            new Banks() { BankId = 26, BType = 2, BankName = "Induslnd Bank" },
            new Banks() { BankId = 27, BType = 2, BankName = "RBL Bank" },
            new Banks() { BankId = 28, BType = 2, BankName = "Karnataka Bank" },
            new Banks() { BankId = 29, BType = 2, BankName = "Nainital Bank" },
            new Banks() { BankId = 30, BType = 2, BankName = " South Indian Bank" },
            new Banks() { BankId = 31, BType = 2, BankName = "J&K Bank" },
            new Banks() { BankId = 32, BType = 2, BankName = "Karur Vysya Bank" },
             new Banks() { BankId = 33, BType = 2, BankName = "Tamilnad Mercantile Bank" }
        };
    }
}
