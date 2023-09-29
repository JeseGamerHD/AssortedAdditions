using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Drawing;

namespace ModdingTutorial.Content.Projectiles.MagicProj
{
    internal class TrueTopazStaffProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 18;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 180;
            Projectile.light = 0.5f;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation(); // Rotation set to face direction

            // Add some lighting
            Lighting.AddLight(Projectile.Center, TorchID.Yellow);

            // Leaves behind a bunch of dust
            if (Main.rand.NextBool(1))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                        DustID.GemTopaz, 0, 0, 150, default, 1f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.YellowTorch, 0, 0, 150, default, 1f);
                dust2.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Creates cool sparkly effect when enemy is hit, vanilla has many effects -> ParticleOrchestraType.Effect
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.SilverBulletSparkle,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.SilverBulletSparkle,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);
        }

        public override void Kill(int timeLeft)
        {
            // On death create dust explosion
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.GemTopaz, 0, 0, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity.Y *= 1f;
                dust.velocity *= 2f;

                dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.YellowTorch, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }
        }
    }
}
