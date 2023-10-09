using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.NPCProj
{
    internal class IceGuardianProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;

            Projectile.timeLeft = 180;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.light = 0.5f;
            Projectile.alpha = 75;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Frost, 0, 0, 50, default, 1f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.Frost, 0, 0, 150, default, 1f);
                dust2.noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn2, 240);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.Frost, 0, 0, 150, default, 1.5f);
                dust.noGravity = true;
                dust.velocity *= 3f;
            }
        }
    }
}
