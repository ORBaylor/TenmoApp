using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;

        public TransferSqlDao(string connString)
        {
            connectionString = connString;
        }

        public IList<Transfer> AllTransferList()
        {
            IList<Transfer> transfers = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM transfer ", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Transfer transfer = GetTransferFromReader(reader);
                    transfers.Add(transfer);

                }
            }
            return transfers;
        }

        public Transfer CreateATransfer(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO transfers (transfer_type_id,transfer_status_id,account_from,account_to,amount)
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

        
        public List<Transfer> GetListOfCompletedTransfersFromAccount(int accountId)
        {
            List<Transfer> transfers = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT transfer_type_desc, transfer_status_desc, account_from, account_to, amount FROM transfer JOIN account on " +
                    "account_from = account_id WHERE " +
                    "account_id = @account_id AND transfer_id >2999 ", conn);
                cmd.Parameters.AddWithValue("@account_id", accountId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Transfer transfer = GetTransferFromReader(reader);
                    transfers.Add(transfer);
                }


            }
            return transfers;
          
        }
    
        // Still working on controller
        public List<Transfer> GetListOfTransfersFromAccount(int accountId)
        {
            List<Transfer> transfers = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount FROM transfer JOIN account on account_from = account_id WHERE account_id = @account_id AND transfer_id > 2999
                                                    ", conn);

                cmd.Parameters.AddWithValue("@account_id", accountId );

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Transfer transfer = GetTransferFromReader(reader);
                    transfers.Add(transfer);
                }


            }
            return transfers;
        }

        public Transfer GetTransferDetails(int transferId)
        {
            Transfer transfer = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM transfer WHERE transfer_id = @transfer_id", conn);
                cmd.Parameters.AddWithValue("@transfer_id", transferId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                     transfer = GetTransferFromReader(reader);

                }
                return transfer;
            }
            
        }

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

        

    }
}
