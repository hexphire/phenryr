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
using Phenryr.Services;
using System.Net.Http;
using Phenryr.Models;
using System.Windows.Media.Imaging;

namespace Phenryr.Modules
{
    public class XkcdCommands : ModuleBase
    {

        public static async Task<XkcdModel> LoadComic(int comicNumber = 0)
        {
            string url = "";

            if(comicNumber > 0)
            {
                url = $"https://xkcd.com/{comicNumber}/info.0.json";
            }
            else
            {
                url = $"https://xkcd.com/info.0.json";
            }

            using (HttpResponseMessage response = await ApiService.HttpApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    XkcdModel comic = await response.Content.ReadAsAsync<XkcdModel>();

                    return comic;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        [Command("xkcd")]
        public async Task xkcdComicAsync(int comicNum = 0)
        {
            var eb = new EmbedBuilder();
            var comic = await LoadComic(comicNum);
            eb.Title = "Today's XKCD:";
            eb.Description = $"XKCD number: {comic.Num}";
            eb.ImageUrl = comic.Img;

            await ReplyAsync(null, false, eb.Build());

        }

        
    }
}
