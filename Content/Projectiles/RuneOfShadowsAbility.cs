using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles
{
	internal class RuneOfShadowsAbility : ModProjectile
	{
		public override string Texture => "AssortedAdditions/Content/Projectiles/MagicProj/SpiritVaseProj";

		public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 32;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.light = 0.25f;
			Projectile.alpha = 255;

			Projectile.DamageType = DamageClass.Generic;

			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile.velocity.Y = 18f;

			// Sound effect TODO FIND COOL SOUND
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 9; // This countsdown automatically
				SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
			}

			// TODO Slow down once hit target/ground
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width * 2, Projectile.height * 2, DustID.GemAmethyst, 0f, 0f, 75, Color.Magenta, 1f);
				dust.velocity *= 1.4f;
				dust.noGravity = true;
			}
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
			Color result = Color.Lerp(Color.Fuchsia, Color.Purple, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 3f;
			float lerpValue = Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(16f, 32f, num);
		}
	}
}
