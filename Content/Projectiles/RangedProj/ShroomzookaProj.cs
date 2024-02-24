using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
	internal class ShroomzookaProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 22;
			Projectile.aiStyle = -1;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.friendly = true;

			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void AI()
		{
			Projectile.velocity.Y = Projectile.velocity.Y + 0.18f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.FungiHit, 1, 1, 0, default, 1f);
				dust.noGravity = true;
				dust.velocity *= 2f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FungiHit, 1, 1, 0, default, 2f);
				dust.noGravity = true;
				dust.velocity *= 2f;
				dust.scale *= 1.25f;
			}

			SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
		}
	}
}
