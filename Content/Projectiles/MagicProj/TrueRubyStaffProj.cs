﻿using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.MagicProj
{
    internal class TrueRubyStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 126;
            Projectile.height = 64;
            Projectile.penetrate = 2;
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

            // Loop through the sprite frames
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

            // Add some lighting
            Lighting.AddLight(Projectile.Center, TorchID.Red);

            // Leaves behind a bunch of dust
            if (Main.rand.NextBool(1))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                        DustID.GemRuby, 0, 0, 150, default, 1.5f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.Fireworks, 0, 0, 150, default, 0.5f);
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

        public override void OnKill(int timeLeft)
        {
            // On death create dust explosion
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.GemRuby, 0, 0, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity.Y *= 1f;
                dust.velocity *= 5f;

                dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.Fireworks, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }
        }
    }
}
