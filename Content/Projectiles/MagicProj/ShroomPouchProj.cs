using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
	internal class ShroomPouchProj : ModProjectile
	{
		public override string Texture => "AssortedAdditions/Content/Projectiles/MagicProj/SpiritVaseProj";

		public override void SetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 80;

			Projectile.DamageType = DamageClass.Magic;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		public override void AI()
		{

			if (Main.rand.NextBool(4))
			{
				Vector2 speed = Main.rand.NextVector2Unit(MathHelper.Pi / 4, MathHelper.Pi / 2) * Main.rand.NextFloat();
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch, speed.X, speed.Y, 0, default, 2f);
				dust.noGravity = true;
				dust.velocity *= 2f;
				dust.scale *= 1.25f;
			}


			if (Projectile.ai[0] == 0)
			{
				for (int i = 0; i < 15; i++)
				{
					Vector2 speed = Main.rand.NextVector2Unit(MathHelper.Pi / 4, MathHelper.Pi / 2) * Main.rand.NextFloat();
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch, speed.X, speed.Y, 0, default, 2f);
					dust.noGravity = true;
					dust.velocity *= 2f;
					dust.scale *= 1.25f;
				}

				SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with { Pitch = 1f }, Projectile.position);
				Projectile.ai[0]++;
			}

			if (Projectile.ai[0] == 1)
			{
				for (int i = 0; i < 40; i++)
				{
					bool changeDust = Main.rand.NextBool();
					int dustType = changeDust ? DustID.BlueTorch : DustID.WaterCandle;
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType, 2f, 2f, 0, default);
					dust.noGravity = true;
					dust.fadeIn = 2;
					dust.velocity = Projectile.velocity * Main.rand.Next(1, 5);
				}

				Projectile.ai[0]++;
			}

			Projectile.velocity *= 0.98f;
		}

		// Killed enemies explode into more spores
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 10;

			if(target.life < 0 && Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Projectile.velocity, ModContent.ProjectileType<ShroomPouchProj>(), 22, 0, Projectile.owner);
			}
		}
	}
}
