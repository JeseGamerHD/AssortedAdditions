using System.Collections.Generic;
using AssortedAdditions.Common.Configs;
using AssortedAdditions.Content.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.Systems
{
    // This class is used for editing vanilla recipes
    internal class ModifiedRecipes : ModSystem
    {
        public override void PostAddRecipes()
        {
			// Due to (optional) ore progression changes, some vanilla items should be craftable using regular anvils instead
			HashSet<int> earlyHardModeAnvilItems = new HashSet<int> 
			{ 
				ItemID.AngelWings,
				ItemID.BatWings,
				ItemID.BeeWings,
				ItemID.BoneWings,
				ItemID.DemonWings,
				ItemID.FairyWings,
				ItemID.FrozenWings,
				ItemID.HarpyWings,
				ItemID.BluePhaseblade,
				ItemID.GreenPhaseblade,
				ItemID.OrangePhaseblade,
				ItemID.PurplePhaseblade,
				ItemID.RedPhaseblade,
				ItemID.WhitePhaseblade,
				ItemID.YellowPhaseblade,
				ItemID.Chik,
				ItemID.CoolWhip,
				ItemID.CrystalBullet,
				ItemID.CursedArrow,
				ItemID.CursedBullet,
				ItemID.DaoofPow,
				ItemID.HolyArrow,
				ItemID.IchorArrow,
				ItemID.MeteorStaff,
				ItemID.OnyxBlaster,
				ItemID.SkyFracture,
				ItemID.SpiritFlame,
				ItemID.MechanicalWorm,
				ItemID.MechanicalEye,
				ItemID.MechanicalSkull
			};

			for (int i = 0; i < Recipe.numRecipes; i++)
			{
				Recipe recipe = Main.recipe[i];

				// The player can disable these changes if they want to in the mod configs
				if (ModContent.GetInstance<ServerSidedToggles>().ModifiedRecipesToggle)
				{
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

				if (earlyHardModeAnvilItems.Contains(recipe.createItem.type))
				{
					recipe.RemoveTile(TileID.MythrilAnvil);
					recipe.AddTile(TileID.Anvils);
				}
			}
		}
	}
}
