using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.NPCs.BossTheHaunt;
using Terraria.Audio;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Tiles.CraftingStations;

namespace AssortedAdditions.Content.Items.Consumables
{
	internal class GraveFlowers : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 42;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Pink;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false; // Normally would be, but I want to make boss summon items non consumable
		}

		public override bool CanUseItem(Player player)
		{
			// Checks if boss already is active and for additional conditions
			return !NPC.AnyNPCs(ModContent.NPCType<TheHaunt>()) && player.ZoneGraveyard && Condition.DownedMechBossAny.IsMet();
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				// The roaring sound when using summon items
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				// Spawns the head which spawns the rest
				int type = ModContent.NPCType<TheHaunt>();

				// Needed so multiplayer works
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.JungleRose, 4);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddIngredient(ModContent.ItemType<MagicEssence>());
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}
	}
}
