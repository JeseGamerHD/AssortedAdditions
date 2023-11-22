using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Blocks
{
    internal class CheckerBlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(38, 38, 24));
            DustType = DustID.Silt;
            HitSound = SoundID.Tink;
        }
    }
}
