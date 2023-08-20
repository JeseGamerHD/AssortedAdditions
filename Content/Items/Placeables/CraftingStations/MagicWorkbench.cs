using Terraria.ID;
using Terraria.ModLoader;
using ModdingTutorial.Content.Tiles.CraftingStations;
using Terraria;

namespace ModdingTutorial.Content.Items.Placeables.CraftingStations
{
    internal class MagicWorkbench : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 23;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.maxStack = 9999;

            Item.consumable = true;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.createTile = ModContent.TileType<MagicWorkbenchTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrystalBall, 1);
            recipe.AddIngredient(ItemID.WorkBench, 1);
            recipe.AddIngredient(ItemID.Book, 15);
            recipe.AddIngredient(ItemID.StoneBlock, 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
