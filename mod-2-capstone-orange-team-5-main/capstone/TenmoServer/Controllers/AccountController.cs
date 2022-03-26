using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Controllers;
using TenmoServer.Models;
using Microsoft.AspNetCore.Authorization;


namespace TenmoServer.Controllers
{
    // api/
    [Route("[Controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountSqlDao accountDao;
        private readonly IUserDao userDao;


        public AccountController(IAccountSqlDao accountDao, IUserDao userDao)
        {
            this.accountDao = accountDao;
            this.userDao = userDao;
            
        }

        [HttpGet("GetAll/{UserId}")]
        public ActionResult<Account> GetAccount(int userId)
        {
<<<<<<< HEAD
           // string authUser = User.Identity.Name;
=======
            //string authUser = User.Identity.Name;
>>>>>>> e156a257629d22a3bc98d27516f3875768af26b4
            //string authUserId = User.FindFirst("sub")?.Value;

            Account account = accountDao.GetAccountByUserId(userId);
            
            return account;
        }

        [HttpGet()]
        public ActionResult<Account> GetAccount2(int id)
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            Account account = accountDao.GetAccountById(id);

            return account ;
        }


        [HttpGet("{userId}")]
        public decimal GetBalance(int userId)
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            int accountId = accountDao.GetAccountIdFromUserId(userId);
            Account account = accountDao.GetAccountById(accountId);
            decimal balance = account.balance;
            return balance;
        }


        [HttpGet("users")]
        public List<User> GetAllUsersIdsAndNames()
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            List<User> allUsers = userDao.GetUsers();
            List<User> userIdsAndNames = new List<User>();
            foreach (User user in allUsers)
            {
                User newUser = new User();
                newUser.UserId = user.UserId;
                newUser.Username = user.Username;
                userIdsAndNames.Add(newUser);
            }
            return userIdsAndNames;
        }


        [HttpGet("users/{accountId}")]
        public string GetUserNameFromAccountId(int accountId)
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            string userName = accountDao.GetUserNameFromAccountId(accountId);
            return userName;
        }


        [HttpPost("transfers/{sendingUserId}")]
        public bool SendTEBucks(int sendingUserId, int receivingUserId, decimal amount)
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            int accountFrom = accountDao.GetAccountIdFromUserId(sendingUserId);
            int accountTo = accountDao.GetAccountIdFromUserId(receivingUserId);
            bool isTransferSuccessful = false;
            Transfer transfer = new Transfer(2, 2, accountFrom, accountTo, amount);
            Transfer newTransfer = accountDao.CreateATransfer(transfer);

            try
            {
                accountDao.UpdateBalance(newTransfer.accountFrom, -newTransfer.amount); //amt is negative since it's being subtracted
                accountDao.UpdateBalance(newTransfer.accountTo, newTransfer.amount); //amt is positive since it's being added
                accountDao.UpdateATransferStatus(newTransfer.TransferId, 2); //sets status to Approved
                isTransferSuccessful = true;
            }
            catch
            {
                accountDao.UpdateATransferStatus(newTransfer.TransferId, 3); //sets status to Rejected & transfer is not successful
            }

            return isTransferSuccessful;
        }



        // insert into postman
        //https://localhost:44315/account/transfer/1001?receivingUserId=1002&amount=50

        //[HttpGet()]
        //public List<Account> GetAllAccounts()
        //{
        //    Account account = 
        //    return 
        //}

    }
}
