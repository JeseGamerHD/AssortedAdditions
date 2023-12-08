using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AssortedAdditions.Content.Items.Accessories
{
    internal class MimicsTongue : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MimicTonguePlayer>().isWearingMimicsTongue = true;
            player.moveSpeed += 0.1f;
        }
    }

    // Dashing part is taken from example mod with slight modifications.
    // For more detailed comments refer to
    // https://github.com/tModLoader/tModLoader/blob/3d21ca363d71c972286e9883b3014872c87ec2ef/ExampleMod/Items/ExampleDashAccessory.cs
    public class MimicTonguePlayer : ModPlayer
    {
        public bool isWearingMimicsTongue; // Flag for accessory

        private const int DashRight = 0;
        private const int DashLeft = 1;
        private const int DashUp = 2;
        private const int DashDown = 3;

        private const int DashCooldown = 70; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        private const int DashDuration = 35; // Duration of the dash afterimage effect in frames

        private const float DashVelocity = 13f; // The initial velocity.
        private int DashDir = -1; // The direction the player has double tapped. Defaults to -1 for no dash double tap
        private int DashDelay = 0; // frames remaining till we can dash again
        private int DashTimer = 0; // frames remaining in the dash

        public override void PreUpdateMovement()
        {
            if (CanDash() && DashDir != -1 && DashDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;
                float dashDirection;

                // Adjust velocity accordingly depending on if the player is moving left or right
                // or up/down
                if (DashDir == 0 && Player.velocity.X < DashVelocity) // Right
                {
                    dashDirection = DashDir == DashRight ? 1 : -1;
                    newVelocity.X = dashDirection * DashVelocity;
                }
                else if (DashDir == 1 && Player.velocity.X > -DashVelocity) // Left
                {
                    dashDirection = DashDir == DashLeft ? -1 : 1;
                    newVelocity.X = dashDirection * DashVelocity;
                }
                else if (DashDir == 2 && Player.velocity.Y > -DashVelocity) // Up
                {
                    dashDirection = DashDir == DashUp ? -1 : 1.3f; // 1.3 to account for gravity
                    newVelocity.Y = dashDirection * DashVelocity;
                }
                else if (DashDir == 3 && Player.velocity.Y < DashVelocity) // Down
                {
                    dashDirection = DashDir == DashDown ? 1 : -1.3f;
                    newVelocity.Y = dashDirection * DashVelocity;
                }
                else
                {
                    return; // Else cant dash
                }

                DashDelay = DashCooldown;
                DashTimer = DashDuration;
                Player.velocity = newVelocity;

                // Play a sound effect
                SoundStyle dashSound = new SoundStyle("AssortedAdditions/Assets/Sounds/Misc/MimicsTongueDash");
                SoundEngine.PlaySound(dashSound, Player.position);
            }

            if (DashDelay > 0)
            {
                DashDelay--;
            }

            if (DashTimer > 0)
            {
                // TODO figure out how to sync the visual effect
                Player.eocDash = DashTimer;
                Player.armorEffectDrawShadowEOCShield = true;
                DashTimer--;
            }
        }

		private bool CanDash()
        {
            return isWearingMimicsTongue
                && Player.dashType == 0
                && !Player.setSolar
                && !Player.mount.Active;
        }

		public override void ResetEffects()
        {
            isWearingMimicsTongue = false;

            // [0] = double tap down
            // [1] = double tap up
            // [2] = double tap right
            // [3] = double tap left

            if(Main.myPlayer == Player.whoAmI)
            {
				if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15)
				{
					DashDir = DashRight;
				}
				else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15)
				{
					DashDir = DashLeft;
				}
				else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[1] < 15)
				{
					DashDir = DashUp;
				}
				else if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[0] < 15)
				{
					DashDir = DashDown;
				}
				else
				{
					DashDir = -1; // No dash
				}
			}
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            // Accessory also grants a 1/10 chance to dodge an attack
            if (isWearingMimicsTongue)
            {
                if (Main.rand.NextBool(10))
                {
                    Player.immune = true;
                    Player.immuneTime = 60;
                    Player.immuneNoBlink = false;

                    SoundStyle dodgeSound = new SoundStyle("AssortedAdditions/Assets/Sounds/Misc/MimicsTongueDodge");
                    SoundEngine.PlaySound(dodgeSound, Player.position);

                    return true;
                }
                else
                {
                    return base.FreeDodge(info);
                }
            }

            return base.FreeDodge(info);
        }
    }
}
