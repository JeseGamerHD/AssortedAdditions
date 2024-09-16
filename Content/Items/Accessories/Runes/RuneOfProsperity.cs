using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Tiles.CraftingStations;
using System.Collections.Generic;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfProsperity : RuneItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.LightRed;
			Item.scale = 0.5f;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RuneOfProsperityPlayer>().isWearingRuneOfProsperity = true;

			if (Main.rand.NextBool(8))
			{
				Dust dust = Dust.NewDustDirect(player.position, player.width / 2, player.height, DustID.GoldCoin);
				dust.noGravity = true;
				dust.velocity.Y -= 1f;
				dust.noLightEmittence = true;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BlankRune>());
			recipe.AddIngredient(ItemID.GoldDust, 30);
			recipe.AddIngredient(ItemID.GoldCoin, 50);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}
	}

	public class RuneOfProsperityPlayer : ModPlayer
	{
		public bool isWearingRuneOfProsperity;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (isWearingRuneOfProsperity)
			{
				target.AddBuff(BuffID.Midas, 120);
			}

		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (isWearingRuneOfProsperity)
			{
				target.AddBuff(BuffID.Midas, 120);
			}
		}

		public override void ResetEffects()
		{
			isWearingRuneOfProsperity = false;
		}
	}

	public class RuneOfProsperityDetour : ModSystem
	{
		public override void Load()
		{
			Terraria.On_Player.PickTile += CanDoubleOreDrop;
		}

		// Note: This does nothing to prevent double drops from player placed ores
		// For that any ores placed by the player would need to be tracked
		private static void CanDoubleOreDrop(On_Player.orig_PickTile orig, Player self, int x, int y, int pickPower)
		{
			Tile tile = Main.tile[x, y]; // What tile is being hit
			ushort type = tile.TileType; // Store it and its type for later use

			// If wearing the rune, the tile is an ore and the double drop chance returns true
			if (self.GetModPlayer<RuneOfProsperityPlayer>().isWearingRuneOfProsperity && TileID.Sets.Ore[tile.TileType] && Main.rand.NextBool(7))
			{
				// Check if its a mod tile and gets its drops if it is (needed later)
				ModTile modTile = TileLoader.GetTile(type);
				IEnumerable<Item> modTileDrops = modTile?.GetItemDrops(x, y);
				// Checked and stored here before the orig call since the tile wont be at the coords after breaking
				// retrieving the drops later would not work

				// Call orig (PickTile will run)
				orig(self, x, y, pickPower);

				// Check if tile broke and if it did, do the double drop
				if (!tile.HasTile)
				{
					// If the tile was not modded, get the vanilla item drop from the dictionary
					if (modTile == null && AssortedAdditions.vanillaOreTileDrops.TryGetValue(type, out int itemDrop))
					{
						self.QuickSpawnItem(WorldGen.GetItemSource_FromTileBreak(x, y), itemDrop, 1);
					}
					else if(modTileDrops != null) // If it was modded, get the drops from modTileDrops (safety null check)
					{
						foreach (var modDrop in modTileDrops)
						{
							self.QuickSpawnItem(WorldGen.GetItemSource_FromTileBreak(x, y), modDrop.type, 1);
						}
					}
				}
			}
			// Conditions not met, just call orig
			else
			{
				orig(self, x, y, pickPower);
			}
		}
	}
}
