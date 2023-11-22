using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AssortedAdditions.Content.Tiles.Blocks
{
    internal class PaintBlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(255, 255, 255));
            DustType = DustID.Smoke;
            HitSound = SoundID.Tink;
        }

        public override bool KillSound(int i, int j, bool fail)
        {
            // When the block breaks, play a different sound
            if (!fail)
            {
                SoundEngine.PlaySound(new SoundStyle("AssortedAdditions/Assets/Sounds/Tiles/PaintTileBreak"));
            }

            // Otherwise play the hit sound
            return base.KillSound(i, j, fail);
        }
    }
}
