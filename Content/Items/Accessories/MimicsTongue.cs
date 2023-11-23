using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AssortedAdditions.Content.Items.Accessories
{
    internal class MimicsTongue : ModItem // TODO other effects, chance to dodge ??
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

    public class MimicTonguePlayer : ModPlayer
    {
        public bool isWearingMimicsTongue; // Flag for accessory

        public const int DashRight = 0;
        public const int DashLeft = 1;
        public const int DashCooldown = 70; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
        public const int DashDuration = 35; // Duration of the dash afterimage effect in frames
        public const float DashVelocity = 13f; // The initial velocity.
        public int DashDir = -1; // The direction the player has double tapped. Defaults to -1 for no dash double tap
        public int DashDelay = 0; // frames remaining till we can dash again
        public int DashTimer = 0; // frames remaining in the dash

        public override void PreUpdateMovement()
        {
            if (CanDash() && DashDir != -1 && DashDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                // Adjust velocity accordingly depending on if the player is moving left or right
                if (DashDir == 0 && Player.velocity.X < DashVelocity) // Right
                {
                    float dashDirection = DashDir == DashRight ? 1 : -1;
                    newVelocity.X = dashDirection * DashVelocity;
                }
                else if (DashDir == 1 && Player.velocity.X > -DashVelocity) // Left
                {
                    float dashDirection = DashDir == DashLeft ? -1 : 1;
                    newVelocity.X = dashDirection * DashVelocity;
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

        public override bool FreeDodge(Player.HurtInfo info)
        {
            // Accessory also grants a 1/10 chance to dodge an attack
            if(isWearingMimicsTongue)
            {
                if(Main.rand.NextBool(10))
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

        public override void ResetEffects()
        {
            isWearingMimicsTongue = false;

            // [0] = double tap down
            // [1] = double tap up
            // [2] = double tap right
            // [3] = double tap left
            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15)
            {
                DashDir = DashRight;
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15)
            {
                DashDir = DashLeft;
            }
            else
            {
                DashDir = -1;
            }
        }
    }
}
