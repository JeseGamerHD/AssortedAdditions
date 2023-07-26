using ModdingTutorial.Content.Tiles.Blocks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Placeables.Blocks
{
    internal class GreenCarpet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.value = Item.sellPrice(copper: 20);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<GreenCarpetTile>();
        }
    }
}
