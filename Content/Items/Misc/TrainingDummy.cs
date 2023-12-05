using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.NPCs;
using Terraria.Audio;

namespace AssortedAdditions.Content.Items.Misc
{
	public class TrainingDummy : ModItem
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

		// This code is maybe more messy than it needs to be
		// Now it seems to work in multiplayer so I don't want to touch it
		public override bool? UseItem(Player player)
		{
			bool canSpawn = false;

			if (Main.myPlayer == player.whoAmI)
			{
				// If no dummy exists, can spawn
				if (!NPC.AnyNPCs(ModContent.NPCType<TrainingDummyNPC>()))
				{
					canSpawn = true;
				}
				else
				{
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						// If a dummy exists, but it is spawned by another player, can spawn
						if (Main.npc[i].type == ModContent.NPCType<TrainingDummyNPC>() && Main.npc[i].ai[0] != player.whoAmI)
						{
							canSpawn = true;
						}
						// If a dummy spawned by the player exists, delete it and spawn a new one
						else if (Main.npc[i].type == ModContent.NPCType<TrainingDummyNPC>() && Main.npc[i].ai[0] == player.whoAmI)
						{

							if(Main.netMode == NetmodeID.SinglePlayer)
							{
								DespawnDummy(i);
							}
							else
							{
								var message = Mod.GetPacket(); // Tell server to remove the dummy
								message.Write((byte)AssortedAdditions.MessageType.DespawnTrainingDummy);
								message.Write(i);
								message.Send();
							}

							canSpawn = true;
							break; // Found our old dummy and removed it, end loop
						}
					}
				}
			}

			// Spawn the dummy if it should be spawned
			// Also needs to be synced
			if (canSpawn && player.whoAmI == Main.myPlayer)
			{
				int xPos = (int)Main.MouseWorld.X;
				int yPos = (int)Main.MouseWorld.Y;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(SoundID.Item8, player.position);
					NPC.NewNPC(player.GetSource_ItemUse(Item), xPos, yPos, ModContent.NPCType<TrainingDummyNPC>(), 0, player.whoAmI);
				}
				else
				{
					var message = Mod.GetPacket(); // Send a message to the server telling that it should spawn a dummy
					message.Write((byte)AssortedAdditions.MessageType.SpawmTrainingDummy);
					message.Write(xPos);
					message.Write(yPos);
					message.Write(player.whoAmI);
					message.Send();
				}
			}

			return true;
		}

		public static void DespawnDummy(int index)
		{
			Main.npc[index].life = 0;
			Main.npc[index].active = false;

			if(Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.SyncNPC, number: index);
			}
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
