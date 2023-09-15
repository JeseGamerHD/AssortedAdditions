using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.MagicProj
{
    internal class BookOfMathematicsProj : ModProjectile
    {
        // This field is used in this projectie's custom AI
        private Vector2 initialCenter;

        // This field is used as a counter for the wave motion
        private int sineTimer;

        // This field "offsets" the progress along the wave
        private float waveOffset;

        // For setting the frame
        private bool setVisual;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 14;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 24;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;
            Projectile.aiStyle = 0;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Magic;
        }

        // This projectile updates its position manually
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            initialCenter = Projectile.Center;
        }

        public override void AI()
        {
            // Pick a frame randomly
            if (!setVisual)
            {
                Projectile.frame = Main.rand.Next(14);
                setVisual = true;
            }
            Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();

            // NOTE: this stuff is from example mod
            // I got no clue how sine waves etc work...
            float velocityLength = Projectile.velocity.Length();

            if (velocityLength > 0)
            {
                // How long one wave should be, measured in pixels
                // A stride of 300 pixels is 18.75 tiles
                float waveStride = 300f;

                float waveProgress = sineTimer * velocityLength / waveStride + waveOffset;  // 1 for each full sine wave
                float radians = waveProgress * MathHelper.TwoPi;  // Convert the wave progress into an angle for MathF.Sin()
                float sine = MathF.Sin(radians) * Projectile.direction;

                // Using the calculated sine value, generate an offset used to position the projectile on the wave
                // The offset should be perpendicular to the velocity direction, hence the RotatedBy call
                Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.PiOver2 * -1);

                // How wide the wave should be, times two
                // An amplitude of 32 pixels is 2 tiles, meaning the total wave width is 64 pixels, or 4 tiles
                float waveAmplitude = 48;

                // Having the projectiles spawn offset from the player might not be ideal.  To fix that, let's reduce the amplitude when the projectile is freshly spawned
                if (sineTimer < 20)
                {
                    // Up to 1/3rd of a second (20/60 = 1/3), make the amplitude grow to the intended size
                    float factor = 1f - sineTimer / 20f;
                    waveAmplitude *= 1f - factor * factor;
                }

                // Get the offset used to adjust the projectile's position
                offset *= sine * waveAmplitude;

                // Update the position manually since ShouldUpdatePosition returns false
                initialCenter += Projectile.velocity;
                Projectile.Center = initialCenter + offset;

                // Update the rotation used to draw the projectile
                // This projectile should act as if it were moving along the sine wave.
                // The rotation can be calculated using the cosine value, which is the slope of the sine wave, and then stretching/squishing the slope based on the amplitude and wave frequency.
                // The slope needs to be inverted due to negative slopes being "upwards" in Terraria's world space.
                float cosine = MathF.Cos(radians) * Projectile.direction;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathF.Atan(-1 * cosine * waveAmplitude * velocityLength / waveStride);
            }
            else
            {
                // Failsafe for when the velocity is 0
                Projectile.rotation = 0;
            }

            sineTimer++;
        }

        public override void Kill(int timeLeft)
        {
            // On death create dust
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.TintableDust, 0, 0, 100, default, 1f);
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // In order to make the projectile draw correctly, that will need to be done manually
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle frame = texture.Frame(1, 14, 0, Projectile.frame);
            Vector2 rotationOrigin;
            float rotation;
            SpriteEffects effects;

            if (Projectile.direction == -1)
            {
                rotationOrigin = new Vector2(5, 5);
                rotation = Projectile.rotation + MathHelper.Pi;
                effects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                rotationOrigin = new Vector2(13, 5);
                rotation = Projectile.rotation;
                effects = SpriteEffects.None;
            }

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), rotation, rotationOrigin, Projectile.scale, effects, 0);

            return false;
        }
    }
}
