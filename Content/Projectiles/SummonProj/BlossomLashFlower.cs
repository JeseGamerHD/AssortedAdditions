using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.SummonProj
{
    internal class BlossomLashFlower : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.alpha = 0;
            Projectile.aiStyle = 0;
            Projectile.light = 0.1f;
            Projectile.timeLeft = 255;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Projectile.alpha++;
            Projectile.rotation += MathHelper.ToRadians(1);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Pink, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Plantera_Pink, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
            }
        }
    }
}
