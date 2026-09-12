using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;
using System.Collections.Generic;
using Terraria.GameContent;

namespace Demo.Common.UI
{
    internal class GuidePanel : UIPanel
    {
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }

    internal class ItemGrid : UIElement
    {
        private const float SlotSize = 40f;
        private const float Padding = 4f;
        private const int ColumnsPerRow = 8;

        public ItemGrid(List<int> itemIDs)
        {
            for (int i = 0; i < itemIDs.Count; i++)
            {
                int row = i / ColumnsPerRow;
                int col = i % ColumnsPerRow;

                var slot = new ItemSlot(itemIDs[i]);
                slot.Width.Set(SlotSize, 0f);
                slot.Height.Set(SlotSize, 0f);
                slot.Left.Set(col * (SlotSize + Padding), 0f);
                slot.Top.Set(row * (SlotSize + Padding), 0f);

                Append(slot);
            }
            int rows = (itemIDs.Count + ColumnsPerRow - 1) / ColumnsPerRow;   // rounds up: 7 items -> 2 rows
            Width.Set(0f, 1f);                                               // fill the list's width
            Height.Set(rows * (SlotSize + Padding), 0f); 
        }
    }

	internal class ItemSlot : UIPanel
	{
		private Item item;

		public ItemSlot(int itemID)
		{
            Item item = new Item();
            item.SetDefaults(itemID);
			this.item = item;
		}

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch); // draws the panel's background/border

            // now draw the item's icon on top of that background
            Main.instance.LoadItem(item.type); // ensures the item's texture is loaded
            Texture2D itemTexture = TextureAssets.Item[item.type].Value;

            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = new Vector2(
                dimensions.X + dimensions.Width / 2f,
                dimensions.Y + dimensions.Height / 2f
            );

            spriteBatch.Draw(
                itemTexture,
                position,
                null,
                Color.White,
                0f,
                itemTexture.Size() / 2f, // origin = center, so it's centered on `position`
                1f,
                SpriteEffects.None,
                0f
            );
        }

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);

			// Tell your UI that this item is being hovered
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);

			// Hide/clear item information
		}
	}



}