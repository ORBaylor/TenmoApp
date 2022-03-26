using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account
    {
        //Set constraints
        public int accountId { get; set; }
        public int userId { get; set; }
        public decimal balance { get; set; }


        //Get all tranfers
        public Account(int accountId, int userId, decimal balance)
        {
            this.accountId = accountId;
            this.userId = userId;
            this.balance = balance;
        }
        public Account()
        {

        }
    }
}
