using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
    internal class DesertsFuryProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
        }

        float rotateSpeed = 0; // Used for accelerating rotation
        float delay = 28; // Used for accelerating sound
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Check if Player Dead
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Gradually speed up the spin
            if (rotateSpeed <= 0.2f)
            {
                rotateSpeed += 0.0015f;
            }

            // Weapon will spin as long as player keeps channeling
            if (player.channel)
            {
                Projectile.position = player.Center; // Needs to be adjusted below
                Projectile.position.Y -= 50;

                if (player.direction == -1) // If player faces left adjust accordingly
                {
                    Projectile.position.X -= 58;
                    Projectile.rotation -= rotateSpeed; // Rotation also changes depending on direction faced
                }
                else // If right, adjust differently
                {
                    Projectile.position.X -= 53;
                    Projectile.rotation += rotateSpeed;
                }

                // Used for swapping the weapon position if the player switches direction
                int switchSides = player.direction;

                // Weapon needs to "point" towards the cursor's X position
                if (Main.MouseWorld.X > player.Center.X)
                {
                    player.ChangeDir(1); // Change player to face said direction
                }
                else if (Main.MouseWorld.X < player.Center.X)
                {
                    player.ChangeDir(-1);
                }

                // Replaces the old projectile with a new one after switching direction
                // New projectile alligns properly
                if (switchSides != player.direction)
                {
                    Projectile.Kill();
                }

                // Makes player hold the weapon
                player.heldProj = Projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;

                // Dust
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Sandnado, 0, 0, 100, default, 2f);
                    dust.noGravity = true;

                    Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Sandstorm, 0, 0, 100, default, 2f);
                    dust2.noGravity = true;
                }

                if (delay > 14)
                {
                    delay -= 0.105f;
                }

                // Sound effect
                // Gets faster along with the spin
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = (int)delay; // This countsdown automatically
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
                }

            }
            else // When channeling stops, the projectile is destroyed
            {
                Projectile.Kill();
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index); // is drawn over the player's arm
        }
    }
}
