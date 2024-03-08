using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
	internal class SawbladeLauncherProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 15;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.penetrate = 7;
			Projectile.timeLeft = 300;

			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Lihzahrd);
				dust.noGravity = true;
			}
			Lighting.AddLight((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, TorchID.Orange, 0.25f);

			Projectile.rotation += 0.2f;

			// Delayed gravity
			if(Projectile.timeLeft < 270)
			{
				Projectile.velocity.Y = Projectile.velocity.Y + 0.5f;
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Lihzahrd);
				dust.noGravity = true;
				dust.velocity *= 1.6f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); // Dust from tile when hit
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center); // Tile hit sound

			// If collide with tile, reduce the penetrate.
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) // Once zero, kill the projectile
			{
				Projectile.Kill();
			}
			else
			{
				// If the projectile hits the left or right side of the tile, reverse the X velocity
				if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}

				// If the projectile hits the top or bottom side of the tile, reverse the Y velocity
				if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}

			return false;
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
