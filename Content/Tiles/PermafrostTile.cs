using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace AssortedAdditions.Content.Tiles
{
    internal class PermafrostTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][TileID.IceBlock] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 300;
            Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(0, 174, 255), name);

            DustType = DustID.Ice;
            HitSound = SoundID.Tink;

            MineResist = 3f;
            MinPick = 150; // Alternative to titanium/adamantite
        }
    }
}
