using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Tiles.Walls
{
    internal class ArtDecoWallTile : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Silt;
            AddMapEntry(new Color(27, 27, 27));
        }
    }
}
