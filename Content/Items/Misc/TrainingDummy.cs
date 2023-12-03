using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.NPCs;
using Terraria.Audio;

namespace AssortedAdditions.Content.Items.Misc
{
	internal class TrainingDummy : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.damage = 0;
			
			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.sellPrice(silver: 2);
			Item.rare = ItemRarityID.Blue;
			Item.useStyle = ItemUseStyleID.Swing;

			Item.useTurn = true;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player)
		{
			bool canSpawn = false;

            if (player.whoAmI == Main.myPlayer)
			{
				// If no dummy exists, can spawn
				if (!NPC.AnyNPCs(ModContent.NPCType<TrainingDummyNPC>()))
				{
					canSpawn = true;
				}
				else
				{
					for (int i = 0; i < Main.npc.Length; i++)
					{
						// If a dummy exists, but it is spawned by another player, can spawn
						if (Main.npc[i].type == ModContent.NPCType<TrainingDummyNPC>() && Main.npc[i].ai[0] != player.whoAmI)
						{
							canSpawn = true;
						}
						// If a dummy spawned by the player exists, teleport it to the location don't spawn a new one
						else if (Main.npc[i].type == ModContent.NPCType<TrainingDummyNPC>() && Main.npc[i].ai[0] == player.whoAmI)
						{
							canSpawn = false;
							Main.npc[i].position = Main.MouseWorld;
							SoundEngine.PlaySound(SoundID.Item8, player.position);
							break; // Found our old dummy, can safely break
						}
					}
				}

				// Spawn the dummy if it should be spawned
				// Also needs to be synced
				if (canSpawn)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						SoundEngine.PlaySound(SoundID.Item8, player.position);
						int index = NPC.NewNPC(player.GetSource_ItemUse(Item), (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, ModContent.NPCType<TrainingDummyNPC>(), 0, player.whoAmI);
						NetMessage.SendData(MessageID.SyncNPC, number: index);
					}
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.WoodenSword, 1);
			recipe.AddIngredient(ItemID.TargetDummy, 1);
			recipe.AddRecipeGroup("IronBar", 2);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
}
