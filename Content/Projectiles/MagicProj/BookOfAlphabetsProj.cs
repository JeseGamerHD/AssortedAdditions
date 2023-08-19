using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.MagicProj
{
    internal class BookOfAlphabetsProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 26;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.timeLeft = 480;
            Projectile.penetrate = 8; // Projectile will bounce from tiles, set in OnTileCollide()

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;

            Projectile.aiStyle = 0;
        }

        private bool setVisual = false; // Projectile can be one of the alphabets, this is set only once and picked randomly
        public override void AI()
        {
            // Pick a frame randomly
            if(!setVisual) 
            {
                Projectile.frame = Main.rand.Next(25);
                Projectile.rotation = Main.rand.Next(-10, 11) * 57.2958f; // Set a random rotation as well
                setVisual = true;
            }

            Projectile.velocity.Y = Projectile.velocity.Y + 0.05f; // 0.1f for arrow gravity, 0.4f for knife gravity
            if (Projectile.velocity.Y > 16f) // This check implements "terminal velocity". We don't want the projectile to keep getting faster and faster. Past 16f this projectile will travel through blocks, so this check is useful.
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // If collide with tile, reduce the penetrate.
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0) // Once zero, kill the projectile
            {
                Projectile.Kill();
            }
            else
            {
                // If the projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }

                // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
            // On death create dust explosion
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.TintableDust, 0, 0, 100, default, 1f);
                dust.noGravity = true;
            }
        }
    }
}
