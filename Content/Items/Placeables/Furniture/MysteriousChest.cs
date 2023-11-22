using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AssortedAdditions.Content.Tiles.Furniture;
using AssortedAdditions.Content.Items.Placeables.Blocks;

namespace AssortedAdditions.Content.Items.Placeables.Furniture
{
    internal class MysteriousChest : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<MysteriousChestTile>());

            Item.width = 32;
            Item.height = 28;
            Item.value = Item.sellPrice(silver: 10);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MysteriousBrick>(), 5);
            recipe.AddIngredient(ItemID.GraniteBlock, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
