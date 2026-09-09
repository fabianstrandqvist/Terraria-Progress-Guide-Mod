using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Demo.Common.DataStructures;
using System.Text.Json;
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

            // // Deserialize the JSON into a list of StageInfo objects
            ProgressionData = JsonSerializer.Deserialize<List<StageInfo>>(json);

            GuideUISystem.SomethingUIStatic?.PopulateItems();
 
        }

    }
}