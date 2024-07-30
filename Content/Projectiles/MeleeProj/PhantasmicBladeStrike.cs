using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
	internal class PhantasmicBladeStrike : ModProjectile
	{
		public override string Texture => "AssortedAdditions/Content/Projectiles/NPCProj/TheHauntProj";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 20;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;

			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			Projectile.DamageType = DamageClass.Melee;

			Projectile.aiStyle = 0;
		}

		private ref float Target => ref Projectile.ai[0];
		private ref float CanHit => ref Projectile.ai[1];

		private ref float Speed => ref Projectile.ai[2];

		public override void AI()
		{
			if(Projectile.timeLeft == 300)
			{
				SoundEngine.PlaySound(SoundID.Item8 with { Pitch = -0.3f, MaxInstances = 3 }, Projectile.position);
			}

			if (!Main.npc[(int)Target].active)
			{
				CanHit++;
				Projectile.netUpdate = true;
			}

			if (CanHit == 0)
			{
				// Accelerate until target is hit
				Speed += 0.8f;
				Projectile.netUpdate = true;
			}
			else
			{
				// Slow down after going through the target
				Projectile.velocity *= 0.96f;

				if(Speed <= 0)
				{
					Projectile.Kill();
				}
			}

			if(CanHit == 0)
			{
				Vector2 direction = Main.npc[(int)Target].Center - Projectile.Center;
				direction.Normalize();

				Projectile.velocity = direction * Speed;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if(CanHit == 0)
			{
				Projectile.timeLeft = 75;
			}

			CanHit++; // Fly off after hitting a target
			Projectile.netUpdate = true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (CanHit > 0)
			{
				return false;
			}

			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width * 2, Projectile.height * 2, DustID.GemDiamond, 0f, 0f, 75, default, 1f);
				dust.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			VertexStrip vertexStrip = new();
			MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
			miscShaderData.UseSaturation(-0.8f);
			miscShaderData.UseOpacity(4f);
			miscShaderData.Apply();
			vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2, true);
			vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();

			return true;
		}

		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.Black, Color.White, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: false)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 255;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 1.2f;
			float lerpValue = Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true);
			num *= 2f - (2f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 36f, num);
		}
	}
}
