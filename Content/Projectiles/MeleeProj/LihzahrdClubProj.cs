using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
	internal class LihzahrdClubProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 11;
		}

		public override void SetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 90;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 0;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = true;

			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			int frameSpeed = 3;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;

				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.Kill();
				}
			}

			if (Main.rand.NextBool(2) && Projectile.frame < 9)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Flare, 0, 0, 0, default, 1.5f);
				dust.noGravity = false;
				dust.velocity *= 1.8f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 8;

			for (int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustDirect(target.Center, 20, 20, DustID.Smoke);
				dust.velocity *= 1.4f;
			}
		}
	}
}
