using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
	internal class AncientGauntletProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 30;

			ProjectileID.Sets.TrailCacheLength[Type] = 15;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 32;
			Projectile.timeLeft = 180;
			Projectile.aiStyle = 0;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = true;

			Projectile.DamageType = DamageClass.Magic;
		}

		public override void AI()
		{
			// Two dust trails on the sides of the Projectile, similar to Meteor Staff projectiles.
			Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X + 14, Projectile.Center.Y) - Projectile.velocity, DustID.Flare, Scale: 1.2f);
			dust.noGravity = true;
			Dust dust2 = Dust.NewDustPerfect(new Vector2(Projectile.Center.X - 14, Projectile.Center.Y) - Projectile.velocity, DustID.Flare, Scale: 1.2f);
			dust2.noGravity = true;

			Lighting.AddLight((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, TorchID.Orange, 0.25f);

			int frameSpeed = 4;
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

			Projectile.rotation = Projectile.velocity.ToRotation(); // Face its direction
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

			SoundStyle sound = new SoundStyle("AssortedAdditions/Assets/Sounds/ProjectileSound/FireExplosion") with { Volume = 0.9f};
			SoundEngine.PlaySound(sound, Projectile.position);
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
			float num = 1.5f;
			float lerpValue = Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}
	}
}
