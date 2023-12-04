using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Items.Weapons.Melee.BoStaffs;
using System.Collections.Generic;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
    internal class BoStaffSpin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.width = 82;
            Projectile.height = 82;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;

			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.ownerHitCheck = true;
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
				Projectile.rotation += 0.2f;
				player.ChangeDir(1);
			}
			else if (Projectile.velocity.X < 0f)
			{
				Projectile.rotation -= 0.2f;
				player.ChangeDir(-1);
			}

			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(15);
			Projectile.Center = playerCenter;
			player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

			// Sprite changes depending on which staff is being used
			Visuals();

			// Sound effect
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 14; // This countsdown automatically
				SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
			}
		}

		private void Visuals()
        {
            Player player = Main.player[Projectile.owner];
            int itemInUse = player.inventory[player.selectedItem].type; // The item the player is currently using

            if (itemInUse == ModContent.ItemType<AshBoStaff>())
            {
                Projectile.frame = 0;
            }

            else if (itemInUse == ModContent.ItemType<BorealBoStaff>())
            {
                Projectile.frame = 1;
            }

            else if (itemInUse == ModContent.ItemType<BoStaff>())
            {
                Projectile.frame = 2;
            }

			else if(itemInUse == ModContent.ItemType<DynastyBoStaff>())
            {
                Projectile.frame = 3;
            }

			else if(itemInUse == ModContent.ItemType<EbonWoodBoStaff>())
            {
                Projectile.frame = 4;
            }

			else if(itemInUse == ModContent.ItemType<MahoganyBoStaff>())
            {
                Projectile.frame = 5;
            }

			else if(itemInUse == ModContent.ItemType<PalmBoStaff>())
            {
                Projectile.frame = 6;
            }

			else if(itemInUse == ModContent.ItemType<PearlWoodBoStaff>())
            {
                Projectile.frame = 7;
            }

			else if(itemInUse == ModContent.ItemType<ShadeWoodBoStaff>())
            {
                Projectile.frame = 8;
            }

			else if(itemInUse == ModContent.ItemType<SpookyBoStaff>())
            {
                Projectile.frame = 9;
            }
        }
    }
}
