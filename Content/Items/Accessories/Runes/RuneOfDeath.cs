using System.Collections.Generic;
using Terraria;
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
