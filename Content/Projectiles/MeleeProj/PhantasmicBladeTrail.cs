using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
	internal class PhantasmicBladeTrail : SwingTrailBase
	{
		public override void SetDefaults()
		{
			base.SetDefaults();

			ScaleMultiplier = 1f;
			ScaleAdderValue = 1.1f;

			PrimaryDustColor = Color.White;
			SecondaryDustColor = Color.LightGray;
			DustType = DustID.Smoke;

			DoHitEffect = false;

			SwingBackColor = new(41, 41, 41);
			SwingMiddleColor = Color.Gray;
			SwingFrontColor = Color.White;
		}

		private int HitCounter = 0;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);

			if(Main.myPlayer == Projectile.owner && HitCounter % 2 == 0)
			{
				Vector2 spawnPos = new Vector2(target.Center.X, target.Center.Y - 400).RotatedBy(Main.rand.NextFloat(-1f, 1f), target.Center);
				int owner = Main.player[Projectile.owner].whoAmI;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, Vector2.Zero, ModContent.ProjectileType<PhantasmicBladeStrike>(), 60, 0, owner, target.whoAmI);
			}

			HitCounter++;
		}

		public override void PostAI()
		{
			Lighting.AddLight(Projectile.position, TorchID.White);
		}
	}
}
