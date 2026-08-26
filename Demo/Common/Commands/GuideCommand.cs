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
    }
    
    public class Melee : GuideCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "meleeguide";
        
        // another problem is that I can only have one command per class, but there are more than one type of equipment
        // lets just do weapons for now - i want to build a UI system later so this is ok for now
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Weapons available:");
            caller.Reply($"Progression stage: {ProgressionCheck()}");
            caller.Reply($"Description: {Data[ProgressionCheck()].Description}");
            Data[ProgressionCheck()].Classes[0].Boxes.ForEach(box => caller.Reply($"Box title: {box.Title}"));
            
            // weapons[ProgressionCheck()].ToList().ForEach(weapon => caller.Reply(Lang.GetItemNameValue(weapon)));
        }
    }

    public class Ranged : GuideCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "rangedguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are ranged");
        }
    }

    public class Magic : GuideCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "magicguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are magic");
        }
    }

    public class Summoner : GuideCommand
    {
        public override CommandType Type => CommandType.Chat;
        public override string Command => "summonerguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are summoner");
        }
    }
}

