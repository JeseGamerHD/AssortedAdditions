using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Walls
{
    internal class PaintWallTile2 : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Asphalt;
            AddMapEntry(new Color(0, 0, 0));
        }
    }
}
