using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.NPCProj
{
	internal class TheHauntGhost : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;

			ProjectileID.Sets.TrailCacheLength[Type] = 15;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.timeLeft = 300;

			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

			CooldownSlot = ImmunityCooldownID.Bosses;

			Projectile.aiStyle = 0;
		}

		public override void AI()
		{
			int target = 0;
			target = Player.FindClosest(Projectile.Center, 1, 1);

			Projectile.ai[1]++;
			if (Projectile.ai[1] < 110f && Projectile.ai[1] > 30f)
			{
				float speed = Projectile.velocity.Length();
				Vector2 direction = Main.player[target].Center - Projectile.Center;
				direction.Normalize();
				direction *= speed;

				Projectile.velocity = (Projectile.velocity * 24f + direction) / 25f;
				Projectile.velocity.Normalize();
				Projectile.velocity *= speed;
			}

			if (Projectile.velocity.Length() < 18f)
			{
				Projectile.velocity *= 1.02f;
			}

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
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Smoke, 0, 0, 100, default, 1.25f);
				dust.noGravity = true;

				Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Cloud, 0, 0, 100, default, 1.5f);
				dust2.noGravity = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 10; i++)
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
