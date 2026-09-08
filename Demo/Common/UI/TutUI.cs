using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Demo.Common.UI
{
    public class GuideUISystem : ModSystem
    {
        internal static JournalUI SomethingUIStatic;
        public UserInterface somethingInterface;

        internal LogoUI logoUI;
        public UserInterface logoInterface;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                SomethingUIStatic = new JournalUI();
                SomethingUIStatic.Initialize();
                somethingInterface = new UserInterface();
                somethingInterface.SetState(SomethingUIStatic);

                logoUI = new LogoUI();          
                logoUI.Initialize();
                logoInterface = new UserInterface();
                logoInterface.SetState(logoUI);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (!Main.gameMenu && JournalUI.visible)
            {
                somethingInterface?.Update(gameTime);
            }

            if (!Main.gameMenu && LogoUI.visible)
            {
                logoInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Add(new LegacyGameInterfaceLayer("Demo: Journal UI", DrawSomethingUI, InterfaceScaleType.UI));
            layers.Add(new LegacyGameInterfaceLayer("Demo: Logo UI", DrawLogoUI, InterfaceScaleType.UI));
        }

        private bool DrawSomethingUI()
        {
            if (!Main.gameMenu && JournalUI.visible)
            {
                somethingInterface.Draw(Main.spriteBatch, new GameTime());
            }
            return true;
        }

        private bool DrawLogoUI()
        {
            if (!Main.gameMenu && LogoUI.visible)
            {
                logoInterface.Draw(Main.spriteBatch, new GameTime());
            }
            return true;
        }
    }
}