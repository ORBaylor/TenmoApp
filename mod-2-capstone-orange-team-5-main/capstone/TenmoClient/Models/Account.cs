using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Account
    {
        //Set constraints
        public int accountId { get; set; }
        public int userId { get; set; }
        public decimal balance { get; set; }

        public string username { get; set; }

        //Get all tranfers
        public Account(int accountId, int userId, decimal balance, string username)
        {
            this.accountId = accountId;
            this.userId = userId;
            this.balance = balance;
            this.username = username;
        }
        public Account()
        {

        }
    }
}
