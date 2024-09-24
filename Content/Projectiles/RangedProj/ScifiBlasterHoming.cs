using AssortedAdditions.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
    internal class ScifiBlasterHoming : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.light = 0.5f;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            float range = 600f; // Max radius that the projectile can detect a target
            float speed = 10f; // Projectile's speed

            // Loop through the sprite frames
            int frameSpeed = 5;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            // Leave a dust trail
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.GlowingMushroom, 0, 0, 150, default, 1f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.BlueTorch, 0, 0, 150, default, 2f);
                dust2.noGravity = true;
            }

            // Homing doesn't work instantly to avoid projectiles overlapping with the main projectile
            if (Projectile.timeLeft > 285)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
                return;
            }

            NPC closestNPC = HelperMethods.FindClosestNPC(Projectile.Center, range);
            if (closestNPC == null)
            {
                // Face towards where its going
                Projectile.rotation = Projectile.velocity.ToRotation();
                return;
            }

            // If found, change the velocity of the projectile and turn it in the direction of the target
            // Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
            Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
        }
    }
}
