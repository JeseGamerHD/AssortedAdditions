using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Tiles.Walls
{
    internal class SteelWallTile : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Asphalt;
            AddMapEntry(new Color(21, 23, 22));
        }
    }
}
