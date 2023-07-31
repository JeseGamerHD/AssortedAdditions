using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Tiles.Walls;
using Terraria.ID;
using ModdingTutorial.Content.Items.Placeables.Blocks;

namespace ModdingTutorial.Content.Items.Placeables.Walls
{
    internal class ArtDecoWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<ArtDecoWallTile>());
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<ArtDecoBlock>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
