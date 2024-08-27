using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
    internal class DraconicBowProj : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 15;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 22;

            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.light = 1f;
            Projectile.alpha = 50;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.aiStyle = 0;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Timer++;

            // Face towards where its going
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Leave a dust trail
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.Flare, 0, 0, 150, default, 3f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.FlameBurst, 0, 0, 150, default, 1f);
                dust2.noGravity = true;
            }

            // Projectile fades in/out a little
            if (Timer <= 60)
            {
                Projectile.alpha++;
            }

            if (Timer >= 60 && Timer <= 120)
            {
                Projectile.alpha--;
            }

            if (Timer >= 120)
            {
                Timer = 0;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 120);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.position);
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Flare, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
        }

		public override bool PreDraw(ref Color lightColor)
		{
			VertexStrip vertexStrip = new();
			MiscShaderData miscShaderData = GameShaders.Misc["FlameLash"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(4f);
			miscShaderData.Apply();
			vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2, true);
			vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();

			return true;
		}

		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.Orange, Color.Brown, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: false)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 1f;
			float lerpValue = Utils.GetLerpValue(0.1f, 1.5f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}
	}
}
