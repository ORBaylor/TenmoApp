using TenmoServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountSqlDao
    {
        private readonly string connectionString;

        public AccountSqlDao(string connString)
        {
            connectionString = connString;
        }

        public Transfer GetTransferById(int transferId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"SELECT transfer_id,transfer_type_id,transfer_status_id,account_from,account_to,amount,1 as Sender,usersfrom.username as name_from,usersto.username as name_to
                                                  FROM transfers
                                                  LEFT JOIN account as fromaccount on account_from = fromaccount.account_id
                                                  LEFT JOIN account as toaccount on account_to = toaccount.account_id
                                                  
                                                  LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
                                                  LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id
                                                  
                                                  WHERE transfer_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", transferId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return GetTransferFromReader(reader);
                }
            }
            return null;
        }
        public IList<Account> List()
        {
            IList<Account> accounts = new List<Account>();



            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM account", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Account account = CreateAccountFromReader(reader);
                        accounts.Add(account);
                    }

                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            return accounts;

        }

        public Account GetAccountByUserId(int userId)
        {
            Account searchAccount = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM account
                                                        where user_id = @user_id", conn);

                    cmd.Parameters.AddWithValue("@user_Id", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        searchAccount = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                //Causing problem
                throw ex;
            }
            return searchAccount;


        }

        public Account GetAccountById(int accountId)
        {
            Account searchAccount = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT account_id, user_id, balance from account
                                                        where account_id = @account_id", conn);

                    cmd.Parameters.AddWithValue("@account_Id", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        searchAccount = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException ex)
            {
                //Causing problem
                throw ex;
            }
            return searchAccount;


        }






















        public List<Transfer> GetAllTransfersForAccount(int accountId)
        {
            List<Transfer> listOfTransfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Changed usersF
                    SqlCommand cmd = new SqlCommand(@"SELECT transfer_id, transfer_type_id,  transfer_status_id, account_from, account_to, amount, 1 as Sender,usersfrom.username as name_from,usersto.username as name_to
                                                      FROM transfers 
                                                      
                                                      LEFT JOIN account as fromaccount on account_from = account_id
                                                      LEFT JOIN account as toaccount on toaccount.account_id = account_to
                                                      
                                                      LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
                                                      LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id
                                                      
                                                      WHERE account_from = 2001
                                                      
                                                      UNION
                                                      
                                                      SELECT transfer_id, transfer_type_id,  transfer_status_id, account_from, account_to, amount, 0 as Sender,usersfrom.username as name_from,usersto.username as name_to
                                                      FROM transfers 
                                                      
                                                      LEFT JOIN account as fromaccount on account_from = account_id
                                                      LEFT JOIN account as toaccount on toaccount.account_id = account_to
                                                      
                                                      LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
                                                      LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id
                                                      WHERE account_to = 2001", conn);

                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Transfer transfer = GetTransferFromReader(reader);

                        listOfTransfers.Add(transfer);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return listOfTransfers;
        }

        public Transfer CreateATransfer(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO transfer (transfer_type_id,transfer_status_id,account_from,account_to,amount)
                                                      OUTPUT inserted.transfer_id
                                                      VALUES (@transfer_type_id,@transfer_status_id,@account_from,@account_to,@amount)", conn);

                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeId);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.accountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.accountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.amount);

                    transfer.TransferId = (int)cmd.ExecuteScalar();

                    return transfer;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public bool UpdateATransferStatus(int transferId, int transferStatusId)
        {
            bool isUpdateSuccessful = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE transfers SET transfer_status_id = @transferStatusId
                                                      where transfer_type_id = @transferId)", conn);

                    cmd.Parameters.AddWithValue("@transferId", transferId);
                    cmd.Parameters.AddWithValue("@transferStatusId", transferStatusId);
                    isUpdateSuccessful = true;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return isUpdateSuccessful;
        }

        public Account UpdateBalance(int accountId, decimal moneyToAdd)
        {
            var balance = GetAccountById(accountId).balance;
            var newBalance = balance + moneyToAdd;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE account
                                                      SET balance = @newBalance
                                                      WHERE account_id = @account_id", conn);

                    cmd.Parameters.AddWithValue("@newBalance", newBalance);
                    cmd.Parameters.AddWithValue("@account_id", accountId);

                    cmd.ExecuteNonQuery();

                    return GetAccountById(accountId);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }



        public int GetAccountIdFromUserId(int userId)
        {
            Account account = new Account();
            int accountId = account.accountId ;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"Select account_id from account
                                                      JOIN tenmo_user on tenmo_user.user_id = account.user_id
                                                      WHERE tenmo_user.user_id = @userId", conn);

                    cmd.Parameters.AddWithValue("@userId", userId);

                    accountId = (int)cmd.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return accountId;

        }

        public string GetUserNameFromAccountId(int accountId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"Select username from tenmo_user
                                                      JOIN account on tenmo_user.user_id = account.user_id
                                                      WHERE account_id = @accountId", conn);
                    cmd.Parameters.AddWithValue("@accountId", accountId);

                    string userName = (string)cmd.ExecuteScalar();
                    return userName;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        private Account CreateAccountFromReader(SqlDataReader reader)
        {
            Account account = new Account();
            account.accountId = Convert.ToInt32(reader["account_id"]);
            account.userId = Convert.ToInt32(reader["user_id"]);
            account.balance = Convert.ToDecimal(reader["balance"]);


            return account;
        }

        private Account GetAccountFromReader(SqlDataReader reader)
        {
            Account account = new Account()
            {
                accountId = Convert.ToInt32(reader["account_id"]),
                userId = Convert.ToInt32(reader["user_id"]),
                balance = Convert.ToDecimal(reader["balance"])
            };
            return account;
        }
        /// <summary>
        /// //////////////////////////////changed this
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer((int)reader["transfer_type_id"], (int)reader["transfer_status_id"],
                (int)reader["account_from"], (int)reader["account_to"], (decimal)reader["amount"]);
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            // transfer.Sender = (int)reader["Sender"];
            //transfer.toUser = (string)reader["name_to"];
            //transfer.fromUser = (string)reader["name_from"];
            return transfer;
        }
        public Account Update(int id, Account account)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE account SET user_id = @user_id, balance = @balance WHERE account_id = @account_id", conn);
                cmd.Parameters.AddWithValue("@account_id", id);
                cmd.Parameters.AddWithValue("@user_id", account.userId);
                cmd.Parameters.AddWithValue("@balance", account.balance);
                cmd.ExecuteNonQuery();
            }

            return null;
        }
    }
}

