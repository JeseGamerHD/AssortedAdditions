using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfDeath : RuneItem
	{
		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 40;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RuneOfDeathPlayer>().isWearingRuneOfDeath = true;
		}

		// The rune would be too big when dropped and Item.scale did not fix this
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

	public class RuneOfDeathPlayer : ModPlayer
	{
		public bool isWearingRuneOfDeath;

		readonly HashSet<int> trapProjectiles = new HashSet<int>() 
		{ 
			ProjectileID.PoisonDart,
			ProjectileID.GeyserTrap,
			ProjectileID.PoisonDartTrap,
			ProjectileID.SpikyBallTrap,
			ProjectileID.SpearTrap,
			ProjectileID.FlamethrowerTrap,
			ProjectileID.Boulder,
			ProjectileID.Explosives
		};

		public override bool CanBeHitByProjectile(Projectile proj)
		{
			if (isWearingRuneOfDeath)
			{
				if(trapProjectiles.Contains(proj.type))
				{
					return false;
				}
			}

			return true;
		}

		public override void ResetEffects()
		{
			isWearingRuneOfDeath = false;
		}
	}

	// Detour in order to grant immunity to spikes
	public class RuneOfDeathDetour : ModSystem
	{
		public override void Load()
		{
			Terraria.On_Player.ApplyTouchDamage += ImmuneToSpikes;
		}

		private static void ImmuneToSpikes(On_Player.orig_ApplyTouchDamage orig, Player self, int tileId, int x, int y)
		{
			// If the player is wearing the rune
			if (self.GetModPlayer<RuneOfDeathPlayer>().isWearingRuneOfDeath)
			{
				// And the tile is spikes
				if(tileId == TileID.Spikes || tileId == TileID.WoodenSpikes)
				{
					return; // Dont call ApplyTouchDamage to skip taking damage
				}
			}
			
			orig(self, tileId, x, y); // otherwise call orig
		}
	}
}
