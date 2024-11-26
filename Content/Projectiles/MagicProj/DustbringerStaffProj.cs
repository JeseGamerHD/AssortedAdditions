using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MagicProj
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
            Projectile.height = 64;
            Projectile.penetrate = -1;
            Projectile.alpha = 50;

            Projectile.DamageType = DamageClass.Magic;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.aiStyle = 0;
        }

        public override void AI()
        {
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

            // Projectile creates some wind sounds
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60; // This countsdown automatically

                SoundStyle WindGust = new SoundStyle("AssortedAdditions/Assets/Sounds/ProjectileSound/Dustbringer");
                WindGust = WindGust with
                {
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

            MovementLogic();
			Projectile.rotation = Projectile.velocity.X * 0.02f; // Slight lean when moving
			Projectile.spriteDirection = Projectile.direction; // Faces left/right correctly
        }

        private ref float Timer => ref Projectile.ai[0];
		private ref float FlightTimer => ref Projectile.ai[1];
		
        // Taken from vanilla aiStyle 145
        // Tried to rename some variables and do some cleanup
		private void MovementLogic()
        {
			float maxFlightTime = 300f;
			if (FlightTimer >= 16f && Timer < maxFlightTime - 15f)
			{
				Timer = maxFlightTime - 15f;
			}

			Timer++;
			if (Timer >= maxFlightTime)
			{
				Projectile.Kill();
			}

			Vector2 vector2 = new Vector2(0f, Projectile.Bottom.Y - Projectile.Top.Y);
			vector2.X = vector2.Y * 0.2f;
			int tileWidth = 16;
			int maxTileheight = 160;

			Vector2 vector3 = new Vector2(Projectile.Center.X - (tileWidth / 2), Projectile.position.Y + Projectile.height - maxTileheight);
			if (Collision.SolidCollision(vector3, tileWidth, maxTileheight) || Collision.WetCollision(vector3, tileWidth, maxTileheight))
			{
				if (Projectile.velocity.Y > 0f)
				{
					Projectile.velocity.Y = 0f;
				}
				else if (Projectile.velocity.Y > -4f)
				{
					Projectile.velocity.Y -= 2f;
				}
				else
				{
					Projectile.velocity.Y -= 4f;
					FlightTimer += 2f;
				}

				if (Projectile.velocity.Y < -16f)
				{
					Projectile.velocity.Y = -16f;
				}
				return;
			}

			// Cant be negative
			FlightTimer -= 1f;
			if (FlightTimer < 0f)
			{
				FlightTimer = 0f;
			}

			if (Projectile.velocity.Y < 0f)
			{
				Projectile.velocity.Y = 0f;
			}
			else if (Projectile.velocity.Y < 4f)
			{
				Projectile.velocity.Y += 2f;
			}
			else
			{
				Projectile.velocity.Y += 4f;
			}

			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}

			if (Timer < maxFlightTime - 30f)
			{
				for (int j = 0; j < 1; j++)
				{
					float amount = Main.rand.NextFloat();
					Vector2 vector4 = new Vector2(MathHelper.Lerp(0.1f, 1f, Main.rand.NextFloat()), MathHelper.Lerp(-1f, 0.9f, amount));
					vector4.X *= MathHelper.Lerp(2.2f, 0.6f, amount);
					vector4.X *= -1f;
				}
			}		
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
            SoundEngine.PlaySound(new SoundStyle("AssortedAdditions/Assets/Sounds/ProjectileSound/DustbringerDeath"), Projectile.position);
        }
    }
}
