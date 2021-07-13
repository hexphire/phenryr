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
        public async Task EftKeyChecker(string keyToSearch)
        {
            var eb = new EmbedBuilder();
            var sb = new StringBuilder();
            char curSym = '\u20BD';
            KeyList keyDB = ImportKeyData();

            List<EftKeyModel> keyList = keyDB.Keys;

            EftKeyModel targetKey = keyList.Where(k => k.KeyName.ToLower().Contains(keyToSearch.ToLower())).FirstOrDefault();

            if(targetKey == null)
            {
                await ReplyAsync("key not found, sorry");
                return;
            }

            MarketModel keyInfo = EftMarketCommands.FetchMarketInfo(targetKey.KeyName);
               
            var lootLines = targetKey.Loot.Select(kvp => kvp.Key + ": " + string.Join(", ", kvp.Value));

            sb.AppendLine($"Current: {keyInfo.Price}{curSym}");
            sb.AppendLine($"24hr Average: {keyInfo.Avg24hPrice}{curSym}");
            sb.AppendLine($"7 Day Average: {keyInfo.Avg7daysPrice}{curSym}");
            sb.AppendLine($"Trader Price: {keyInfo.TraderPrice}{keyInfo.TraderPriceCur} from {keyInfo.TraderName}");

            eb.Title = targetKey.KeyName;
            eb.ThumbnailUrl = targetKey.Icon;
            eb.AddField("Loot:", string.Join(Environment.NewLine, lootLines));
            eb.AddField("Price Data:", sb.ToString());

            eb.WithUrl(keyInfo.WikiLink);
            




            await ReplyAsync(null, false, eb.Build());

        }



    }
}
