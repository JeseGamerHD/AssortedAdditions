using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles
{
    internal class FrostBobber : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BobberWooden);
            DrawOriginOffsetY = -8;
        }
    }
}
