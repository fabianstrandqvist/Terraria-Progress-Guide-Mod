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

        public List<StageInfo> Data => ProgressionDataSystem.ProgressionData;

        public override void OnInitialize()
        {
            visible = false;

            panel = new GuidePanel();
            panel.Left.Set(500, 0);
            panel.Top.Set(100, 0);
            panel.Width.Set(100, 0);
            panel.Height.Set(100, 0);
            Append(panel);

            // start with an empty grid — no data access yet
            grid = new ItemGrid(new List<Item>());
            grid.Width.Set(0f, 1f);
            grid.Height.Set(0f, 1f);
            panel.Append(grid);

            // thinking to make one grid for each class, that can be tabbed to change
            // or is it better to just have a single grid then but the item changes?
        }

        private List<Item> GetItems()
        {
            var items = new List<Item>();

            if (Data == null || Data.Count == 0)
                return items;

            foreach (var classInfo in Data[0].Classes)
            {
                foreach (var box in classInfo.Boxes)
                {
                    foreach (var itemID in box.Items)
                    {
                        if (int.TryParse(itemID, out int id))
                        {
                            Item item = new Item();
                            item.SetDefaults(id);
                            items.Add(item);
                        }
                    }
                }
            }

            return items;
        }


        public void PopulateItems()
        {
            panel.RemoveChild(grid);

            grid = new ItemGrid(GetItems());
            grid.Width.Set(0f, 1f);
            grid.Height.Set(0f, 1f);
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