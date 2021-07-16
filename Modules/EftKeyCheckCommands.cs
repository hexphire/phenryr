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

namespace Phenryr.Modules
{
    public class EftKeyCheck : ModuleBase
    {

        public static KeyList ImportKeyData()
        {
            string keyFilePath = "../../Database/TarkovKeyList.json";
            KeyList keys = JsonConvert.DeserializeObject<KeyList>(File.ReadAllText($"{keyFilePath}"));

            return keys;
            
        }

        [Command("keycheck")]
        public async Task EftKeyChecker([Remainder]string keyToSearch)
        {
            string searchTarget = keyToSearch;
            if (!searchTarget.ToLower().Contains("key"))
            {
                searchTarget += "-key";
            }
            var eb = new EmbedBuilder();
            var sb = new StringBuilder();
            MarketModel keyInfo;
            char curSym = '\u20BD';
            char trendUp = '\u2197';
            char trendDown = '\u2198';

            KeyList keyDB = ImportKeyData();

            List<EftKeyModel> keyList = keyDB.Keys;

            EftKeyModel targetKey = keyList.Where(k => k.KeyName.ToLower().Contains(keyToSearch.ToLower())).FirstOrDefault();

            
            keyInfo = await EftMarketCommands.FetchMarketInfo(searchTarget.ToLower());


            if (keyInfo == null)
            {
                await ReplyAsync("No key was found, check your spelling or make sure to use the whole name");
                return;
            }

            sb.AppendLine($"Current: {keyInfo.Price:N0} {curSym}");
            sb.AppendLine($"Trader Price: {keyInfo.TraderPrice:N0} {keyInfo.TraderPriceCur} from {keyInfo.TraderName}");
            sb.AppendLine("**Average Prices:**");
            sb.AppendLine($"24hr Average: {keyInfo.Avg24hPrice:N0} {curSym}");
            sb.AppendLine($"7 Day Average: {keyInfo.Avg7daysPrice:N0} {curSym}");
            sb.AppendLine("**Price Trends:**");
            if (keyInfo.Diff24H < 0)
            {
                sb.AppendLine($"24hr price trend: {trendDown}");
            }
            else
            {
                sb.AppendLine($"24hr price trend: {trendUp}");
            }
            if(keyInfo.Diff7Days < 0)
            {
                sb.AppendLine($"7 Day price trend: {trendDown}");
            }
            else
            {
                sb.AppendLine($"7 Day price trend: {trendUp}");
            }

            
            
            if (targetKey != null && targetKey.KeyName.Equals(keyInfo.Name))
            {
                eb.WithTitle(targetKey.KeyName);
                eb.WithThumbnailUrl(targetKey.Icon);
                var lootLines = targetKey.Loot.Select(kvp => kvp.Key + ": " + string.Join(", ", kvp.Value));
                eb.AddField("Loot:", string.Join(Environment.NewLine, lootLines));
            }
            else
            {
                eb.Title = keyInfo.Name;
                eb.ThumbnailUrl = keyInfo.Icon;

            }

            eb.AddField("Prices:", sb.ToString());
            eb.WithUrl(keyInfo.WikiLink);

            var sinceUpdate = keyInfo.Updated.Subtract(DateTime.Now);
            eb.WithFooter(footer => footer.Text = $"Price Last Updated: {sinceUpdate.Minutes} miniutes ago");
            




            await ReplyAsync(null, false, eb.Build());

        }



    }
}
