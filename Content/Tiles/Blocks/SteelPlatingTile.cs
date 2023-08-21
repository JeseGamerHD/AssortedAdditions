using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Tiles.Blocks
{
    internal class SteelPlatingTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLargeFrames[Type] = 1; // Plating tiles have this since their sprites are organised in a certain way

            AddMapEntry(new Color(21, 23, 22));
            DustType = DustID.Asphalt;
            HitSound = SoundID.Tink;
        }
    }
}
