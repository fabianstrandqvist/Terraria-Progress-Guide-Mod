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
        public static Dictionary<(int, int), List<InfoBox>> ProgressionCache { get; private set; } // stageIndex, classIndex -> list of itemIDs
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
            ProgressionCache = BuildCache();

            GuideUISystem.SomethingUIStatic?.PopulateItems(0, 0); // demoing class 0 and stage 0
 
        }

        // TODO: make the item calling recursive later - also save the box title as well
        private Dictionary<(int, int), List<InfoBox>> BuildCache()
        {
            var cache = new Dictionary<(int, int), List<InfoBox>>();

            if (ProgressionData == null || ProgressionData.Count == 0)
                return cache;

            for (int stageIndex = 0; stageIndex < ProgressionData.Count; stageIndex++)
            {
                var stage = ProgressionData[stageIndex];

                for (int classIndex = 0; classIndex < stage.Classes.Count; classIndex++)
                {
                    var classInfo = stage.Classes[classIndex];
                    var classList = new List<InfoBox>();
                    classList.AddRange(classInfo.Boxes); // add the InfoBoxes for this class to the list
                    cache[(stageIndex, classIndex)] = classList; // store the list of InfoBoxes for this class in the cache

                    
                }
            }

            return cache;
        }

    }
}