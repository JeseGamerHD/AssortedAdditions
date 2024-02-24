using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
	internal class ShroomMacepoleProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 94;
			Projectile.height = 94;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;

			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.friendly = true;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

			if (Main.myPlayer == Projectile.owner)
			{
				if (player.channel)
				{
					float holdoutDistance = player.HeldItem.shootSpeed * Projectile.scale;
					Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);

					if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y)
					{
						Projectile.netUpdate = true;
					}

					Projectile.velocity = holdoutOffset;
				}
				else
				{
					Projectile.Kill();
				}
			}

			if (Projectile.velocity.X > 0f)
			{
				Projectile.rotation += 0.1f;
				player.ChangeDir(1);
			}
			else if (Projectile.velocity.X < 0f)
			{
				Projectile.rotation -= 0.1f;
				player.ChangeDir(-1);
			}

			DustEffect();

			Projectile.timeLeft = 2;
			Projectile.spriteDirection = Projectile.direction;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(2);
			Projectile.Center = playerCenter;
			player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

			// Sound effect
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 28; // This countsdown automatically
				SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
			}
		}

		// Taken from Sky Dragon's Fury AI
		private void DustEffect()
		{
			int direction = Math.Sign(Projectile.velocity.X);
			float rotationAdjustment = Projectile.rotation - (float)Math.PI / 4f * direction;
			Vector2 rotation = rotationAdjustment.ToRotationVector2();
			Vector2 perpendicular = rotation.RotatedBy((float)Math.PI / 2f * Projectile.spriteDirection);

			for (int j = 0; j < 4; j++)
			{
				float velocityAdjustment = 1f;
				float positionAdjustment = 1f;
				switch (j)
				{
					case 1:
						positionAdjustment = -1f;
						break;
					case 2:
						positionAdjustment = 1.25f;
						velocityAdjustment = 0.5f;
						break;
					case 3:
						positionAdjustment = -1.25f;
						velocityAdjustment = 0.5f;
						break;
				}
				if (!Main.rand.NextBool(6))
				{
					bool dustVariation = Main.rand.NextBool();
					int dustType = dustVariation ? DustID.BlueTorch : DustID.WaterCandle;

					Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, dustType, 0f, 0f, 100, default, 1.5f);
					dust.position = Projectile.Center + rotation * (60f + Main.rand.NextFloat() * 20f) * positionAdjustment;
					dust.velocity = perpendicular * (4f + 4f * Main.rand.NextFloat()) * positionAdjustment * velocityAdjustment;
					dust.noGravity = true;
					dust.customData = this;
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 20;

			for(int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(target.position, 20, 20, DustID.BlueTorch);
				dust.velocity *= 1.4f;
			}
		}
	}
}
