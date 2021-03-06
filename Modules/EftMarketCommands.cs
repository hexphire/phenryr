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
            var uri = new Uri("https://tarkov-market.com/api/v1/item?q=" + $"{itemName.Replace("%20", "-")}")
            {

            };
            using (HttpResponseMessage response = await ApiService.EftApiClient.GetAsync(uri))
            {
                if (response.IsSuccessStatusCode)
                {
                    
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    
                    
                    if(JsonConvert.DeserializeObject<List<MarketModel>>(jsonString).Count == 0)
                    {
                        return null;
                    }

                    var item = JsonConvert.DeserializeObject<List<MarketModel>>(jsonString)[0];
                    return item;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }


        [Command("pricecheck")]
        [RoleRequired("Member")]
        public async Task EftPriceCheck([Remainder]string item)
        {
            var eb = new EmbedBuilder();
            var sb = new StringBuilder();
            char curSym = '\u20BD';
            char trendUp = '\u2197';
            char trendDown = '\u2198';       

            MarketModel itemInfo = await EftMarketCommands.FetchMarketInfo(item.ToLower());

            if(itemInfo == null)
            {
                await ReplyAsync("No item was found, check your spelling or make sure to use the whole name");
                return;
            }


            sb.AppendLine($"Current: {itemInfo.Price:N0} {curSym}");
            sb.AppendLine($"Trader Price: {itemInfo.TraderPrice:N0} {itemInfo.TraderPriceCur} from {itemInfo.TraderName}");
            sb.AppendLine("**Average Prices:**");
            sb.AppendLine($"24hr Average: {itemInfo.Avg24hPrice:N0} {curSym}");
            sb.AppendLine($"7 Day Average: {itemInfo.Avg7daysPrice:N0} {curSym}");
            sb.AppendLine("**Price Trends:**");
            if (itemInfo.Diff24H < 0)
            {
                sb.AppendLine($"24hr price trend: {trendDown}");
            }
            else
            {
                sb.AppendLine($"24hr price trend: {trendUp}");
            }
            if (itemInfo.Diff7Days < 0)
            {
                sb.AppendLine($"7 Day price trend: {trendDown}");
            }
            else
            {
                sb.AppendLine($"7 Day price trend: {trendUp}");
            }
            eb.WithTitle(itemInfo.Name);
            eb.WithThumbnailUrl(itemInfo.Icon);
            eb.AddField("Prices:", sb.ToString());
            eb.WithUrl(itemInfo.WikiLink);
            var sinceUpdate = itemInfo.Updated.Subtract(DateTime.Now);
            eb.WithFooter(footer => footer.Text = $"Price Last Updated: {sinceUpdate.Minutes} miniutes ago");


            await ReplyAsync(null, false, eb.Build());
        }
    }
}
