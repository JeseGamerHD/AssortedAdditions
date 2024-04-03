using Terraria.ID;
using Terraria;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Tiles.CraftingStations;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using ReLogic.Content;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class ManaRune : RuneItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(11, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.scale = 0.5f;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// Only increase based on base mana without buffs etc so they won't stack
			player.statManaMax2 = (int)(player.statManaMax * 1.5f);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BlankRune>());
			recipe.AddIngredient(ModContent.ItemType<MagicEssence>(), 10);
			recipe.AddIngredient(ItemID.ManaCrystal, 3);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}

		// The rune icon would be too big when dropped and Item.scale did not fix this
		// If the sprite's size is reduced then the icon is too small in the inventory so either way drawing would be required..
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Asset<Texture2D> texture = TextureAssets.Item[Type];
			//Texture2D texture = TextureAssets.Item[Type].Value; // Item's spritesheet

			Rectangle? source = null;
			if (Main.itemAnimations[Type] != null)
			{
				// The current frame of the animation, null check for items that have one frame
				source = Main.itemAnimations[Type].GetFrame(texture.Value);
			}

			// Draw item with 0.5 scale
			Main.spriteBatch.Draw(texture.Value, Item.position - Main.screenPosition, source, lightColor, 0, Vector2.Zero, scale * 0.5f, SpriteEffects.None, 0f);

			return false;
		}
	}
}
