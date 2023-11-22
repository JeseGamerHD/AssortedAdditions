using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Projectiles.NPCProj
{
    internal class GoblinBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 30;
            Projectile.timeLeft = 300;
            Projectile.light = 0.1f;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.hostile = true;

            Projectile.aiStyle = 0;

            DrawOffsetX = -2;
            DrawOriginOffsetY = -5;
        }

        public override bool CanHitPlayer(Player target)
        {
            // Only explosion will deal damage
            if (Projectile.timeLeft > 3)
            {
                return false;
            }

            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false; // Stays on the ground
        }

        public override void AI()
        {
            // Fall downwards
            Projectile.velocity.Y += 0.25f;

            // When close to dying, explosion
            if (Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                Projectile.alpha = 255;
                Projectile.knockBack = 10f;
                ExplosionEffect();
            }
            else // Roll a little
            {
                Projectile.velocity.X *= 0.96f;
                Projectile.rotation += Projectile.velocity.X * 0.1f;
            }

            // Fuse smoke
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                        DustID.Smoke, 0, 0, 0, Color.DarkGray, 1f);
                dust.velocity.Y -= 0.12f;
            }

            if (Projectile.timeLeft % 30 == 0)
            {
                // Sound effect
                SoundEngine.PlaySound(SoundID.Item13 with { Volume = 0.8f, MaxInstances = 1 }, Projectile.position);
            }
        }

        private void ExplosionEffect()
        {
            int explosionArea = 80;
            Vector2 oldSize = Projectile.Size;
            // Resize the projectile hitbox to be bigger.
            Projectile.position = Projectile.Center;
            Projectile.Size += new Vector2(explosionArea);
            Projectile.Center = Projectile.position;

            Projectile.tileCollide = false;
            Projectile.velocity *= 0.01f;
            // Damage enemies inside the hitbox area
            Projectile.Damage();

            //Resize the hitbox to its original size
            Projectile.position = Projectile.Center;
            Projectile.Size = new Vector2(10);
            Projectile.Center = Projectile.position;

            // Sound effect
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // Smoke Dust
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
            }

            // Fire Dust
            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }
        }
    }
}
