using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Tiles.CraftingStations;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	[AutoloadEquip(EquipType.Wings)]
	internal class RuneOfFlight : RuneItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(12, 4, true));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(160, 10f, 1f);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(gold: 6);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.controlJump)
			{
				int offSet = player.direction == 1 ? -20 : 20;
				Vector2 position = new Vector2(player.position.X + offSet, player.position.Y);
				if (Main.rand.NextBool(4))
				{
					Dust dust = Dust.NewDustDirect(position, player.width, player.height, DustID.MushroomTorch, 1f, 1f, 200, default, 1.75f);
					dust.noGravity = true;
				}

				if (Main.rand.NextBool(4))
				{
					Dust dust = Dust.NewDustDirect(player.position - player.velocity, player.width, player.height, DustID.Cloud, Scale: 1.2f);
					dust.noGravity = true;
				}
			}

			player.GetModPlayer<RuneOfFlightPlayer>().isWearingRuneOfFlight = true;
		}

		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			if(equippedItem.wingSlot == 0)
			{
				return false;
			}

			return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BlankRune>());
			recipe.AddIngredient(ModContent.ItemType<MagicEssence>(), 10);
			recipe.AddIngredient(ItemID.SoulofFlight, 20);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}
	}

	public class RuneOfFlightPlayer : ModPlayer
	{
		public bool isWearingRuneOfFlight;

		public override void ResetEffects()
		{
			isWearingRuneOfFlight = false;
		}
	}

	public class StopWingsFromBeingEquippedWithRuneOfFlight : GlobalItem
	{
		public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.wingSlot > 0 && entity.type != ModContent.ItemType<RuneOfFlight>();

		public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
		{
			
			if (player.GetModPlayer<RuneOfFlightPlayer>().isWearingRuneOfFlight)
			{
				return false;
			}

			return base.CanEquipAccessory(item, player, slot, modded);
		}

	}
}
