using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AssortedAdditions.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    internal class HangGlider : ModItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(1, -1, 1f);
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.SilverBar, 5);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.Silk, 20);
			recipe2.AddIngredient(ItemID.TungstenBar, 5);
			recipe2.AddTile(TileID.Sawmill);
			recipe2.Register();
		}

		private int Timer = 0; // For smoothly rotating the player to glide position
        private int landingTimer = 90; // For smoothly rotating the player back to normal position during landing

        public override bool WingUpdate(Player player, bool inUse)
        {
            // Player is gliding
            if (player.wingsLogic > 0 && player.controlJump && player.wingTime == 0 && player.velocity.Y != 0 && player.jump == 0)
            {
                if(Timer == 0)
                {
					SoundStyle deploySound = new SoundStyle("AssortedAdditions/Assets/Sounds/Misc/HangGliderDeploy");
					SoundEngine.PlaySound(deploySound, player.position);
				}

                landingTimer = 90;
                if(Timer != 90)
                {
                    Timer += 5;
                }
                
                // Player can't use items during a glide
                // since the player will keep flipping weirdly
                player.GetModPlayer<GlidingPlayer>().playerIsGliding = true; 

                // Smoothly rotate until rotation is 90 degrees
                if(player.fullRotation != player.fullRotation + player.direction * MathHelper.PiOver2)
                {
                    player.fullRotation += player.direction * MathHelper.ToRadians(Timer);
                }
                else // Otherwise stay at 90 degrees and lean towards the direction of travel
                {
                    player.fullRotation += player.direction * MathHelper.PiOver2;
                    player.fullRotation += player.velocity.X * 0.01f;
                }
            }
            // Player is falling down without gliding while item is equipped
            else if (player.wingsLogic > 0 && !player.controlJump && player.wingTime == 0 && player.velocity.Y != 0)
            {
                landingTimer = 90;
                if (Timer != 90)
                {
                    Timer += 5;
                }

                if (player.fullRotation != player.fullRotation + player.direction * MathHelper.PiOver2)
                {
                    player.fullRotation += player.direction * MathHelper.ToRadians(Timer);
                }
                else
                {
                    player.fullRotation += player.direction * MathHelper.PiOver2;
                    player.fullRotation += player.velocity.X * 0.01f;
                }

                player.GetModPlayer<GlidingPlayer>().playerIsGliding = true; // keep items disabled, use looks weird 
            }
            else
            {
                Timer = 0;
                if (landingTimer != 0)
                {
                    player.fullRotation += player.direction * MathHelper.ToRadians(landingTimer);
                    landingTimer -= 10;
                    player.GetModPlayer<GlidingPlayer>().playerIsGliding = true; // Still buggy, keep true
                }
                else
                {
                    player.fullRotation = default;
                    // Player has landed smoothly, playerIsGliding gets reset in the ModPlayer
                }
            }

            return false;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 12f;
        }
    }

    public class GlidingPlayer : ModPlayer
    {
        public bool playerIsGliding;

        public override void ResetEffects()
        {
            playerIsGliding = false;
        }
    }

    public class GlideDisableWeapons : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if (player.GetModPlayer<GlidingPlayer>().playerIsGliding)
            {
                return false;
            }

            return base.CanUseItem(item, player);
        }
    }
}
