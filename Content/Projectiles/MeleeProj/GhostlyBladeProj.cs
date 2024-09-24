using AssortedAdditions.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
	internal class GhostlyBladeProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 32;
			Projectile.aiStyle = 0;
			Projectile.alpha = 55;
			Projectile.timeLeft = 600;

			Projectile.DamageType = DamageClass.Melee;

			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			// Leave a dust trail
			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
							DustID.WhiteTorch, 0, 0, 0, default, 2f);
				dust.noGravity = true;
			}

			// First fly up for two seconds
			Projectile.ai[0]++;
			if (Projectile.ai[0] <= 120)
			{
				Projectile.velocity.Y = -2f;
				return;
			}

			// Then finds a target and homes in on it
			NPC closestNPC = HelperMethods.FindClosestNPC(Projectile.Center, 1000f);
			if (closestNPC == null)
			{
				// If no target is found within 2 seconds then fly off
				Projectile.velocity.Y = -2f;
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
				return;
			}

			// If found, change the velocity of the projectile and turn it in the direction of the target
			// Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
			// With these the projectile will move towards the target in a smooth way
			float target = (closestNPC.Center - Projectile.Center).ToRotation();
			float curve = Projectile.velocity.ToRotation();
			float maxTurn = MathHelper.ToRadians(3f); // Lower values mean faster turning
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.WrapAngle(curve.AngleTowards(target, maxTurn)) - curve);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.WhiteTorch, 0f, 0f, 75, default, 1f);
				dust.velocity *= 1.4f;
				dust.noGravity = true;
			}

			SoundEngine.PlaySound(SoundID.Zombie54, Projectile.position);
		}
	}
}
