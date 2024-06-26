﻿using AssortedAdditions.Common.Systems;
using AssortedAdditions.Content.Tiles.CraftingStations;
using Microsoft.Xna.Framework;
using AssortedAdditions.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Misc
{
    internal class MysteriousKey : ModItem
    {
        public override void SetStaticDefaults()
        {

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GoldenKey);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item8;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
					Vector2 roomPos = new((Main.dungeonX + 3) * 16, (Main.dungeonY + 16) * 16);
					player.Teleport(roomPos, TeleportationStyleID.TeleportationPotion);
				}
                else
                {
					var message = Mod.GetPacket();
                    message.Write((byte)AssortedAdditions.MessageType.TeleportDungeon);
                    message.Send();
				}
                
                SoundEngine.PlaySound(SoundID.Item6, player.position);
            }

			// After this the wizard will begin to sell items from the mysterious chest
			// Otherwise items would be obtainable only once per world 
			ModContent.GetInstance<WorldUnlocks>().mysteriousKeyWasUsed = true;

			return true;
        }

        public static void TeleportInMultiplayer(int playerIndex)
        {
			Vector2 roomPos = new((Main.dungeonX + 3) * 16, (Main.dungeonY + 16) * 16);
            Player player = Main.player[playerIndex];
			player.Teleport(roomPos, TeleportationStyleID.TeleportationPotion);
            player.velocity = Vector2.Zero;
			NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, player.position.X, player.position.Y, 1);
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofMight, 15);
            recipe.AddIngredient(ItemID.SoulofSight, 15);
            recipe.AddIngredient(ItemID.SoulofFright, 15);
            recipe.AddIngredient(ItemID.ShadowKey, 1);
            recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            recipe.Register();
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Purple.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }
    }
}
