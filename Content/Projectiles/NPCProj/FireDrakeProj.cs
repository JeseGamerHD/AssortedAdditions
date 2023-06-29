using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.NPCProj
{
    internal class FireDrakeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = 180;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.light = 0.5f;
            Projectile.alpha = 75;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
            // Faces the correct way
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            // Leave a dust trail
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.Flare, 0, 0, 150, default, 2f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.OrangeTorch, 0, 0, 150, default, 1f);
                dust2.noGravity = true;
            }

            // Loop through the sprite frames
            int frameSpeed = 5;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Projectile.alpha++; // Fades out since alpha is initially 75 it will be 255 once timeLeft reaches 0
        }
    }
}
