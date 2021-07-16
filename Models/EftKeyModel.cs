using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phenryr.Models
{


    public class EftKeyModel 
    {   
        [JsonProperty("Icon")]
        public string Icon { get; set; }
        [JsonProperty("Key Name")]
        public string KeyName { get; set; }
        [JsonProperty("Loot")]
        public IDictionary<string, string[]> Loot { get; set; }
        [JsonProperty("Used In Quest")]
        public IDictionary<string, bool> UsedInQuest { get; set; }
        [JsonProperty("Always Unlocked")]
        public IDictionary<string, bool> AlwaysUnlocked  { get; set; }

   
    }

    public class KeyList
    {
        private List<EftKeyModel> keys;

        public EftKeyModel this[int index]
        {
            get { return keys[index]; }
            set { keys[index] = value; }
        }

        public List<EftKeyModel > Keys { get => keys; set => keys = value; }

        
    }
}
