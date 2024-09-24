using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedAdditions.Helpers;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
	internal class PhantasmicGauntletProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 2;

			ProjectileID.Sets.TrailCacheLength[Type] = 20;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.timeLeft = 180;

			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

			Projectile.DamageType = DamageClass.Magic;

			Projectile.aiStyle = 0;
		}

		public ref float Timer => ref Projectile.ai[0];

		public override void AI()
		{
			NPC target = HelperMethods.FindClosestNPC(Projectile.Center, 1000f);

			Timer++;
			if (Timer < 120f && Timer > 30f && target != null)
			{
				float speed = Projectile.velocity.Length();
				Vector2 direction = target.Center - Projectile.Center;
				direction.Normalize();
				direction *= speed;

				Projectile.velocity = (Projectile.velocity * 10f + direction) / 15f;
				Projectile.velocity.Normalize();
				Projectile.velocity *= speed;
			}

			if (Projectile.velocity.Length() < 12f)
			{
				Projectile.velocity *= 1.03f;
			}

			int frameSpeed = 15;
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

			Projectile.spriteDirection = Projectile.direction;
			if (Projectile.direction < 0)
			{
				Projectile.rotation = (float)Math.Atan2(0f - Projectile.velocity.Y, 0f - Projectile.velocity.X);
			}
			else
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			}
		}

		public override void PostAI()
		{
			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.WhiteTorch, 0, 0, 100, default, Main.rand.NextFloat(1.25f, 2f));
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.WhiteTorch, 1, 1, 100, default, 1.25f);
				dust.noGravity = true;
				dust.velocity *= 1.2f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}


		public override bool PreDraw(ref Color lightColor)
		{
			VertexStrip vertexStrip = new();
			MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
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
			Color result = Color.Lerp(Color.Black, Color.Gray, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: false)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 255;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 1.3f;
			float lerpValue = Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 36f, num);
		}
	}
}
