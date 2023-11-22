using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
    internal class SporeOrbSmall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 12;
            Projectile.scale = 1.5f;
            Projectile.light = 0.5f;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;

            Projectile.DamageType = DamageClass.Magic;
            AIType = ProjectileID.SpikyBall;

            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // Loop through animation
            int frameSpeed = 3;
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
            Projectile.spriteDirection = Projectile.direction; // Faces left/right correctly

            // Basic gravity
            Projectile.velocity.Y = Projectile.velocity.Y + 0.18f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }

            Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.JungleSpore, 0, 0, 150, default, 1f);
            dust.noGravity = true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.timeLeft >= 570)
            {
                return false;
            }

            return base.CanHitNPC(target);
        }

        public override bool CanHitPvp(Player target)
        {
            if (Projectile.timeLeft >= 570)
            {
                return false;
            }

            return base.CanHitPvp(target);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.JungleSpore, 0, 0, 150, default, 1f);
                dust.noGravity = true;
            }
        }
    }
}
