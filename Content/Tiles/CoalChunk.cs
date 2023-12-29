using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles
{
    internal class CoalChunk : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 200;
            TileID.Sets.Ore[Type] = true;

            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Coal");
            AddMapEntry(new Color(55, 52, 52), name);

            DustType = DustID.Asphalt;
            HitSound = SoundID.Tink;

            MineResist = 1.3f;
            MinPick = 40;
        }
    }
}
