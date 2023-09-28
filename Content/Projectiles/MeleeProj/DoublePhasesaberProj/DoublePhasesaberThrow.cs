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

        int originalDirection;
        Rectangle projectileRect;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            bool returnProjectile = false; // Used for returning the projectile back

            if(originalDirection == 0)
            {
                originalDirection = Projectile.direction;
            }

            // Projectile will rotate when thrown
            Projectile.rotation += 0.5f;

            // Sound effect
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8; // This countsdown automatically
                SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
            }

            // Custom method for choosing sprite and light color
            Visuals();

            // Slow down the projectile
            // NOTE: this is a dumb way to slow and later accelerate something
            // Could have had a longer timer and just subtracted and later added X from the velocity...
            // ...or done it like in DunerangProj.cs
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

                // This makes the catch look better, without this the projectile sometimes dies when it doesn't look like it has hit the player
                // since the sprite does not cover the whole hitbox
                // Still not perfect, but works for now...
                if (originalDirection == 1)
                {
                    projectileRect = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width / 2, Projectile.height / 2);
                }
                else
                {
                    projectileRect = new((int)Projectile.Center.X, (int)Projectile.Center.Y, Projectile.width / 2, Projectile.height / 2);
                }

                // Once the projectile reaches the player, it will disappear
                if (player.Hitbox.Intersects(projectileRect))
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
