using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Tiles.Blocks;

namespace ModdingTutorial.Content.Items.Placeables.Blocks
{
    internal class PaintBlock : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.value = Item.sellPrice(copper: 15);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<PaintBlockTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.StoneBlock, 2);
            recipe.AddTile(TileID.HeavyWorkBench);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.StoneSlab, 1);
            recipe2.AddTile(TileID.HeavyWorkBench);
            recipe2.Register();
        }
    }
}
