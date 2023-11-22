using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
    internal class StarryWandProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 50;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(1))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                        DustID.GoldCoin, 0, 0, 100, default, 3f);
                dust.noGravity = true;
                dust.noLight = true;
            }

            if (Main.rand.NextBool(3))
            {
                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                             DustID.YellowStarDust, 0, 0, 100, default, 2f);
                dust2.noGravity = true;
                dust2.noLight = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                            DustID.GoldCoin, 0, 0, 100, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
            }
        }
    }
}
