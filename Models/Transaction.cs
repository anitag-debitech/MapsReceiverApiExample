﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapsReceiverApi.Models
{
    public class Transaction
    {
        public string BankCode { get; set; }
        public Guid NetUpTransactionGuid { get; set; }
        public string SourceDeliveryService { get; set; }
        public DateTime InitialRecieveDateTime { get; set; }
        public DateTime ForwardedDateTime { get; set; }
        public string BankTransactionId { get; set; }
        public string AccountNumber { get; set; }
        public string Currency { get; set; }
        public bool IsCreditTransaction { get; set; }
        public double TransactionValue { get; set; }
        public double TransactionFeesValue { get; set; }
        public string PayerReferenceNumber { get; set; }
        public double AccountBalance { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public DateTime EffectiveDateTime { get; set; }
        public bool PendingClearance { get; set; }
        public string TransactionDescription { get; set; }
        public string PaymentChannel { get; set; }
        public string BankProvidedChecksum { get; set; }
        public bool BankChecksumMatched { get; set; }
        public string NetUpChecksum { get; set; }
        public string AdditionalData1 { get; set; }
        public string AdditionalData2 { get; set; }
        public string AdditionalData3 { get; set; }
        public string AdditionalData4 { get; set; }
        public string AdditionalData5 { get; set; }
    }
}