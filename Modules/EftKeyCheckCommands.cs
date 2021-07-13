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
            string result = string.Empty;
            KeyList keys = JsonConvert.DeserializeObject<KeyList>(File.ReadAllText($"{keyFilePath}"));

            return keys;
            
        }

        [Command("keycheck")]
        public async Task EftKeyChecker([Remainder]string keyToSearch)
        {
            var eb = new EmbedBuilder();
            var sb = new StringBuilder();
            MarketModel keyInfo;
            char curSym = '\u20BD';
            char trendUp = '\u2197';
            char trendDown = '\u2198';

            KeyList keyDB = ImportKeyData();

            List<EftKeyModel> keyList = keyDB.Keys;

            EftKeyModel targetKey = keyList.Where(k => k.KeyName.ToLower().Contains(keyToSearch.ToLower())).FirstOrDefault();

            keyInfo = await EftMarketCommands.FetchMarketInfo(keyToSearch.ToLower());
            
            sb.AppendLine($"Current: {keyInfo.Price}{curSym}");
            sb.AppendLine($"24hr Average: {keyInfo.Avg24hPrice}{curSym}");
            sb.AppendLine($"7 Day Average: {keyInfo.Avg7daysPrice}{curSym}");
            sb.AppendLine($"Trader Price: {keyInfo.TraderPrice}{keyInfo.TraderPriceCur} from {keyInfo.TraderName}");

            if(keyInfo.Diff24H < 0)
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
            
            if (targetKey != null)
            {
                eb.Title = targetKey.KeyName;
                eb.ThumbnailUrl = targetKey.Icon;
                var lootLines = targetKey.Loot.Select(kvp => kvp.Key + ": " + string.Join(", ", kvp.Value));
                eb.AddField("Loot:", string.Join(Environment.NewLine, lootLines));
            }
            else
            {
                eb.Title = keyInfo.Name;
                eb.ThumbnailUrl = keyInfo.Icon;
            }
            eb.AddField("Price Data:", sb.ToString());

            eb.WithUrl(keyInfo.WikiLink);
            




            await ReplyAsync(null, false, eb.Build());

        }



    }
}
