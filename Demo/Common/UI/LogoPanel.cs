using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;

namespace Demo.Common.UI
{

    internal class GuideIcon : UIImage
    {

        private Asset<Texture2D> texture;
        public GuideIcon(Asset<Texture2D> texture) : base(texture)
        {
            this.texture = texture;
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            JournalUI.visible = !JournalUI.visible;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();

            spriteBatch.Draw(
                texture.Value,
                dimensions.ToRectangle(),
                Color.White
            );
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

    }

}