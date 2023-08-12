using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Items.Placeables.Ores;

namespace ModdingTutorial.Common.GlobalItems
{
    // This class is used for tweaking item recipes
    internal class ModifiedRecipes : GlobalItem
    {
        public override void AddRecipes()
        {
            Recipe torch = Recipe.Create(ItemID.Torch, 10);
            torch.AddIngredient(ModContent.ItemType<CoalChunk>());
            torch.AddRecipeGroup("Wood");
            torch.Register();

            Recipe leather = Recipe.Create(ItemID.Leather, 1);
            leather.AddIngredient(ItemID.Vertebrae, 5);
            leather.AddTile(TileID.WorkBenches);
            leather.Register();

            Recipe leather2 = Recipe.Create(ItemID.Leather, 3);
            leather2.AddIngredient(ItemID.Bunny, 1);
            leather2.AddTile(TileID.WorkBenches);
            leather2.Register();

            Recipe leather3 = Recipe.Create(ItemID.Leather, 3);
            leather3.AddRecipeGroup("Squirrels", 1);
            leather3.AddTile(TileID.WorkBenches);
            leather3.Register();
        }
    }
}
