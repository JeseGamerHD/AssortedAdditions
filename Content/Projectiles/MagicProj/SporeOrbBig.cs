using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
    internal class SporeOrbBig : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;

            // For shader trail
            ProjectileID.Sets.TrailCacheLength[Type] = 28;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 34;
            Projectile.aiStyle = 0;
            Projectile.light = 0.25f;
            Projectile.penetrate = 3;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // Loop through animation
            int frameSpeed = 5;
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

            if (Main.rand.NextBool())
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.JungleSpore, 5, 5, 150, default, 1.5f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.JungleTorch, 5, 5, 150, default, 1.5f);
                dust2.noGravity = true;
            }

            // When the projectile hits something it slows down to hit the target up to 3 times
            // If the target dies or the projectile passes the target, then kill it
            if (Projectile.ai[0] != 0)
            {
                if (!Projectile.Hitbox.Intersects(Main.npc[TargetBeingHit].Hitbox)
                    || !Main.npc[TargetBeingHit].active)
                {
                    Projectile.Kill();
                }
            }
        }

        public int TargetBeingHit
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] == 0)
            {
                Projectile.velocity *= 0.1f;
                Projectile.ai[2]++;
                Projectile.timeLeft = 75;

                TargetBeingHit = target.whoAmI;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.ai[2] == 0)
            {
                Projectile.velocity *= 0.1f;
                Projectile.ai[2]++;
                Projectile.timeLeft = 75;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.JungleSpore, Projectile.velocity.X, Projectile.velocity.Y, 150, default, 1.5f);
                dust.noGravity = true;
            }

            int amount = Main.rand.Next(1, 5);
            for (int i = 0; i <= amount; i++)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Vector2 launchVelocity = new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-8, -4));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity, ModContent.ProjectileType<SporeOrbSmall>(), 15, 2, Projectile.owner);
                }
            }

            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
        }

        private static readonly VertexStrip _vertexStrip = new();

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2, true);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return true;
        }

        private Color StripColors(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Gold, Color.YellowGreen, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = 0;
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float num = 2f;
            float lerpValue = Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(16f, 32f, num);
        }
    }
}
