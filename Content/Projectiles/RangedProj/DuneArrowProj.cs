using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.RangedProj
{
    internal class DuneArrowProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.damage = 0;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() +  MathHelper.PiOver2;

            // Basic arrow gravity
            Projectile.velocity.Y = Projectile.velocity.Y + 0.05f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); // Dust from tile when hit
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center); // Tile hit sound
            return true;
        }
    }
}
