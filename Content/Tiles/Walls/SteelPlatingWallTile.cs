using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Walls
{
    internal class SteelPlatingWallTile : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            Main.wallLargeFrames[Type] = 1; // // Plating walls have this since their sprites are organised in a certain way
            DustType = DustID.Asphalt;
            AddMapEntry(new Color(21, 23, 22));
        }
    }
}
