using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Tiles
{
    internal class DuneOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 300;
            Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently

            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Dune Ore");
            AddMapEntry(new Color(255, 115, 0), name);

            DustType = DustID.Gold;
            HitSound = SoundID.Tink;

            MineResist = 1.3f;
            MinPick = 65; // Alternative to hellstone
        }
    }
}
