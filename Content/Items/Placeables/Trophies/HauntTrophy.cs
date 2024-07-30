﻿using Terraria;
using AssortedAdditions.Content.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Placeables.Trophies
{
	internal class HauntTrophy : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<HauntTrophyTile>();
			Item.placeStyle = 0;
		}
	}
}
