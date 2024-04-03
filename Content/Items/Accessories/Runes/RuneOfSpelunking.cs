using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfSpelunking : RuneItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Orange;
			Item.scale = 0.5f;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.nightVision = true;
			player.findTreasure = true;
			player.pickSpeed -= 0.20f;

			if(Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(player.position, player.width / 2, player.height, DustID.GoldCoin);
				dust.noGravity = true;
				dust.velocity.Y -= 1f;
				dust.noLightEmittence = true;
			}
		}

		// The rune icon would be too big when dropped and Item.scale did not fix this
		// If the sprite's size is reduced then the icon is too small in the inventory so either way drawing would be required..
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Asset<Texture2D> texture = TextureAssets.Item[Item.type];

			Rectangle? source = null;
			if (Main.itemAnimations[Type] != null)
			{
				// The current frame of the animation, null check for items that have one frame
				source = Main.itemAnimations[Type].GetFrame(texture.Value);
			}

			// Draw item with 0.5 scale
			Main.spriteBatch.Draw(texture.Value, Item.position - Main.screenPosition, source, lightColor, 0, Vector2.Zero, scale * 0.75f, SpriteEffects.None, 0f);

			return false;
		}
	}
}
