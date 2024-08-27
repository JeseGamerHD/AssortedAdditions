using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using System;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
    internal class CosmicTomeProj : ModProjectile
    {
        // Re-use the shooting star sprite, visual difference comes in shader
        public override string Texture => "AssortedAdditions/Content/Projectiles/RangedProj/ShootingStarProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 45;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.light = 1f;
            Projectile.alpha = 50;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Magic;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            // Setting the velocity is handled in the shoot() of CosmicTome.cs

            Timer++;
            Projectile.rotation = MathHelper.ToRadians(Timer * 5f); // Rotates the star smoothly

            // Leave a dust trail
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.CorruptTorch, 0, 0, 150, default, 1f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.PlatinumCoin, 0, 0, 150, default, 2f);
                dust2.noGravity = true;
            }

            // ai[1] set by the shoot method
			Projectile.tileCollide = Projectile.Bottom.Y >= Projectile.ai[1];
		}

		public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item10);
        }

        // The cool trail effect happens under here:

        private static readonly VertexStrip _vertexStrip = new();

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return true;
        }

        private Color StripColors(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.HotPink, Color.Indigo, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = 0;

            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float num = 3f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 32f, num);
        }
    }
}
