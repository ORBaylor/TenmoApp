using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferTypeId  { get; set; }
        // get id
        // list all id's
        public int TransferStatusId { get; set; }
        // get id
        //list all id's
        public int accountFrom { get; set; }
        //get accountFrom
        public int accountTo { get; set; }
        //get accountIn
        public decimal amount { get; set; }
        //get amount


        public int Sender { get; set; }
        public string fromUser { get; set; }
        public string toUser { get; set; }
        public Transfer(int transferTypeId, int transferStatusId, int accountFrom, int accountTo, decimal amount)
        {
            this.TransferTypeId = transferTypeId;
            this.TransferStatusId = transferStatusId;
            this.accountFrom = accountFrom;
            this.accountTo = accountTo;
            this.amount = amount;
        }
    }
}
