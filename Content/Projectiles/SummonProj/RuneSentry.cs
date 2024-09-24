using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using AssortedAdditions.Helpers;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles.SummonProj
{
	internal class RuneSentry : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 54;
			Projectile.penetrate = -1;

			Projectile.tileCollide = false;
			Projectile.friendly = true;

			Projectile.DamageType = DamageClass.Summon;
		}

		private ref float ControllerWhoAmI => ref Projectile.ai[0];
		private ref float OffSet => ref Projectile.ai[1];
		private ref float Timer => ref Projectile.ai[2];

		private int shootTimer = 0;

		public override void AI()
		{
			Projectile controller = Main.projectile[(int)ControllerWhoAmI];
			if (!controller.active)
			{
				Projectile.Kill();
			}
			Projectile.timeLeft = 120;

			if (shootTimer % 90 == 0)
			{
				ShootAtEnemy();
			}
			shootTimer++;

			Timer++;
			// Up/Down floating movement offset
			if (Timer <= 60)
			{
				OffSet -= 0.2f;
			}
			else if(Timer > 60 && Timer <= 120)
			{
				OffSet += 0.2f;
			}
			else
			{
				Timer = 0;
			}

			// The Sentry will float up/down above the controller
			// the - 45 to controller.Center.Y is just a good spot to float up/down from
			Projectile.Center = new Vector2(controller.Center.X, controller.Center.Y - 45 - OffSet);

			// Animate through the sprite frames
			int frameSpeed = 12;
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

			if(Main.rand.NextBool(7))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch);
				dust.noGravity = true;
				dust.noLight = true;
			}
		}

		private void ShootAtEnemy()
		{
			NPC target = HelperMethods.FindClosestNPC(Projectile.Center, 1500f);
			if (target != null)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Vector2 direction = target.Center - Projectile.Center;
					direction.Normalize();
					float rotation = MathHelper.ToRadians(75);
					float amount = 3;

					// Shoots out three at a time and spreads them out
					for (int i = 0; i < amount; i++)
					{
						Vector2 velocity = direction.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (amount - 1)));
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, velocity * 16f, ModContent.ProjectileType<RuneSentryShot>(), 55, 4, Projectile.owner);
					}
				}

				// Sound effect
				SoundEngine.PlaySound(SoundID.Item109 with { Pitch = 0.3f }, Projectile.position);

				// Dust effect
				for (int i = 0; i < 30; i++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f); // Creates a circle of dust
					Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.IchorTorch, speed * 3f, 75, default, 1.5f);
					dust.noGravity = true;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}

	public class RuneSentryShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 35;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 14;
			Projectile.timeLeft = 300;

			Projectile.tileCollide = false;
			Projectile.friendly = true;

			Projectile.DamageType = DamageClass.Summon;
		}

		private ref float Timer => ref Projectile.ai[0];
		public override void AI()
		{
			NPC target = HelperMethods.FindClosestNPC(Projectile.Center, 1000f);

			Timer++;
			if (Timer < 120f && Timer > 15f && target != null)
			{
				float speed = Projectile.velocity.Length();
				Vector2 direction = target.Center - Projectile.Center;
				direction.Normalize();
				direction *= speed;

				Projectile.velocity = (Projectile.velocity * 5f + direction) / 30f;
				Projectile.velocity.Normalize();
				Projectile.velocity *= speed;
			}

			if (target == null)
			{
				Projectile.velocity.Y = Projectile.velocity.Y + 0.3f; // 0.1f for arrow gravity, 0.4f for knife gravity
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
							DustID.IchorTorch, 0, 0, 150, default, 1f);
				dust.noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
				Projectile.owner);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur,
				new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
				Projectile.owner);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, 0f, 0f, 100, default, 1f);
				dust.velocity *= 1.4f;
			}
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
			Color result = Color.Lerp(Color.PeachPuff, Color.Yellow, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: false)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
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
