using ModdingTutorial.Content.Items.Placeables.Blocks;
using ModdingTutorial.Content.Tiles.Walls;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Placeables.Walls
{
    internal class PaintWall2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<PaintWallTile2>());
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<PaintBlock2>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
