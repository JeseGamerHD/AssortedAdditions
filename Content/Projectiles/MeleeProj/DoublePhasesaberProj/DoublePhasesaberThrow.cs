using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Weapons.Melee.Sabers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.MeleeProj.DoublePhasesaberProj
{
    internal class DoublePhasesaberThrow : ModProjectile
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
            Projectile.timeLeft = 3000;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner]; // Used for returning the boomerang
            bool returnProjectile = false; // Used for returning the projectile back

            // Projectile will rotate when thrown
            Projectile.rotation += 0.3f;

            // Sound effect
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8; // This countsdown automatically
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
            }

            Visuals();

            // Gradually slow down the projectile after 0.5 seconds in the air
            if (Projectile.timeLeft < 2970)
            {
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 5f)
                {
                    Projectile.velocity *= 0.97f;
                    returnProjectile = true;
                }
            }

            if (returnProjectile == true)
            {
                Projectile.velocity = Projectile.DirectionTo(player.Center);

                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 5f)
                {
                    Projectile.velocity *= 10f;
                }

                // Once the projectile reaches the player, it will disappear
                if (Projectile.Hitbox.Intersects(player.Hitbox))
                {
                    Projectile.Kill();
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White; // Saber glows in the dark
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index); // Saber is drawn over players
        }

        private void Visuals()
        {
            Player player = Main.player[Projectile.owner];
            int itemInUse = player.inventory[player.selectedItem].type; // The item the player is currently using

            // Each color has its own ModItem so the frame can be selected based on that
            // Emitted light color is also selected here
            if (itemInUse == ModContent.ItemType<YellowDoublePhasesaber>())
            {
                Projectile.frame = 0;
                Lighting.AddLight(Projectile.Center, TorchID.Yellow);
            }

            if (itemInUse == ModContent.ItemType<OrangeDoublePhasesaber>())
            {
                Projectile.frame = 1;
                Lighting.AddLight(Projectile.Center, TorchID.Orange);
            }

            if (itemInUse == ModContent.ItemType<GreenDoublePhasesaber>())
            {
                Projectile.frame = 2;
                Lighting.AddLight(Projectile.Center, TorchID.Green);
            }

            if (itemInUse == ModContent.ItemType<BlueDoublePhasesaber>())
            {
                Projectile.frame = 3;
                Lighting.AddLight(Projectile.Center, TorchID.Blue);
            }

            if (itemInUse == ModContent.ItemType<PurpleDoublePhasesaber>())
            {
                Projectile.frame = 4;
                Lighting.AddLight(Projectile.Center, TorchID.Purple);
            }

            if (itemInUse == ModContent.ItemType<RedDoublePhasesaber>())
            {
                Projectile.frame = 5;
                Lighting.AddLight(Projectile.Center, TorchID.Red);
            }
            if (itemInUse == ModContent.ItemType<WhiteDoublePhasesaber>())
            {
                Projectile.frame = 6;
                Lighting.AddLight(Projectile.Center, TorchID.White);
            }
        }
    }
}
