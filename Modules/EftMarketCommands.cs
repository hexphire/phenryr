using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Phenryr.Models;
using Phenryr.Services;
using System.Net.Http;

namespace Phenryr.Modules
{
    public class EftMarketCommands : ModuleBase
    {

        

        public static async Task<MarketModel> FetchMarketInfo(string itemName = "Physical bitcoin")
        {
            var uri = new Uri("https://tarkov-market.com/api/v1/item?q=" + $"{itemName}");


            using (HttpResponseMessage response = await ApiService.EftApiClient.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;

                    var item = JsonConvert.DeserializeObject<List<MarketModel>>(jsonString)[0];

                    return item;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }        
        }
    }
}
