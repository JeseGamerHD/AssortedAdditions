using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.RangedProj
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

        public override void AI()
        { 
            Player player = Main.player[Projectile.owner]; // Used for returning the boomerang

            // Projectile will rotate when thrown
            Projectile.rotation += 0.4f;

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

            // Gradually slow down the projectile after 0.5 seconds in the air
            if (Projectile.timeLeft < 2970)
            {
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 5f)
                {
                    Projectile.velocity *= 0.97f;
                    Projectile.tileCollide = false;
                }
            }

            // Projectile will begin to ignore tiles once it hits something or after some time
            // Once that happens it should return to the player
            if (Projectile.tileCollide == false)
            {
                // Code that directs the boomerang back to the player
                Projectile.velocity = Projectile.DirectionTo(player.Center);

                // Timer that slowly accelerates and curves the boomerang back
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] > 5f)
                {
                    Projectile.velocity *= 8f; // Speed looks alright with this
                }

                // Once the projectile reaches the player, it will disappear
                if (Projectile.Hitbox.Intersects(player.Hitbox))
                {
                    Projectile.Kill();
                }
            }
        }

        // When the boomerang hits something, set tileCollide to false
        // This will make it return to the player in AI()
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.tileCollide = false;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Projectile.tileCollide = false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return false;
        }
    }
}
