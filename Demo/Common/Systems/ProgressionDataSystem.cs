using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Demo.Common.DataStructures;
using System.Text.Json;
using System.IO;

namespace Demo.Common.Systems
{
    public class ProgressionDataSystem : ModSystem
    {
        public static List<StageInfo> ProgressionData { get; private set; }
        public override void PostSetupContent()
        {
            using Stream stream = Mod.GetFileStream("Content/Data/progression.json"); // path relative to your mod's root
            using StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            ProgressionData = JsonSerializer.Deserialize<List<StageInfo>>(json);

            Terraria.Main.NewText($"Progression stages loaded: {ProgressionData?.Count ?? 0}");
            
        }
    }
}