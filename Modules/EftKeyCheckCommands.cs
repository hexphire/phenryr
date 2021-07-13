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
            KeyList keyDB = ImportKeyData();

            List<EftKeyModel> keyList = keyDB.Keys;

            EftKeyModel targetKey = keyList.Where(k => k.KeyName.ToLower().Contains(keyToSearch.ToLower())).FirstOrDefault();

            if(targetKey == null)
            {
                await ReplyAsync("key not found, sorry");
                return;
            }
   
            var lootLines = targetKey.Loot.Select(kvp => kvp.Key + ": " + string.Join(", ", kvp.Value));



            eb.Title = targetKey.KeyName;
            eb.ThumbnailUrl = targetKey.Icon;
            eb.AddField("Loot", string.Join(Environment.NewLine, lootLines));
            




            await ReplyAsync(null, false, eb.Build());

        }



    }
}
