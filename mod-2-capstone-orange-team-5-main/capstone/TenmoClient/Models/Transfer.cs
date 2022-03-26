using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
   public  class Transfer
    {
        public int TransferId { get; set; }
        public int TransferTypeId { get; set; }
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

        //public Transfer(int transferTypeId, int transferStatusId, int accountFrom, int accountTo, decimal amount)
        //{
        //    this.TransferTypeId = transferTypeId;
        //    this.TransferStatusId = transferStatusId;
        //    this.accountFrom = accountFrom;
        //    this.accountTo = accountTo;
        //    this.amount = amount;
        //}
    }
}
