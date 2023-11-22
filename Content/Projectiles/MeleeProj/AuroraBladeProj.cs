using Microsoft.Xna.Framework;
using Terraria.GameContent.Drawing;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
    public class AuroraBladeProj : SwingTrailBase
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            ScaleMultiplier = 0.6f;
            ScaleAdderValue = 1f;

            PrimaryDustColor = Color.SkyBlue;
            SecondaryDustColor = Color.White;
            DustType = 15;

            DoHitEffect = true;
            HitEffect = ParticleOrchestraType.SilverBulletSparkle;

            SwingBackColor = new(12, 212, 108);
            SwingMiddleColor = new(12, 212, 196);
            SwingFrontColor = new(14, 52, 189);
        }
    }
}