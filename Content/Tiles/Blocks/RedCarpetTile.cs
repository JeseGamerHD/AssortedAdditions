using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Blocks
{
    internal class RedCarpetTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(202, 21, 21));
            DustType = DustID.Crimson;
            HitSound = SoundID.Dig;
        }
    }
}
