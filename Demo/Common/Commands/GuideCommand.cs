using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Common.Systems;
using Demo.Common.DataStructures;


namespace Demo.Common.Commands
{
    public abstract class GuideCommand : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        private record ProgressionStage(string Name, Func<bool> IsCleared);

        public List<StageInfo> Data => ProgressionDataSystem.ProgressionData;
        public Dictionary<string, int> ItemToIdMap => ProgressionDataSystem.ItemToIdMap;


        // how do I distinct between Gearing Up and Pre-Bosses?
        // Fix progression logic
        List<ProgressionStage> stages = new List<ProgressionStage>()
        {
            new ProgressionStage("Gearing Up",     () => NPC.downedBoss1),
            new ProgressionStage("Pre-Bosses", () => NPC.downedBoss1),
            new ProgressionStage("Pre-Skeletron",  () => NPC.downedBoss3),
            new ProgressionStage("Pre-Wall of Flesh", () => NPC.downedMechBossAny),
            new ProgressionStage("Pre-mechanical bosses", () => NPC.downedMechBossAny),
            new ProgressionStage("Pre-Plantera", () => NPC.downedMechBossAny),
            new ProgressionStage("Pre-Golem", () => NPC.downedMechBossAny),
            new ProgressionStage("Pre-Lunatic Cultist", () => NPC.downedMechBossAny),
            new ProgressionStage("Pre-Moon Lord", () => NPC.downedMechBossAny),
            new ProgressionStage("Endgame", () => NPC.downedMechBossAny),
        
        };

        protected int ProgressionCheck()
        {
            for (int i = 0; i < stages.Count; i++)
            {
                if (!stages[i].IsCleared())
                {
                    return i;
                }
            }
            return -1;
        }

        protected List<InfoBox> GetChildren(InfoBox box)
        {
            List<InfoBox> items = new List<InfoBox>();
            foreach (var child in box.Children)
            {
                items.Add(child);
                items.AddRange(GetChildren(child));
            }
            return items;
        }

        protected void PrintItems(int classIndex, CommandCaller caller)
        {
            Data[ProgressionCheck()].Classes[classIndex].Boxes.ForEach(box =>
            {
                PrintHelper(box, caller);
                GetChildren(box).ForEach(child =>
                {
                    PrintHelper(child, caller);
                });
            });
        }

        private void PrintHelper(InfoBox box, CommandCaller caller)
        {
            caller.Reply($"Equipment: {box.Title}");
            box.Items.ForEach(itemId =>
            {
                Item item = new Item();
                item.SetDefaults(int.Parse(itemId));
                caller.Reply($"{itemId}: {item.Name}");
            });
        }
    }
    
    public class Melee : GuideCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "meleeguide";
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Weapons available:");
            caller.Reply($"Progression stage: {ProgressionCheck()}");
            caller.Reply($"Description: {Data[ProgressionCheck()].Description}");

            // this will probably miss recursive children, fix this later!
            // should i also cache it or something to a dict instead of populating with defalt values etc like now

            PrintItems(0, caller);
        }
    }

    public class Ranged : GuideCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "rangedguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are ranged");
            PrintItems(1, caller);
        }
    }

    public class Magic : GuideCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "magicguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are magic");
            PrintItems(2, caller);
        }
    }

    public class Summoner : GuideCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "summonerguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are summoner");
            PrintItems(3, caller);
        }
    }
}

