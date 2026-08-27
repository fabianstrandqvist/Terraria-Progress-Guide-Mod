using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Demo.Common.DataStructures;
using System.Text.Json;
using System.IO;
using Terraria;
using Terraria.ID;

namespace Demo.Common.Systems
{
    public class ProgressionDataSystem : ModSystem
    {
        public static List<StageInfo> ProgressionData { get; private set; } // not needed maybe
        public static Dictionary<string, int> ItemToIdMap { get; private set; } = new Dictionary<string, int>();
        public override void PostSetupContent()
        {

            // V1: load json only
            using Stream stream = Mod.GetFileStream("Content/Data/progression.json"); // path relative to your mod's root
            using StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            ProgressionData = JsonSerializer.Deserialize<List<StageInfo>>(json);

            // NEW: create a dict that maps items to their IDs for quicker lookups
            // Might need fuzzy finding if names do not perfectly match, but for now, let's assume they do

            // Terraria.ID.ItemID itemID = new Terraria.ID.ItemID();

            ItemToIdMap = new Dictionary<string, int>();
            foreach (StageInfo stage in ProgressionData)
            {
                foreach (var classlist in stage.Classes)
                {
                    foreach (var box in classlist.Boxes)
                    {
                        // Must delete spaces, and also look at ' encoding if its incorrect!
                        foreach (var item in box.Items)
                        {
                            string cleanedItem = item.Replace(" ", "").Replace("'", ""); // what about the ' encoding?

                            // one option is to use cleanItem as key, but it is probably better to use original so its consistent with the json data file
                            if (ItemID.Search.TryGetId(cleanedItem, out int itemId))
                            {
                                ItemToIdMap[item] = (short)itemId;
                            }

                            // needs fuuzzy finding later, for example woodenyoyo exists as woodyoyo and does not work

                    

                        }

                    }
                }
            }

            
            
        }
    }
}