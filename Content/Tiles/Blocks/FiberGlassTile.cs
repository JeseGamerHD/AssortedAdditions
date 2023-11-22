using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Blocks
{
    internal class FiberGlassTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;

            AddMapEntry(new Color(118, 200, 232));
            DustType = 13;
            HitSound = SoundID.Shatter;
        }
    }
}
