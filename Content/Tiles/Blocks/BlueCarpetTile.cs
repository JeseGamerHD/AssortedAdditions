using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Blocks
{
    internal class BlueCarpetTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(21, 48, 87));
            DustType = 172;
            HitSound = SoundID.Dig;
        }
    }
}
