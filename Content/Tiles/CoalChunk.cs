﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Tiles
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

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Coal");
            AddMapEntry(new Color(55, 52, 52), name);

            DustType = DustID.Asphalt;
            ItemDrop = ModContent.ItemType<Items.Placeables.CoalChunk>();
            HitSound = SoundID.Tink;

            MineResist = 1.3f;
            MinPick = 40;
        }
    }
}
