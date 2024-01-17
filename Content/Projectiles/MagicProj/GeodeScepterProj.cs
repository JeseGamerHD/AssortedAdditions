using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
	internal class GeodeScepterProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 20;
			Projectile.timeLeft = 300;

			Projectile.DamageType = DamageClass.Magic;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
		}

		private bool setOnce = false;
		private readonly int [] dustTrailType = { DustID.GemDiamond, DustID.GemSapphire, DustID.GemTopaz, DustID.GemAmethyst, DustID.GemRuby, DustID.GemEmerald };

		public override void AI()
		{
			if(!setOnce)
			{
				Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
				setOnce = true;
			}

			if (Main.rand.NextBool())
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, dustTrailType[Projectile.frame]);
				dust.noGravity = true;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); // Dust from tile when hit
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center); // Tile hit sound
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
					dustTrailType[Projectile.frame], 0, 0, 150, default, 1f);
				dust.noGravity = true;
			}
		}
	}
}
