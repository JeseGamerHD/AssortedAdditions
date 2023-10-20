using ModdingTutorial.Content.Tiles.Blocks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Placeables.Blocks
{
    internal class FiberGlass : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.value = Item.sellPrice(copper: 60);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<FiberGlassTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe.AddIngredient(ItemID.Glass, 3);
            recipe.AddIngredient(ItemID.Gel, 1);
            recipe.AddTile(TileID.GlassKiln);
            recipe.Register();
        }
    }
}
