using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.MagicProj
{
    internal class IcyWandProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.scale = 0.5f;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }

        private bool setOnce = false;
        public override void AI()
        {
            // Set the frame to 1 of 3 possible ones to create some variation in the projectiles
            // Also randomize rotation
            if (!setOnce)
            {
                int frame = Main.rand.Next(0, 3);
                switch (frame)
                {
                    case 0:
                        Projectile.frame = 0;
                        Projectile.rotation = frame;
                        break;
                    case 1:
                        Projectile.frame = 1;
                        Projectile.rotation = frame;
                        break;
                    case 2:
                        Projectile.frame = 2;
                        Projectile.rotation = frame;
                        break;
                }
                setOnce = true;
            }

            // Dust stuff
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                        DustID.SnowSpray, 0, 0, 100, default, 0.5f);
                dust.noGravity = false;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 180);
        }
    }
}
