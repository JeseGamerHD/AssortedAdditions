using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Walls
{
    internal class CheckerWallTile : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Silt;
            AddMapEntry(new Color(38, 38, 24));
        }
    }
}
