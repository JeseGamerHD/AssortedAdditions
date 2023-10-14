using ModdingTutorial.Common.Configs;
using ModdingTutorial.Content.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.Systems
{
    // This class is used for editing vanilla recipes
    internal class ModifiedRecipes : ModSystem
    {
        public override void PostAddRecipes()
        {
            // The player can disable these changes if they want to in the mod configs
            if(ModContent.GetInstance<VanillaChangeToggle>().ModifiedRecipesToggle)
            {
                for (int i = 0; i < Recipe.numRecipes; i++)
                {
                    Recipe recipe = Main.recipe[i];

                    // If recipe results in FrostHelmet and the recipe used TitaniumBar as an ingredient
                    if (recipe.HasResult(ItemID.FrostHelmet) && recipe.TryGetIngredient(ItemID.TitaniumBar, out Item titaniumBar))
                    {
                        recipe.RemoveIngredient(titaniumBar); // Remove the ingredient
                        recipe.AddIngredient(ModContent.ItemType<FrostBar>(), 10); // Replace it
                    }
                    else if (recipe.HasResult(ItemID.FrostHelmet) && recipe.TryGetIngredient(ItemID.AdamantiteBar, out Item adamantiteBar))
                    {
                        recipe.DisableRecipe(); // Here just disable the alt recipe, this mod only has one recipe for this armor
                    }

                    else if (recipe.HasResult(ItemID.FrostBreastplate) && recipe.TryGetIngredient(ItemID.TitaniumBar, out titaniumBar))
                    {
                        recipe.RemoveIngredient(titaniumBar);
                        recipe.AddIngredient(ModContent.ItemType<FrostBar>(), 20);
                    }
                    else if (recipe.HasResult(ItemID.FrostBreastplate) && recipe.TryGetIngredient(ItemID.AdamantiteBar, out adamantiteBar))
                    {
                        recipe.DisableRecipe();
                    }

                    else if (recipe.HasResult(ItemID.FrostLeggings) && recipe.TryGetIngredient(ItemID.TitaniumBar, out titaniumBar))
                    {
                        recipe.RemoveIngredient(titaniumBar);
                        recipe.AddIngredient(ModContent.ItemType<FrostBar>(), 16);
                    }
                    else if (recipe.HasResult(ItemID.FrostLeggings) && recipe.TryGetIngredient(ItemID.AdamantiteBar, out adamantiteBar))
                    {
                        recipe.DisableRecipe();
                    }
                }
            }
        }
    }
}
