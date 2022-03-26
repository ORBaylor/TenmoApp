using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using TenmoServer.DAO;
using Microsoft.AspNetCore.Authorization;


namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDao transferDao;



        public TransferController(ITransferDao transferDao)
        {
            this.transferDao = transferDao;
        }


        [HttpGet()]
        public  List<Transfer> ShowAllTranfers()
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            List<Transfer> transfers = (List<Transfer>)transferDao.AllTransferList();

            return transfers;
        }

        
        [HttpGet("{transferId}")]
        public Transfer GetTransferDetails(int transferId)
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            Transfer transfer = transferDao.GetTransferDetails(transferId);
            return transfer;
        }

        //Not working************************************
        //Throws error
        //Enter the line below into postman
        //https://localhost:44315/Transfer/All/2005 
        [HttpGet("All/{accountId}")]
        public List<Transfer> AllTransferFromAccount(int accountId)
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            // Might need to change this if it does not give me back a list
            List<Transfer> allTransfers = transferDao.GetListOfTransfersFromAccount(accountId);
            return allTransfers;

        }


        [HttpGet("Get/{accountId}")]
        public List<Transfer> AllTransferDetailsFromAccount(int accountId)
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            List<Transfer> allTransfers = transferDao.GetListOfCompletedTransfersFromAccount(accountId);
            return allTransfers;

        }


        [HttpGet("whoami")]

        public ActionResult WhoAmI()
        {
            //string authUser = User.Identity.Name;
            //string authUserId = User.FindFirst("sub")?.Value;

            string userName = User.FindFirst("name")?.Value;
            User user = new User();
            return Ok($"{userName}");
        }

        // public IList<>

        //[HttpGet("transfers/{transferId}")]
        //public Transfer ShowATransfer(int transferId)
        //{
        //    Transfer transfer = accountDao.GetTransferById(transferId);
        //    return transfer;
        //}

    }
}
