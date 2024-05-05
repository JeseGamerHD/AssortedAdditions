using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles;

public class DuneRodBobber : ModProjectile // Bobber for the Dune Rod item
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.BobberWooden);
        DrawOriginOffsetY = -8;
    }
}
