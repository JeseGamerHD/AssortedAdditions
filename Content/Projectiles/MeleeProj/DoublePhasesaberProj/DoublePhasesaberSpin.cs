using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ModdingTutorial.Content.Items.Weapons.Melee.Sabers;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace ModdingTutorial.Content.Projectiles.MeleeProj.DoublePhasesaberProj
{
    internal class DoublePhasesaberSpin : ModProjectile // This handles all the double phasesabers, projectile sprite will be swapped depending on which Item is used
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7; // 7 different colors, AI will pick the right one
        }

        public override void SetDefaults()
        {
            Projectile.width = 112;
            Projectile.height = 112;
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
                Projectile.position.Y -= 52;

                if (player.direction == -1) // If player faces left adjust accordingly
                {
                    Projectile.position.X -= 60;
                    Projectile.rotation -= 0.3f; // Rotation also changes depending on direction faced
                }
                else // If right, adjust differently
                {
                    Projectile.position.X -= 55; 
                    Projectile.rotation += 0.3f;
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

                // The color of the sprite is determined in a custom method
                Visuals();

                // Sound effect
                if (Projectile.soundDelay == 0)
                {
                    Projectile.soundDelay = 8; // This countsdown automatically
                    SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
                }

            }
            else // When channeling stops, the projectile is destroyed
            {
                Projectile.Kill();
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index); // Saber is drawn over the player's arm
        }

        //public override string GlowTexture => "ModdingTutorial/Content/Projectiles/MeleeProj/DoublePhasesaberProj/DoublePhasesaberThrow_Glow";

        // The color of the sprite will be chosen here
        private void Visuals()
        {
            Player player = Main.player[Projectile.owner];
            int itemInUse = player.inventory[player.selectedItem].type; // The item the player is currently using

            // Each color has its own ModItem so the frame can be selected based on that
            // Emitted light color is also selected here
            if (itemInUse == ModContent.ItemType<YellowDoublePhasesaber>()) { 
                Projectile.frame = 0; 
                Lighting.AddLight(player.Center, TorchID.Yellow); }

            if (itemInUse == ModContent.ItemType<OrangeDoublePhasesaber>()) { 
                Projectile.frame = 1;
                Lighting.AddLight(player.Center, TorchID.Orange);
            }

            if (itemInUse == ModContent.ItemType<GreenDoublePhasesaber>()) { 
                Projectile.frame = 2;
                Lighting.AddLight(player.Center, TorchID.Green);
            }

            if (itemInUse == ModContent.ItemType<BlueDoublePhasesaber>()) { 
                Projectile.frame = 3;
                Lighting.AddLight(player.Center, TorchID.Blue);
            }

            if (itemInUse == ModContent.ItemType<PurpleDoublePhasesaber>()) { 
                Projectile.frame = 4;
                Lighting.AddLight(player.Center, TorchID.Purple);
            }

            if (itemInUse == ModContent.ItemType<RedDoublePhasesaber>()) {
                Projectile.frame = 5;
                Lighting.AddLight(player.Center, TorchID.Red);
            }
            if (itemInUse == ModContent.ItemType<WhiteDoublePhasesaber>()) {
                Projectile.frame = 6;
                Lighting.AddLight(player.Center, TorchID.White);
            }
        }
    }
}
