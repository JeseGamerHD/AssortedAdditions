using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
    internal class DesertsFuryProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;

			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.friendly = true;
			Projectile.hide = true;
		}

		float rotateSpeed = 0; // Used for accelerating rotation
		float delay = 28; // Used for accelerating sound

		public override void AI()
        {
            Player player = Main.player[Projectile.owner];
			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

			// Gradually speed up the spin
			if (rotateSpeed <= 0.2f)
            {
				rotateSpeed += 0.0015f;
            }


            if(Main.myPlayer == Projectile.owner)
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
				Projectile.rotation += rotateSpeed;
				player.ChangeDir(1);
			}
			else if (Projectile.velocity.X < 0f)
			{
				Projectile.rotation -= rotateSpeed;
				player.ChangeDir(-1);
			}

			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(2);
			Projectile.Center = playerCenter;
			player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

			if (delay > 14)
			{
				delay -= 0.105f;
			}

			// Sound effect
			// Gets faster along with the spin
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = (int)delay; // This countsdown automatically
				SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
			}

			if (Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Sandnado, 0, 0, 100, default, 2f);
				dust.noGravity = true;

				Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Sandstorm, 0, 0, 100, default, 2f);
				dust2.noGravity = true;
			}
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index); // is drawn over the player's arm
        }
    }
}
