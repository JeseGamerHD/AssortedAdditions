using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.MagicProj
{
    internal class DustbringerStaffProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 80;
            Projectile.penetrate = -1;
            Projectile.stepSpeed = 10; // Projectile moves along ground and steps up blocks
            Projectile.alpha = 50;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.aiStyle = 0;
        }

        bool setOnce = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Loop through the sprite frames
            int frameSpeed = 3;
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

            Projectile.rotation = Projectile.velocity.X * 0.02f; // Slight lean when moving
            Projectile.spriteDirection = Projectile.direction; // Faces left/right correctly

            // Projectile creates some wind sounds
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60; // This countsdown automatically

                SoundStyle WindGust = new SoundStyle("ModdingTutorial/Assets/Sounds/ProjectileSound/Dustbringer");
                WindGust = WindGust with {
                    Volume = 2.3f,
                    MaxInstances = 3
                };
                SoundEngine.PlaySound(WindGust, Projectile.position);
            }

            // Creates bunch of dust
            if (Main.rand.NextBool(1))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                        DustID.YellowTorch, 5, 5, 150, default, 1.5f);
                dust.noGravity = true;
                dust.noLight = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width * 2, Projectile.height * 2,
                            DustID.SandstormInABottle, 5, 5, 150, default, 2f);
                dust2.noGravity = true;
                dust2.noLight = true;
            }

            // Ensures that the projectile will move and not get stuck
            if (!setOnce)
            {
                Projectile.position.Y -= 5;

                if(player.direction == 1)
                {
                    Projectile.velocity.X = 7;
                    setOnce = true;
                }
                else
                {
                    Projectile.velocity.X = -7;
                    setOnce = true;
                }
            }

            // Constant gravity, projectile moves along ground
            Projectile.velocity.Y = 5;

            // When encountering a block, the projectile will go over it as long as its just 1 tile tall (for reference look at how the player and NPCs do this in game)
            Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {         
            if (Projectile.velocity.X != oldVelocity.X)
            {
                return true;
            }

            Projectile.timeLeft = 10;
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.YellowTorch, 0, 0, 100, default, 2f);
                dust.noGravity = true;
                dust.velocity.Y *= 1f;
                dust.velocity *= 10f;

                dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.SandstormInABottle, 0f, 0f, 100, default, 2f);
                dust.noGravity = true;
            }

            // Play a poof sound when projectile dies
            SoundEngine.PlaySound(new SoundStyle("ModdingTutorial/Assets/Sounds/ProjectileSound/DustbringerDeath"), Projectile.position);
        }
    }
}
