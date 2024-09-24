using AssortedAdditions.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedAdditions.Content.Items.Accessories.Runes;

namespace AssortedAdditions.Content.Projectiles.SummonProj
{
	internal class RuneSentryController : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 28;
			Projectile.penetrate = -1;

			Projectile.tileCollide = true;
			Projectile.friendly = true;

			Projectile.DamageType = DamageClass.Summon;
		}

		public ref float RuneAbilityActivate => ref Projectile.ai[0];

		public override bool PreAI()
		{
			// Spawn the Sentry part if one is not already spawned
			if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<RuneSentry>()] < 1)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), new Vector2(Projectile.Center.X + 24, Projectile.Center.Y + 15),
						Vector2.Zero, ModContent.ProjectileType<RuneSentry>(), 40, 4, Projectile.owner, Projectile.whoAmI);
				}
			}

			return true;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			if (Main.myPlayer == owner.whoAmI)
			{
				if (owner.dead || !owner.active || !owner.GetModPlayer<RuneOfGuardingPlayer>().isWearingRuneOfGuarding)
				{
					Projectile.Kill();
				}

				// If the owner presses the ability key again, the controller will teleport to the player
				if (CustomKeyBinds.RuneAbility.JustPressed && Projectile.timeLeft != 3600)
				{ // timeleft check because its spawned initally using the same key from an accessory, which creates dust effect so this prevents double dust effect on spawning the projectile
					RuneAbilityActivate = 1;
					Projectile.netUpdate = true;
				}
			}

			Vector2 distanceVector = owner.Center - Projectile.Center;
			float distanceToOwner = distanceVector.Length();
			if (distanceToOwner > 2000f)
			{
				Projectile.Kill(); // Despawn if owner is far away
			}

			Projectile.velocity.Y = 6f; // Constant gravity
			Projectile.timeLeft = 120; // Keep it alive

			// Teleport
			if (RuneAbilityActivate != 0)
			{
				// Dust effect before teleport
				for (int i = 0; i < 30; i++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(1.5f, 1.5f); // Creates a circle of dust
					Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Teleporter, speed * 6f, 75, default, 2f);
					dust.noGravity = true;
				}

				Projectile.position = new Vector2(owner.position.X, owner.position.Y - 20);
				SoundEngine.PlaySound(SoundID.Item82, owner.position);

				// Dust effect after teleport
				for (int i = 0; i < 30; i++)
				{
					Vector2 speed = Main.rand.NextVector2CircularEdge(1.5f, 1.5f); // Creates a circle of dust
					Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.IchorTorch, speed * 6f, 75, default, 2f);
					dust.noGravity = true;
				}

				RuneAbilityActivate = 0;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false; // False to not go through platforms

			// Stop stutter when in slopes
			if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			{
				Projectile.velocity.Y *= 0;
			}

			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false; // Dont die when hitting tiles
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}
