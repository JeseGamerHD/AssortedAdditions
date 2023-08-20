using ModdingTutorial.Content.Tiles.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.Systems
{
    // This class is used for adding the Magic Work Bench as the required crafting station
    // for vanilla items that would normally require bookcases or crystal ball
    internal class MagicWorkBenchCrafting : ModSystem
    {
        public override void PostAddRecipes()
        {
            // Loop through all the recipes
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasTile(TileID.Bookcases))
                {
                    recipe.RemoveTile(TileID.Bookcases);
                    recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
                }

                if(recipe.HasTile(TileID.CrystalBall))
                {
                    recipe.RemoveTile(TileID.CrystalBall);
                    recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
                }
            }
        }
    }
}
