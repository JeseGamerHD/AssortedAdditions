using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Items.Tools
{
    internal class Telelocator : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.scale = 1.5f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Lime;
            Item.UseSound = SoundID.Item8;
        }

        public override bool CanUseItem(Player player)
        {
            // Can only be used if no cooldown is active
            if (player.HasBuff(BuffID.ChaosState))
            {
                return false;
            }

			// Disable teleporting into the jungle temple early or into tiles
			int xCheck = (int)(Main.MouseWorld.X / 16f);
            int yCheck = (int)(Main.MouseWorld.Y / 16f);
			if ((Main.tile[xCheck, yCheck].WallType == WallID.LihzahrdBrickUnsafe && !NPC.downedPlantBoss && (Main.remixWorld || yCheck > Main.worldSurface)) || Collision.SolidCollision(Main.MouseWorld, player.width, player.height))
			{
				return false; 
			}

			// All good, teleport
			return true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer) // Important check
            {
                // Set the coordinates, in this case the mouse position
                Vector2 teleportHere = new(Main.MouseWorld.X, Main.MouseWorld.Y);
                player.Teleport(teleportHere, TeleportationStyleID.TeleportationPotion);
                NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, (float)player.whoAmI, teleportHere.X, teleportHere.Y, 1);
                player.AddBuff(BuffID.ChaosState, 240); // Cooldown    
            }

            return true;
        }
    }
}
