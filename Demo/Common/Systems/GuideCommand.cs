using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Common.Systems
{
    public abstract class GuideCommands : ModCommand
    {
        public override CommandType Type => CommandType.Chat;
        private record ProgressionStage(string Name, Func<bool> IsCleared);

        // The actual wiki progression is a little confusing
        // this can be problematic if player does not follow intended progression, refactor later
        List<ProgressionStage> stages = new List<ProgressionStage>()
        {
            new ProgressionStage("Pre-Bosses (King Slime, Eye of Cthulhu ...)",     () => NPC.downedBoss1),
            new ProgressionStage("Pre-Eater of Worlds / Brain of Cthulhu / Queen Bee", () => NPC.downedBoss2),
            new ProgressionStage("Pre-Skeletron",  () => NPC.downedBoss3),
            new ProgressionStage("Pre-Hardmode (wall of flesh?)", () => NPC.downedMechBossAny),
        
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
    
    public class Melee : GuideCommands
    {
        public override CommandType Type => CommandType.Chat; // do i have to write this again?
        public override string Command => "meleeguide";

        // placeholder but testing progression check
        int[][] weapons = new int[][]
        {
            new int[] { ItemID.CopperShortsword, ItemID.CopperBroadsword, ItemID.CopperBow },
            new int[] { ItemID.IronShortsword, ItemID.IronBroadsword, ItemID.IronBow },
            new int[] { ItemID.SilverShortsword, ItemID.SilverBroadsword, ItemID.SilverBow },
            new int[] { ItemID.GoldShortsword, ItemID.GoldBroadsword, ItemID.GoldBow },
        };
        
        // another problem is that I can only have one command per class, but there are more than one type of equipment
        // lets just do weapons for now
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are melee");
            caller.Reply("Weapons available:");
            caller.Reply($"Progression stage: {ProgressionCheck()}");
            weapons[ProgressionCheck()].ToList().ForEach(weapon => caller.Reply(Lang.GetItemNameValue(weapon)));
        }
    }

    public class Ranged : GuideCommands
    {
        public override CommandType Type => CommandType.Chat; // do i have to write this again?
        public override string Command => "rangedguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are ranged");
        }
    }

    public class Magic : GuideCommands
    {
        public override CommandType Type => CommandType.Chat; // do i have to write this again?
        public override string Command => "magicguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are magic");
        }
    }

    public class Summoner : GuideCommands
    {
        public override CommandType Type => CommandType.Chat; // do i have to write this again?
        public override string Command => "summonerguide";
        
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            caller.Reply("Your are summoner");
        }
    }
}

