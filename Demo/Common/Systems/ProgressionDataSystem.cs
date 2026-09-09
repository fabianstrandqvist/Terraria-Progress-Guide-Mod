using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Demo.Common.DataStructures;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Terraria;
using Terraria.ID;
using Demo.Common.UI;

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

            // Item IDs are quoted numbers in the JSON ("55"), so allow reading them straight into int
            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            // Deserialize the JSON into a list of StageInfo objects
            ProgressionData = JsonSerializer.Deserialize<List<StageInfo>>(json, options);

            GuideUISystem.SomethingUIStatic?.PopulateItems();
 
        }

    }
}