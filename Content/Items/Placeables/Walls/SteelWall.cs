using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Tiles.Walls;
using AssortedAdditions.Content.Items.Placeables.Blocks;

namespace AssortedAdditions.Content.Items.Placeables.Walls
{
    internal class SteelWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<SteelWallTile>());
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<SteelBlock>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
