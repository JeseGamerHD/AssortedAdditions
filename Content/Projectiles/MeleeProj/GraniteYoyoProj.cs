using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
	internal class GraniteYoyoProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 7f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 128; // Divide the value by 16 to get the reach in tiles
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;

			ProjectileID.Sets.TrailCacheLength[Type] = 20;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = -1;

			Projectile.friendly = true;

			Projectile.aiStyle = ProjAIStyleID.Yoyo;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
		}

		public override void PostAI()
		{
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width * 2, Projectile.height * 2, DustID.Granite);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
			}

			Lighting.AddLight((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, TorchID.Purple, 0.25f);
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
			Color result = Color.Lerp(Color.MidnightBlue, Color.MidnightBlue, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: false)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 0.1f;
			float lerpValue = Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}
	}
}
