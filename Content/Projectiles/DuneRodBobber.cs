using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles;

public class DuneRodBobber : ModProjectile // Bobber for the Dune Rod item
{
    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ProjectileID.BobberWooden);
        DrawOriginOffsetY = -8;
    }

    public override void ModifyFishingLine(ref Vector2 lineOriginOffset, ref Color lineColor)
    {
        lineOriginOffset = new Vector2(44, -29); // Where the line is drawn from
        lineColor = Color.LightGray; // Sets the fishing line's color.
    }
}
