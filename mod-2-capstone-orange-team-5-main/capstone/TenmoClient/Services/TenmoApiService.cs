using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {
        //public readonly string ApiUrl;

        //added new and it got rid of the warning on client
        //private  new  RestClient client = null;
        private readonly string API_URL = "https://localhost:44315/";
        public TenmoApiService(string apiUrl) : base(apiUrl)
        {
            if (client == null)
            {
                client = new RestClient(apiUrl);
            }
        }

        // Add methods to call api here...

        public decimal GetAccountBlance(int userId)
        {
            RestRequest request = new RestRequest($"Account/{userId}");
            IRestResponse<decimal> response = client.Get<decimal>(request);
           // CheckForError(response);

            return response.Data;
        }
        public List<Account> GetAllUserAndNames()
        {
            RestRequest request = new RestRequest("Account/users");
            IRestResponse<List<Account>> response = client.Get<List<Account>>(request);

            return response.Data;
        }
        public string GetUserNameFromAccount(int accountId)
        {
            RestRequest request = new RestRequest($"Account/users/{accountId}");
            IRestResponse<string> response = client.Get<string>(request);

            return response.Data;

        }
        /// <summary>
        /// To get account number 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Account GetAccount(int id)
        {
            RestRequest request = new RestRequest($"Account/GetAll/{id}");
            IRestResponse<Account> response = client.Get<Account>(request);
            return response.Data;
        }


       
        public void TransferMoney(int userId, decimal xferAmount)
        {

            RestRequest request = new RestRequest(API_URL + "account/transfers/" + UserId + "?receivingUserId=" + userId + "&amount=" + xferAmount);
            IRestResponse response = client.Post(request);

        }

        public IList<Transfer> GetTransferFromAccountNumber(int accountId)
        {
            RestRequest request = new RestRequest($"Transfer/All/{accountId}");
            IRestResponse<IList<Transfer>> response = client.Get<IList<Transfer>>(request);
            
            return response.Data;
        }

        public Transfer GetAllDeatilsFromTransfer(int transferId)
        {
            RestRequest request = new RestRequest($"Transfer/{transferId}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            return response.Data;
        }

        //[HttpGet("transfers")]
        ////use ?accountId=int query 
        //public ActionResult<Transfer> ShowUserTransfers(int userId)
        //{
        //    int accountId = accountDao.GetAccountIdFromUserId(userId);
        //    List<Transfer> listOfTransfers = accountDao.GetAllTransfersForAccount(accountId);
        //    return Ok(listOfTransfers);
        //}


        //public Reservation GetReservation(int reservationId)
        //{
        //    RestRequest request = new RestRequest($"reservations/{reservationId}");
        //    IRestResponse<Reservation> response = client.Get<Reservation>(request);

        //    CheckForError(response, $"Get reservation {reservationId}");
        //    return response.Data;
        //}

    }
}
