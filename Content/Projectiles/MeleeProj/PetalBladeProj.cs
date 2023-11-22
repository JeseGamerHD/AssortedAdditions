using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
    internal class PetalBladeProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.FlowerPowPetal);
            Projectile.aiStyle = -1;
        }

        public float setOnce
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {

            if (setOnce == 0)
            {
                Projectile.rotation -= MathHelper.ToRadians(45);
                setOnce = 1;
            }

            // Face its direction
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Gravity
            Projectile.velocity.Y = Projectile.velocity.Y + 0.15f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Pink, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
            }
        }
    }
}
