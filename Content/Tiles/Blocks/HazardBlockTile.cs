using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Tiles.Blocks
{
    internal class HazardBlockTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(255, 255, 32));
            DustType = DustID.IchorTorch;
            HitSound = SoundID.Tink;
        }
    }
}
