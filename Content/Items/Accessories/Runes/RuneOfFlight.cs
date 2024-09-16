using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Tiles.CraftingStations;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	[AutoloadEquip(EquipType.Wings)]
	internal class RuneOfFlight : RuneItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(12, 4, true));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150, 11f, 1.8f, true, 13f, 2.5f);
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.value = Item.sellPrice(gold: 6);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BlankRune>());
			recipe.AddIngredient(ModContent.ItemType<MagicEssence>());
			recipe.AddIngredient(ItemID.SoulofFlight, 20);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			player.wingsLogic = 22; // The wings will behave like the hover board
			// Unsure if there is another way to make hover wings easily

			// Only draw and animate the sprite if the player is flying/falling etc
			if (player.wingTime != 150 || player.velocity.Y != 0)
			{
				player.wingFrameCounter++;
				if (player.wingFrameCounter > 12) // How many frames to spend on each frame
				{
					
					player.wingFrame++;
					player.wingFrameCounter = 0;
					if (player.wingFrame > 3)
					{
						player.wingFrame = 1;
					}
					SoundEngine.PlaySound(SoundID.Item24 with { MaxInstances = 1, Pitch = 0.2f }, player.position);
				}

				Lighting.AddLight(new Vector2(player.Center.X, player.Center.Y + 20), TorchID.Blue);

				if (Main.rand.NextBool(2))
				{
					Dust.NewDustPerfect(new Vector2(player.Center.X, player.Center.Y + 25) - player.velocity, 132, Scale: 0.85f, newColor: Color.SkyBlue);
				}
			}
			else // Frame 0 is empty, it displays when not flying/falling
			{
				player.wingFrame = 0;
				player.wingFrameCounter = 0;
			}

			return true;
		}
	}
}
