﻿using Terraria;
using AssortedAdditions.Content.Tiles.Banners;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Placeables.Banners
{
	internal class PhantomMageBanner : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.useAnimation = 15;
			Item.useTime = 10;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue; // All banners are blue and sell for 2 silver
			Item.value = Item.sellPrice(silver: 2);
			Item.createTile = ModContent.TileType<PhantomMageBannerTile>();
		}
	}
}
