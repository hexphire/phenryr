using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phenryr.Models
{
    public class MarketModel
    {
        public string Uid { get; set; }
        public string BsgId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public float Price { get; set; }
        public float BasePrice { get; set; }
        public float Avg24hPrice { get; set; }
        public float Avg7daysPrice { get; set; }
        public string TraderName { get; set; }
        public float TraderPrice { get; set; }
        public string TraderPriceCur { get; set; }
        public string Updated { get; set; }
        public float Slots { get; set; }
        public float Diff24H { get; set; }
        public float Diff7Days { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public string WikiLink { get; set; }
        public string Img { get; set; }
        public string ImgBig { get; set; }
        public string Reference { get; set; }
    }
}
