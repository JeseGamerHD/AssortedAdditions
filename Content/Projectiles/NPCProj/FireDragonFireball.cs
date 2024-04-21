using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.NPCProj
{
	internal class FireDragonFireball : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 25;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 26;

			Projectile.light = 0.25f;
			Projectile.timeLeft = 500;
			Projectile.scale = 1.2f;

			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.ignoreWater = true;

			Projectile.aiStyle = 0;
		}

		public override void AI()
		{
			Projectile.rotation += MathHelper.ToRadians(3);
			Projectile.spriteDirection = Projectile.direction; // Faces left/right correctly

			if (Main.rand.NextBool())
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FlameBurst, 1f, 1f, 0, default, 1.5f);
				dust.noGravity = true;
				dust.velocity *= 1.6f;
			}

			// Two dust trails on the sides of the Projectile, similar to Meteor Staff projectiles.
			Dust dustTrail = Dust.NewDustPerfect(new Vector2(Projectile.Center.X + 12, Projectile.Center.Y) - Projectile.velocity, DustID.Flare, Scale: 2f);
			dustTrail.noGravity = true;
			Dust dustTrail2 = Dust.NewDustPerfect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y) - Projectile.velocity, DustID.Flare, Scale: 2f);
			dustTrail2.noGravity = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.FlameBurst, Scale: 1.5f);
				dust.noGravity = true;
				dust.velocity *= Projectile.oldVelocity;
				dust.velocity.Normalize();
			}

			SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, Projectile.position);
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
			float num = 1.3f;
			float lerpValue = Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}
	}
}
