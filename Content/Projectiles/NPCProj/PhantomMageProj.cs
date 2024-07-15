using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.NPCProj
{
	internal class PhantomMageProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;

			ProjectileID.Sets.TrailCacheLength[Type] = 30;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 24;
			Projectile.timeLeft = 300;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = true;

			Projectile.light = 0.25f;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public float sineTimer
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
			Projectile.rotation += MathHelper.ToRadians(3); // Constantly rotating

			sineTimer++;
			float sine = (float)Math.Sin(MathHelper.ToRadians(sineTimer * 3f));

			Vector2 offset = Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.PiOver2);
			float amplitude = 88f;
			offset *= sine *= amplitude;

			initialCenter += Projectile.velocity;
			Projectile.Center = initialCenter + offset;

			Visuals();
		}

		private void Visuals()
		{
			if (Main.rand.NextBool(4))
			{
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.LavaMoss, 0, 0, 100, default, 1f);
			}

			// Go through frames
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

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
					DustID.Clentaminator_Red, 0, 0, 100, default, 1f);
				dust.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			VertexStrip vertexStrip = new();
			MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
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
			Color result = Color.Lerp(Color.Red, Color.DarkRed, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: false)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 0.05f;
			float lerpValue = Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}
	}
}
