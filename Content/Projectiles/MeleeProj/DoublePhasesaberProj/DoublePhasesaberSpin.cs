using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using AssortedAdditions.Content.Items.Weapons.Melee.Sabers;

namespace AssortedAdditions.Content.Projectiles.MeleeProj.DoublePhasesaberProj
{
    internal class DoublePhasesaberSpin : ModProjectile // This handles all the double phasesabers, projectile sprite will be swapped depending on which Item is used
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7; // 7 different colors, AI will pick the right one
        }

        public override void SetDefaults()
        {
            Projectile.width = 112;
            Projectile.height = 112;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
			
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
			Projectile.friendly = true;
            Projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

            if (Main.myPlayer == Projectile.owner)
            {
                if (player.channel)
                {
                    float holdoutDistance = player.HeldItem.shootSpeed * Projectile.scale;
                    Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);

                    if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }

                    Projectile.velocity = holdoutOffset;
                }
                else
                {
                    Projectile.Kill();
                }
            }

            if (Projectile.velocity.X > 0f)
            {
                Projectile.rotation += 0.3f;
                player.ChangeDir(1);
            }
            else if (Projectile.velocity.X < 0f)
            {
				Projectile.rotation -= 0.3f;
				player.ChangeDir(-1);
            }

            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);
            Projectile.Center = playerCenter;
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

			// The color of the sprite is determined in a custom method
			Visuals();

            // Sound effect
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 9; // This countsdown automatically
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index); // Saber is drawn over the player's arm
        }

        // The color of the sprite will be chosen here
        private void Visuals()
        {
            Player player = Main.player[Projectile.owner];
            int itemInUse = player.inventory[player.selectedItem].type; // The item the player is currently using

            // Each color has its own ModItem so the frame can be selected based on that
            // Emitted light color is also selected here
            if (itemInUse == ModContent.ItemType<YellowDoublePhasesaber>())
            {
                Projectile.frame = 0;
                Lighting.AddLight(player.Center, TorchID.Yellow);
            }

            else if (itemInUse == ModContent.ItemType<OrangeDoublePhasesaber>())
            {
                Projectile.frame = 1;
                Lighting.AddLight(player.Center, TorchID.Orange);
            }

            else if (itemInUse == ModContent.ItemType<GreenDoublePhasesaber>())
            {
                Projectile.frame = 2;
                Lighting.AddLight(player.Center, TorchID.Green);
            }

            else if (itemInUse == ModContent.ItemType<BlueDoublePhasesaber>())
            {
                Projectile.frame = 3;
                Lighting.AddLight(player.Center, TorchID.Blue);
            }

            else if (itemInUse == ModContent.ItemType<PurpleDoublePhasesaber>())
            {
                Projectile.frame = 4;
                Lighting.AddLight(player.Center, TorchID.Purple);
            }

            else if (itemInUse == ModContent.ItemType<RedDoublePhasesaber>())
            {
                Projectile.frame = 5;
                Lighting.AddLight(player.Center, TorchID.Red);
            }
            else if (itemInUse == ModContent.ItemType<WhiteDoublePhasesaber>())
            {
                Projectile.frame = 6;
                Lighting.AddLight(player.Center, TorchID.White);
            }
        }
    }
}
