using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.SummonProj
{
	internal class GraniteStaffMinionOrbit : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.penetrate = -1;

			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;

			Projectile.DamageType = DamageClass.Summon;
		}

		float ParentIdentity
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		float Offset
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		float OrbitTimer
		{
			get => Projectile.ai[2];
			set => Projectile.ai[2] = value;
		}

		public override void AI()
		{
			Projectile parent = Main.projectile.FirstOrDefault(x => x.identity == ParentIdentity);

			if (parent.type != ModContent.ProjectileType<GraniteStaffMinion>() || !parent.active)
			{
				Projectile.active = false;
			}
			else
			{
				Projectile.timeLeft = 666;
			}

			// Circles around the parent Projectile
			OrbitTimer += 0.05f;
			Vector2 Orbit = parent.Center + new Vector2(Offset, 0).RotatedBy(OrbitTimer);
			Projectile.Center = Orbit;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}
}
