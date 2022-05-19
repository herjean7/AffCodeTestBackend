using AffynBackend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AffynBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        [HttpGet]
        [EnableCors]
        [Route("AccountBalance")]
        public async Task<AccountBalance> GetAccountBalance()
        {
            AccountBalance accBalance = new AccountBalance();

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("https://api-rinkeby.etherscan.io/api?module=account&action=balance&address=0x4BC0345E17ff8C346867417f2E183FCd5e97bd5B&tag=latest&apikey=EW3UVHFVJESQN3Y2E483FQ9CFX3JE98I8E");

                    if (Res.IsSuccessStatusCode)
                    {
                        var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                        accBalance = JsonConvert.DeserializeObject<AccountBalance>(ObjResponse);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("GetWalletAddressAsync :" + ex.Message);
                }
                return accBalance;
            }
        }

        [HttpGet]
        [Route("FynTokenInfo")]
        public async Task<String> GetFYNTokenCirculatingInfo()
        {
            string circulatingInfo = string.Empty;
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = await client.GetAsync("https://affyn-api.herokuapp.com/api/polygon/circulating");

                    if (Res.IsSuccessStatusCode)
                    {
                        circulatingInfo = Res.Content.ReadAsStringAsync().Result;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("GetFYNTokenCirculatingInfo: " + ex.Message);
                }
                return circulatingInfo;
            }
        }

        //0xfa2d3aa1a5c37fa46dd2d5c30341f6ab975b288f9c0acf145dc42dbfc0237bf2 - ok
        //0x8dac9d2429f2f73cab7db5d26986f77b96552188b1c969a50ce7bff563ffbb6c - error
        [HttpGet]
        [Route("SignBlockchainTransaction")]
        public async Task<Transaction> SignBlockchainTransaction(string transactionHash)
        {
            Transaction transaction = new Transaction();

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringBuilder sb = new StringBuilder();
                    sb.Append("https://api-rinkeby.etherscan.io/api?module=transaction&action=getstatus&txhash=");
                    sb.Append(transactionHash);
                    sb.Append("&apikey=EW3UVHFVJESQN3Y2E483FQ9CFX3JE98I8E");
                    HttpResponseMessage Res = await client.GetAsync(sb.ToString());

                    if (Res.IsSuccessStatusCode)
                    {
                        var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                        transaction = JsonConvert.DeserializeObject<Transaction>(ObjResponse);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("SignBlockchainTransaction :" + ex.Message);
                }
                return transaction;
            }
        }
    }
}
