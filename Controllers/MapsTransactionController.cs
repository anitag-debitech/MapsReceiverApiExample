using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Services.Description;
using MapsReceiverApi.Models;
using Newtonsoft.Json;
using NLog;

namespace MapsReceiverApi.Controllers
{
    public class MapsTransactionController : ApiController
    {

        Logger logger = LogManager.GetCurrentClassLogger();

        // TODO: Change the API key in the Web.config file to your API key
        private string myApiKey = ConfigurationManager.AppSettings["ApiKey"];
        private string expectedSourceIp = ConfigurationManager.AppSettings["ExpectedSourceIp"];

        [HttpPost]
        public void ReceiveTransaction(List<Transaction> transactions)
        {
            // Validate the API key sent in the request
            var apiHeaderValue = Request.Headers.FirstOrDefault(x => x.Key == "API");
            if (!apiHeaderValue.Value.Any() || apiHeaderValue.Value.FirstOrDefault() != myApiKey)
            {
                logger.Error("The request from the remote server did not include the required API key!");
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            // Validate source IP address (Optional)
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string originIp = GetSourceIpAddress();
            if (expectedSourceIp != null && originIp != expectedSourceIp)
            {
                logger.Error($"The request originated from an unexpected source IP address. The origin IP address was: {originIp}");
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            logger.Debug($"Received {transactions.Count} Transactions");
            
            foreach (var transaction in transactions)
            {
                // Validate the transaction checksum
                if (!ValidateChecksum(transaction))
                {
                    logger.Error($"Checksum validation for transaction {transaction.NetUpTransactionGuid} has failed");
                    continue;
                }

                logger.Info($"MAPS Transaction with GUID {transaction.NetUpTransactionGuid} has been validated");

                //TODO: Implement application logic
                logger.Debug($"Transaction Data for {transaction.NetUpTransactionGuid}:\n " +
                             $"{JsonConvert.SerializeObject(transaction, Formatting.Indented)}");
            }
        }

        protected bool ValidateChecksum(Transaction transaction)
        {
            string expectedChecksum;

            #region Internal Logic for checksum validation

            NumberFormatInfo setPrecision = new NumberFormatInfo();
            setPrecision.NumberDecimalDigits = 2;
            setPrecision.CurrencyGroupSeparator = "";
            string creditDebitTransaction = transaction.IsCreditTransaction ? "c" : "d";
            string hashDetails = $"{transaction.TransactionValue.ToString("F", setPrecision) + creditDebitTransaction + transaction.AccountNumber + transaction.PayerReferenceNumber}";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(hashDetails));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                expectedChecksum = builder.ToString();
            }

            #endregion

            return expectedChecksum == transaction.NetUpChecksum;
        }

        protected string GetSourceIpAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
