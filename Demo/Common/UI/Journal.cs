using Terraria.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System.Collections.Generic;
using Demo.Common.Systems;
using Demo.Common.DataStructures;


namespace Demo.Common.UI
{
    internal class JournalUI : UIState
    {
        public static bool visible;
        public GuidePanel panel;
        private ItemGrid grid;
        private UITextPanel<string> nextButton;

        public List<StageInfo> Data => ProgressionDataSystem.ProgressionData;
        public Dictionary<(int, int), List<int>> Cache => ProgressionDataSystem.ProgressionCache;
        private int stage;
        private int classIndex;

        public override void OnInitialize()
        {
            visible = false;

            panel = new GuidePanel();
            panel.Left.Set(500, 0);
            panel.Top.Set(100, 0);
            panel.Width.Set(500, 0);
            panel.Height.Set(500, 0);
            Append(panel);

            // start with an empty grid — no data access yet
            grid = new ItemGrid(new List<Item>());
            grid.Width.Set(0f, 1f);
            grid.Height.Set(0f, 1f);
            panel.Append(grid);

            // thinking to make one grid for each class, that can be tabbed to change
            // or is it better to just have a single grid then but the item changes?

            // should change to the next page
            // its placement should also depend on the full panel size
            nextButton = new UITextPanel<string>(">");
            nextButton.Left.Set(100f, 0f);
            nextButton.Top.Set(100f, 0f);
            nextButton.Width.Set(20f, 0f);
            nextButton.Height.Set(20f, 0f);
            nextButton.OnLeftClick += (evt, element) =>
            {
                PopulateItems(stage, (classIndex + 1) % 5); // this assumed that all stages have 5 classes, can be unsafe
            };
            panel.Append(nextButton);

        }

        private List<Item> GetItems(int stage, int classIndex)
        {
            var items = new List<Item>();

            if (Cache == null || Cache.Count == 0)
                return items;

            foreach (int itemID in Cache[(stage, classIndex)])
            {
                
                    Item item = new Item();
                    item.SetDefaults(itemID);
                    items.Add(item);
                
            }

            return items;
        }



        public void PopulateItems(int stage, int classIndex)
        {
            this.stage = stage;
            this.classIndex = classIndex;
            panel.RemoveChild(grid);
            // demoing class 0 and stage 0
            grid = new ItemGrid(GetItems(stage, classIndex));
            grid.Width.Set(0f, 1f);
            grid.Height.Set(0f, 1f);
            grid.IgnoresMouseInteraction = true; // temporary fix
            panel.Append(grid);
        }

    }

    internal class LogoUI : UIState
    {
        public static bool visible;
        public GuideIcon icon;

        public override void OnInitialize()
        {
            visible = true;

            var texture = ModContent.Request<Texture2D>("Demo/Content/Items/Materials/SteelShard"); // adjust path to your actual PNG location
            icon = new GuideIcon(texture);
            icon.Left.Set(700f, 0f);
            icon.Top.Set(20f, 0f);
            icon.Width.Set(40f, 0f);
            icon.Height.Set(40f, 0f);

            Append(icon);
        }
    }
}