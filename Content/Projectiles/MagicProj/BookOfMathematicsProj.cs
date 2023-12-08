using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
    internal class BookOfMathematicsProj : ModProjectile
    {
        // This field is used in this projectie's custom AI
        private Vector2 initialCenter;

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
            Projectile.penetrate = 2;
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

		// This field is used as a counter for the wave motion
		public float sineTimer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void AI()
        {
            // Pick a frame randomly
            if (!setVisual)
            {
                Projectile.frame = Main.rand.Next(14);
                initialCenter = Projectile.Center;
                setVisual = true;
            }
            Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();

            sineTimer++;
            float sine = (float)Math.Sin(MathHelper.ToRadians(sineTimer * 3f));

            Vector2 offset = Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.PiOver2);
            float amplitude = 48f;
            offset *= sine *= amplitude;

            initialCenter += Projectile.velocity;
            Projectile.Center = initialCenter + offset;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.TintableDust, 0, 0, 100, default, 1f);
                dust.noGravity = true;
            }
        }
    }
}
