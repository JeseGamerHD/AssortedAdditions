using Microsoft.Xna.Framework;
using Terraria.GameContent.Drawing;

namespace ModdingTutorial.Content.Projectiles.MeleeProj
{
    internal class CosmicBladeProj : SwingTrailBase
    {
        public override void SetDefaults()
        {
            base.SetDefaults();

            ScaleMultiplier = 0.3f;
            ScaleAdderValue = 0.9f;

            PrimaryDustColor = Color.HotPink;
            SecondaryDustColor = Color.White;
            DustType = 15;

            DoHitEffect = true;
            HitEffect = ParticleOrchestraType.NightsEdge;

            SwingBackColor = new(159, 5, 255);
            SwingMiddleColor = new(134, 5, 255);
            SwingFrontColor = new(109, 5, 255); 
        }
    }
}
