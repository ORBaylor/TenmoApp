using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
   public interface IAccountSqlDao
    {
        Account GetAccountById(int accountId);

        IList<Account> List();
        List<Transfer> GetAllTransfersForAccount(int accountId);

        Transfer GetTransferById(int transferId);

        Transfer CreateATransfer(Transfer transfer);

        bool UpdateATransferStatus(int transferId, int transferStatusId);

        Account UpdateBalance(int accountId, decimal amount);

        int GetAccountIdFromUserId(int userId);

        string GetUserNameFromAccountId(int accountId);
        Account GetAccountByUserId(int userId);

    }
}
