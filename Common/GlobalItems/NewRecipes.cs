using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Tiles.CraftingStations;
using AssortedAdditions.Content.Items.Placeables.Ores;

namespace AssortedAdditions.Common.GlobalItems
{
    // This class is used for adding alternative recipes
    // or new recipes for vanilla items
    // For editing/deleting recipes look at ModifiedRecipes.cs
    internal class NewRecipes : GlobalItem
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

            //Demon/Crimson Altar items can also be crafted at Magic Work Bench
            // ************************************************************************************** //
            Recipe ancientHallowedGreaves = Recipe.Create(ItemID.AncientHallowedGreaves, 1);
            ancientHallowedGreaves.AddIngredient(ItemID.HallowedBar, 18);
            ancientHallowedGreaves.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            ancientHallowedGreaves.Register();

            Recipe ancientHallowedHeadgear = Recipe.Create(ItemID.AncientHallowedHeadgear, 1);
            ancientHallowedHeadgear.AddIngredient(ItemID.HallowedBar, 18);
            ancientHallowedHeadgear.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            ancientHallowedHeadgear.Register();

            Recipe ancientHallowedHelmet = Recipe.Create(ItemID.AncientHallowedHelmet, 1);
            ancientHallowedHelmet.AddIngredient(ItemID.HallowedBar, 18);
            ancientHallowedHelmet.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            ancientHallowedHelmet.Register();

            Recipe ancientHallowedHood = Recipe.Create(ItemID.AncientHallowedHood, 1);
            ancientHallowedHood.AddIngredient(ItemID.HallowedBar, 18);
            ancientHallowedHood.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            ancientHallowedHood.Register();

            Recipe ancientHallowedMask = Recipe.Create(ItemID.AncientHallowedMask, 1);
            ancientHallowedMask.AddIngredient(ItemID.HallowedBar, 18);
            ancientHallowedMask.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            ancientHallowedMask.Register();

            Recipe ancientHallowedPlateMail = Recipe.Create(ItemID.AncientHallowedPlateMail, 1);
            ancientHallowedPlateMail.AddIngredient(ItemID.HallowedBar, 24);
            ancientHallowedPlateMail.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            ancientHallowedPlateMail.Register();

            Recipe bloodySpine = Recipe.Create(ItemID.BloodySpine, 1);
            bloodySpine.AddIngredient(ItemID.Vertebrae, 15);
            bloodySpine.AddIngredient(ItemID.ViciousPowder, 30);
            bloodySpine.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            bloodySpine.Register();

            Recipe deerThing = Recipe.Create(ItemID.DeerThing, 1);
            deerThing.AddIngredient(ItemID.FlinxFur, 3);
            deerThing.AddIngredient(ItemID.CrimtaneOre, 5);
            deerThing.AddIngredient(ItemID.Lens, 1);
            deerThing.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            deerThing.Register();

            Recipe deerThing2 = Recipe.Create(ItemID.DeerThing, 1);
            deerThing2.AddIngredient(ItemID.FlinxFur, 3);
            deerThing2.AddIngredient(ItemID.DemoniteOre, 5);
            deerThing2.AddIngredient(ItemID.Lens, 1);
            deerThing2.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            deerThing2.Register();

            Recipe nightsEdge = Recipe.Create(ItemID.NightsEdge, 1);
            nightsEdge.AddIngredient(ItemID.BloodButcherer, 1);
            nightsEdge.AddIngredient(ItemID.Muramasa, 1);
            nightsEdge.AddIngredient(ItemID.BladeofGrass, 1);
            nightsEdge.AddIngredient(ItemID.FieryGreatsword, 1);
            nightsEdge.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            nightsEdge.Register();

            Recipe nightsEdge2 = Recipe.Create(ItemID.NightsEdge, 1);
            nightsEdge2.AddIngredient(ItemID.LightsBane, 1);
            nightsEdge2.AddIngredient(ItemID.Muramasa, 1);
            nightsEdge2.AddIngredient(ItemID.BladeofGrass, 1);
            nightsEdge2.AddIngredient(ItemID.FieryGreatsword, 1);
            nightsEdge2.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            nightsEdge2.Register();

            Recipe slimeCrown = Recipe.Create(ItemID.SlimeCrown, 1);
            slimeCrown.AddIngredient(ItemID.Gel, 20);
            slimeCrown.AddIngredient(ItemID.GoldCrown, 1);
            slimeCrown.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            slimeCrown.Register();

            Recipe slimeCrown2 = Recipe.Create(ItemID.SlimeCrown, 1);
            slimeCrown2.AddIngredient(ItemID.Gel, 20);
            slimeCrown2.AddIngredient(ItemID.PlatinumCrown, 1);
            slimeCrown2.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            slimeCrown2.Register();

            Recipe suspiciousLookingEye = Recipe.Create(ItemID.SuspiciousLookingEye, 1);
            suspiciousLookingEye.AddIngredient(ItemID.Lens, 6);
            suspiciousLookingEye.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            suspiciousLookingEye.Register();

            Recipe voidBag = Recipe.Create(ItemID.VoidLens, 1);
            voidBag.AddIngredient(ItemID.Bone, 30);
            voidBag.AddIngredient(ItemID.JungleSpores, 15);
            voidBag.AddIngredient(ItemID.ShadowScale, 30);
            voidBag.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            voidBag.Register();

            Recipe voidBag2 = Recipe.Create(ItemID.VoidLens, 1);
            voidBag2.AddIngredient(ItemID.Bone, 30);
            voidBag2.AddIngredient(ItemID.JungleSpores, 15);
            voidBag2.AddIngredient(ItemID.TissueSample, 30);
            voidBag2.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            voidBag2.Register();

            Recipe voidVault = Recipe.Create(ItemID.VoidVault, 1);
            voidVault.AddIngredient(ItemID.Bone, 15);
            voidVault.AddIngredient(ItemID.JungleSpores, 8);
            voidVault.AddIngredient(ItemID.ShadowScale, 15);
            voidVault.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            voidVault.Register();

            Recipe voidVault2 = Recipe.Create(ItemID.VoidVault, 1);
            voidVault2.AddIngredient(ItemID.Bone, 15);
            voidVault2.AddIngredient(ItemID.JungleSpores, 8);
            voidVault2.AddIngredient(ItemID.TissueSample, 15);
            voidVault2.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            voidVault2.Register();

            Recipe wormFood = Recipe.Create(ItemID.WormFood, 1);
            wormFood.AddIngredient(ItemID.VilePowder, 30);
            wormFood.AddIngredient(ItemID.RottenChunk, 15);
            wormFood.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            wormFood.Register();
            // ************************************************************************************** //

            Recipe shadowKey = Recipe.Create(ItemID.ShadowKey, 1);
            shadowKey.AddIngredient(ItemID.Obsidian, 20);
            shadowKey.AddIngredient(ItemID.SoulofNight, 5);
            shadowKey.AddTile(TileID.Anvils);
            shadowKey.Register();

            Recipe woodenBoomerang = Recipe.Create(ItemID.WoodenBoomerang);
            woodenBoomerang.AddRecipeGroup("Wood", 30);
            woodenBoomerang.AddIngredient(ItemID.Silk, 3);
            woodenBoomerang.AddTile(TileID.WorkBenches);
            woodenBoomerang.Register();
        }
    }
}
