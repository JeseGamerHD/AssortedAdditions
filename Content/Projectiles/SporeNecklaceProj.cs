using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles
{
    internal class SporeNecklaceProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 14;
            Projectile.timeLeft = 1800;
            Projectile.light = 0.25f;
            Projectile.penetrate = 6;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;

            Projectile.aiStyle = ProjAIStyleID.GroundProjectile;
            AIType = ProjectileID.SpikyBall;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override void PostAI()
        {
            if (Projectile.timeLeft < 60)
            {
                Projectile.alpha += 5;
            }
        }
    }
}
