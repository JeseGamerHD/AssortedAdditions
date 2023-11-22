using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Items.Weapons.Melee.BoStaffs;

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
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Check if Player Dead
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Weapon will spin as long as player keeps channeling
            if (player.channel)
            {
                Projectile.position = player.Center; // Needs to be adjusted below
                Projectile.position.Y -= 40;

                if (player.direction == -1) // If player faces left adjust accordingly
                {
                    Projectile.position.X -= 40;
                    Projectile.rotation -= 0.2f; // Rotation also changes depending on direction faced
                }
                else // If right, adjust differently
                {
                    Projectile.position.X -= 45;
                    Projectile.rotation += 0.2f;
                }

                // Used for swapping the weapon position if the player switches direction
                int switchSides = player.direction;

                // Weapon needs to "point" towards the cursor's X position
                if (Main.MouseWorld.X > player.Center.X)
                {
                    player.ChangeDir(1); // Change player to face said direction
                }
                else if (Main.MouseWorld.X < player.Center.X)
                {
                    player.ChangeDir(-1);
                }

                // Replaces the old projectile with a new one after switching direction
                // New projectile alligns properly
                if (switchSides != player.direction)
                {
                    Projectile.Kill();
                }

                // Makes player hold the weapon
                player.heldProj = Projectile.whoAmI;
                player.itemTime = 2;
                player.itemAnimation = 2;
                player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);

                // Sprite changes depending on which staff is being used
                Visuals();

                // Sound effect
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 14; // This countsdown automatically
                    SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
                }

            }
            else // When channeling stops, the projectile is destroyed
            {
                Projectile.Kill();
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

            if (itemInUse == ModContent.ItemType<BorealBoStaff>())
            {
                Projectile.frame = 1;
            }

            if (itemInUse == ModContent.ItemType<BoStaff>())
            {
                Projectile.frame = 2;
            }

            if (itemInUse == ModContent.ItemType<DynastyBoStaff>())
            {
                Projectile.frame = 3;
            }

            if (itemInUse == ModContent.ItemType<EbonWoodBoStaff>())
            {
                Projectile.frame = 4;
            }

            if (itemInUse == ModContent.ItemType<MahoganyBoStaff>())
            {
                Projectile.frame = 5;
            }

            if (itemInUse == ModContent.ItemType<PalmBoStaff>())
            {
                Projectile.frame = 6;
            }

            if (itemInUse == ModContent.ItemType<PearlWoodBoStaff>())
            {
                Projectile.frame = 7;
            }

            if (itemInUse == ModContent.ItemType<ShadeWoodBoStaff>())
            {
                Projectile.frame = 8;
            }

            if (itemInUse == ModContent.ItemType<SpookyBoStaff>())
            {
                Projectile.frame = 9;
            }
        }
    }
}
