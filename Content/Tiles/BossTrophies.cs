﻿using AssortedAdditions.Content.Items.Placeables.Trophies;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace AssortedAdditions.Content.Tiles
{
    internal class BossTrophies : ModTile
    {
        // OLD CODE, still working for single trophy. Simplified version in HauntTrophyTile.cs
        // The previous idea was to have all trophies in one sprite / tile but that method does not work
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            DustType = 7;
            TileID.Sets.DisableSmartCursor[Type] = true;

            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Trophy");
            AddMapEntry(new Color(120, 85, 60), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int item = 0;
            switch (frameX / 54)
            {
                case 0:
                    item = ModContent.ItemType<FireDragonTrophy>();
                    break;
            }

            if (item > 0)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), i * 16, j * 16, 48, 48, item);
            }
        }
    }

}
