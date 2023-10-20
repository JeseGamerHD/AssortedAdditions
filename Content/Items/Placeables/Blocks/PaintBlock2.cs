using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Tiles.Blocks;

namespace ModdingTutorial.Content.Items.Placeables.Blocks
{
    internal class PaintBlock2 : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.value = Item.sellPrice(copper: 20);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<PaintBlockTile2>();
        }
    }
}
