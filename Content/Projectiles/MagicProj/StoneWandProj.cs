using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
    internal class StoneWandProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 300;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                        DustID.Stone, 0, 0, 0, default, 1f);
                dust.noGravity = true;
                dust.noLight = true;
            }

            // Face where its going
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Slow down
            Projectile.velocity *= 0.98f;

            // After a bit fall down faster
            if (Projectile.timeLeft <= 270)
            {
                Projectile.velocity.Y += 0.25f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); // Dust from tile when hit
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center); // Tile hit sound
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                            DustID.Stone, 0, 0, 0, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
            }
        }
    }
}
