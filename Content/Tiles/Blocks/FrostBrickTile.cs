using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Blocks
{
    internal class FrostBrickTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileShine[Type] = 975;

            AddMapEntry(new Color(0, 174, 255));
            DustType = DustID.Ice;
            HitSound = SoundID.Tink;
        }
    }
}
