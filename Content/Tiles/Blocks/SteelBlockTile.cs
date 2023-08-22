using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Tiles.Blocks
{
    internal class SteelBlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(21, 23, 22));
            DustType = DustID.Asphalt;
            HitSound = SoundID.Tink;
        }
    }
}
