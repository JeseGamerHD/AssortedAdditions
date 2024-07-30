using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Projectiles.SummonProj
{
	internal class PhantasmicDaggerSoul : ModProjectile
	{
		public override string Texture => "AssortedAdditions/Content/Projectiles/MagicProj/SpiritVaseProj";
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.alpha = 255;

			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			Projectile.aiStyle = 0;
		}

		public override void AI()
		{
			Player target = Main.player[Projectile.owner];

			Projectile.ai[1]++;
			if (target != null)
			{
				float speed = Projectile.velocity.Length();
				Vector2 direction = target.Center - Projectile.Center;
				direction.Normalize();
				direction *= speed;

				Projectile.velocity = (Projectile.velocity * 8f + direction) / 15f;
				Projectile.velocity.Normalize();
				Projectile.velocity *= speed;
			}

			if (Projectile.velocity.Length() < 13f)
			{
				Projectile.velocity *= 1.02f;
			}

			Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.WhiteTorch, 0, 0, 0, default, 3f);
			dust.noGravity = true;
			dust.noLight = true;

			if(Projectile.Hitbox.Intersects(target.Hitbox))
			{
				Projectile.Kill();
			}
		}

		public override void PostAI()
		{
			if (Main.rand.NextBool(2))
			{
				Dust dust = Dust.NewDustPerfect(Projectile.position - Projectile.velocity, DustID.WhiteTorch, null, 100, default, Main.rand.NextFloat(1.25f, 2f));
				dust.noGravity = true;
				dust.noLight = true;
			}
		}
	}
}
