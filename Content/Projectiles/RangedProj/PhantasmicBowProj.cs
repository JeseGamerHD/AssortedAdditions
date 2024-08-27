using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
	internal class PhantasmicBowProj : ModProjectile
	{
		public override string Texture => "AssortedAdditions/Content/Projectiles/NPCProj/TheHauntProj";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 28;
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

			Projectile.DamageType = DamageClass.Ranged;

			Projectile.aiStyle = 0;
		}

		private ref float FlyOff => ref Projectile.ai[0];

		public override void AI()
		{
			if(Projectile.timeLeft == 300)
			{
				SoundEngine.PlaySound(SoundID.Item5, Main.player[Projectile.owner].position);
			}

			float range = 600f; // Max radius that the projectile can detect a target

			NPC closestNPC = FindClosestNPC(range);
			if (closestNPC == null || FlyOff != 0)
			{
				Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
				if (Projectile.velocity.Y > 12f) // Increase Y velocity to add gravity until max of 16 is reached
				{
					Projectile.velocity.Y = 12f;
				}
				Projectile.rotation = Projectile.velocity.ToRotation(); // Face towards where its going
				return;
			}

			if (FlyOff == 0)
			{
				// With these the projectile will move towards the target in a smooth way
				// instead of snapping it moves in a curved way
				float target = (closestNPC.Center - Projectile.Center).ToRotation();
				float curve = Projectile.velocity.ToRotation();
				float maxTurn = MathHelper.ToRadians(2f);

				// Set the velocity and rotation:
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.WrapAngle(curve.AngleTowards(target, maxTurn)) - curve);
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
		}

		private NPC FindClosestNPC(float range)
		{
			NPC closestNPC = null;

			// Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
			float sqrMaxDetectDistance = range * range;

			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.CanBeChasedBy())
				{
					// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
					float sqrDistanceToTarget = Vector2.DistanceSquared(npc.Center, Projectile.Center);

					// Check if it is within the radius
					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = npc;
					}
				}
			}

			return closestNPC;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			FlyOff++; // Fly off after hitting a target
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			FlyOff++;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (FlyOff > 0)
			{
				return false;
			}

			return base.CanHitNPC(target);
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
			Color result = Color.Lerp(Color.Black, Color.Gray, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: false)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 255;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 2f;
			float lerpValue = Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 36f, num);
		}
	}
}
