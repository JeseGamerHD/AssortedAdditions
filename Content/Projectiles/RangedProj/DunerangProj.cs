using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
    internal class DunerangProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.timeLeft = 3000; // Important, used for slowing down projectile and returning it
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1; // So that the boomerang doesn't die once hitting enemies

            Projectile.aiStyle = 0;
        }


        private bool turnBack = false; // Once true, boomerang will return
        private bool setValues = false; // Once true, values have been set
        private float speed;
        private float speedLimit;
        private float slowDown = 0.2f;
        private float rotationDirection;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];


            // Set the speed depending on vector's length
            if (setValues == false)
            {
                speed = Projectile.velocity.Length();
                speedLimit = speed;

                // Rotates right/left depending on which direction it was thrown
                if (player.direction == 1)
                    rotationDirection = 0.4f;
                else
                    rotationDirection = -0.4f;

                setValues = true; // Values are set only once
            }

            // Projectile will rotate when thrown
            Projectile.rotation += rotationDirection;

            // Spawn some dust particles
            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.YellowTorch, 0, 0, 100, Color.Yellow, 2f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.GemTopaz, 3, 3, 100, Color.Yellow, 1f);
                dust2.noGravity = true;
            }

            // Sound effect
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8; // This countsdown automatically
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            // Start returning
            if (turnBack == true)
            {
                Projectile.tileCollide = false; // Returns through tiles

                // Code that directs the boomerang back to the player (Polar Vector)
                Projectile.velocity = new Vector2((float)Math.Cos((player.Center - Projectile.Center).ToRotation()),
                                                 (float)Math.Sin((player.Center - Projectile.Center).ToRotation())) * speed;
                speed += slowDown; // Needs to be accelerated since it has been slowed down first

                // Limit to how much it can accelerate
                if (speed > speedLimit)
                {
                    speed = speedLimit;
                }

                // Once the projectile reaches the player, it will disappear
                if (Projectile.Hitbox.Intersects(player.Hitbox))
                {
                    Projectile.Kill();
                }
            }
            else // Begin to slow down after not hitting anything
            {
                Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                speed -= slowDown;
            }
            if (speed < 1f) // Once it has slowed down enough...
            {
                turnBack = true; // ...turn back
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            turnBack = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            turnBack = true;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); // Dust from tile when hit
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center); // Tile hit sound
            return false;
        }
    }
}
