using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Walls
{
    internal class HazardWallTile : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.IchorTorch;
            AddMapEntry(new Color(255, 255, 32));
        }
    }
}
