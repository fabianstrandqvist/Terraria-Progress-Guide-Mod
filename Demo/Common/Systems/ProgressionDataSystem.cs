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

            using Stream stream = Mod.GetFileStream("Content/Data/progression_id.json"); // path relative to mod root
            using StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            // // Deserialize the JSON into a list of StageInfo objects
            ProgressionData = JsonSerializer.Deserialize<List<StageInfo>>(json);
    


            // // Might need fuzzy finding if names do not perfectly match, but for now, let's assume they do
            // // Fuzzy finding should be done already at the data stage and not here since it has big ...

            // // Terraria.ID.ItemID itemID = new Terraria.ID.ItemID();

            // ItemToIdMap = new Dictionary<string, int>();
            // foreach (StageInfo stage in ProgressionData)
            // {
            //     foreach (var classlist in stage.Classes)
            //     {
            //         foreach (var box in classlist.Boxes)
            //         {
            //             // Must delete spaces, and also look at ' encoding if its incorrect!
            //             foreach (var item in box.Items)
            //             {
            //                 string cleanedItem = item.Replace(" ", "").Replace("'", ""); // what about the ' encoding?

            //                 // one option is to use cleanItem as key, but it is probably better to use original so its consistent with the json data file
            //                 if (ItemID.Search.TryGetId(cleanedItem, out int itemId))
            //                 {
            //                     ItemToIdMap[item] = (short)itemId;
            //                 }

            //                 // needs fuuzzy finding later, for example woodenyoyo exists as woodyoyo and does not work

                    

            //             }

            //         }
            //     }
            // }  
        }

    }
}