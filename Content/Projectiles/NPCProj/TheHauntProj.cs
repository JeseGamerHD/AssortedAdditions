using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace AssortedAdditions.Content.Projectiles.NPCProj
{
	internal class TheHauntProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 35;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 600;
			Projectile.alpha = 100;

			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

			Projectile.aiStyle = 0;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public float SineTimer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		private Vector2 initialCenter;

		// For setting the frame
		private bool setInitialValues;

		public override void AI()
		{
			if (!setInitialValues)
			{
				initialCenter = Projectile.Center;
				setInitialValues = true;
			}
			Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
			

			SineTimer++;
			float sine = (float)Math.Sin(MathHelper.ToRadians(SineTimer * 3f));

			Vector2 offset = Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.PiOver2);
			float amplitude = 100f;
			offset *= sine *= amplitude;

			initialCenter += Projectile.velocity;
			Projectile.Center = initialCenter + offset;

			int frameSpeed = 10;
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
		}

		public override void PostAI()
		{
			Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X + 8, Projectile.Center.Y), DustID.WhiteTorch, null, 100, default, Main.rand.NextFloat(1.5f, 2.1f));
			dust.noGravity = true;

			Dust dust2 = Dust.NewDustPerfect(new Vector2(Projectile.Center.X - 8, Projectile.Center.Y), DustID.WhiteTorch, null, 100, default, Main.rand.NextFloat(1.5f, 2.1f));
			dust2.noGravity = true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Smoke, 1, 1, 100, default, 1.25f);
				dust.noGravity = true;
				dust.velocity *= 1.2f;

				Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Cloud, 1, 1, 100, default, 1.5f);
				dust2.noGravity = true;
				dust.velocity *= 1.4f;
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
