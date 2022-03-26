using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
   public  interface ITransferDao
    {

        IList<Transfer> AllTransferList();
        Transfer GetTransferDetails(int transferId);
       // decimal GetBalance(int accountId);
        //Transfer CreateNewTransfer(NewTransfer newTransfer);
        //Transfer UpdateTransfer(int transferId, int transferStatusId, int transferTypeId);
        List<Transfer> GetListOfTransfersFromAccount(int accountId);
        List<Transfer> GetListOfCompletedTransfersFromAccount(int accountId);
       // IList<Transfer> GetListOfPendingRequestsFromUser(int account);




    }
}
