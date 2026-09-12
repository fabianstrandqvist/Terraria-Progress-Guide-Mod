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

        private UIList itemList;
        private UITextPanel<string> nextButton;

        public List<StageInfo> Data => ProgressionDataSystem.ProgressionData;
        public Dictionary<(int, int), List<InfoBox>> Cache => ProgressionDataSystem.ProgressionCache;
        private int stage;
        private int classIndex;
        private UITextPanel<string> classText;

        public override void OnInitialize()
        {
            visible = false;

            panel = new GuidePanel();
            panel.Left.Set(500, 0);
            panel.Top.Set(100, 0);
            panel.Width.Set(500, 0);
            panel.Height.Set(500, 0);
            Append(panel);

            // // start with an empty grid — no data access yet
            // grid = new ItemGrid(new List<Item>());
            // grid.Width.Set(0f, 1f);
            // grid.Height.Set(0f, 1f);
            // panel.Append(grid);

            // thinking to make one grid for each class, that can be tabbed to change
            // or is it better to just have a single grid then but the item changes?

            // should change to the next page
            // its placement should also depend on the full panel size
            nextButton = new UITextPanel<string>(">");
            nextButton.Left.Set(420f, 0f);
            nextButton.Top.Set(5f, 0f);
            nextButton.Width.Set(20f, 0f);
            nextButton.Height.Set(20f, 0f);
            nextButton.OnLeftClick += (evt, element) =>
            {
                PopulateItems(stage, (classIndex + 1) % 5); // this assumes that all stages have 5 classes, can be unsafe
            };
            panel.Append(nextButton);

            classText = new UITextPanel<string>("Melee");
            classText.Left.Set(200f, 0f);
            classText.Top.Set(5f, 0f);
            classText.Width.Set(100f, 0f);
            classText.Height.Set(20f, 0f);
            panel.Append(classText);

            itemList = new UIList();
            itemList.Top.Set(50f, 0f);
            itemList.Width.Set(0f, 1f);
            itemList.Height.Set(-30f, 1f);
            itemList.ManualSortMethod = (e) => { };
            panel.Append(itemList);

        }


        public void PopulateItems(int stage, int classIndex)
        {
            this.stage = stage;
            this.classIndex = classIndex;
            // panel.RemoveChild(grid);
            // // demoing class 0 and stage 0
            // grid = new ItemGrid(GetItems(stage, classIndex));
            // grid.Width.Set(0f, 1f);
            // grid.Height.Set(0f, 1f);
            // grid.IgnoresMouseInteraction = true; // temporary fix
            // panel.Append(grid);

            itemList.Clear();
            //itemList.Add(new ItemGrid(GetItems(stage, classIndex)));
            foreach (var box in Cache[(stage, classIndex)])
            {
                var boxText = new UITextPanel<string>(box.Title);
                boxText.Width.Set(0f, 1f);
                boxText.Height.Set(20f, 0f);
                itemList.Add(boxText);
                itemList.Add(new ItemGrid(box.Items));
            }
            
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