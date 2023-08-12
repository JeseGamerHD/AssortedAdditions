using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Items.Placeables.Ores;
using ModdingTutorial.Content.Items.Placeables.Blocks;

namespace ModdingTutorial.Common.GlobalItems
{
    // This class gives many of the vanilla accessories alternative sources
    // E.g. crafting recipes, sold by npcs...

    internal class ModifiedAccessories : GlobalItem
    {

        // MOVEMENT
        // *******************
        // HermesBoots
        // DuneriderBoots (SandBoots)
        // FlurryBoots
        // SailfishBoots
        // WaterWalkingBoots
        // IceSkates
        // Aglet
        // ObsidianRose
        // CloudinaBottle
        // FrogLeg
        // DivingHelmet
        // ClimbingClaws
        // ShoeSpikes
        // FlyingCarpet

        public override void AddRecipes()
        {
            // MOVEMENT
            // ******************************************************************************
            Recipe hermesBoots = Recipe.Create(ItemID.HermesBoots, 1);
            hermesBoots.AddIngredient(ItemID.Leather, 6);
            hermesBoots.AddIngredient(ItemID.Feather, 6);
            hermesBoots.AddIngredient(ItemID.BlackThread, 2);
            hermesBoots.AddTile(TileID.TinkerersWorkbench);
            hermesBoots.Register();

            Recipe duneriderBoots = Recipe.Create(ItemID.SandBoots, 1); // DuneriderBoots
            duneriderBoots.AddIngredient(ItemID.Leather, 6);
            duneriderBoots.AddIngredient(ItemID.DesertFossil, 20);
            duneriderBoots.AddIngredient(ItemID.BlackThread, 2);
            duneriderBoots.AddTile(TileID.TinkerersWorkbench);
            duneriderBoots.Register();

            Recipe flurryBoots = Recipe.Create(ItemID.FlurryBoots, 1);
            flurryBoots.AddIngredient(ItemID.Leather, 6);
            flurryBoots.AddIngredient(ItemID.IceBlock, 20);
            flurryBoots.AddIngredient(ItemID.BlackThread, 2);
            flurryBoots.AddTile(TileID.TinkerersWorkbench);
            flurryBoots.Register();

            Recipe sailfishBoots = Recipe.Create(ItemID.SailfishBoots, 1);
            sailfishBoots.AddIngredient(ItemID.Leather, 6);
            sailfishBoots.AddIngredient(ItemID.ReefBlock, 20);
            sailfishBoots.AddIngredient(ItemID.BlackThread, 2);
            sailfishBoots.AddTile(TileID.TinkerersWorkbench);
            sailfishBoots.Register();

            Recipe waterWalkingBoots = Recipe.Create(ItemID.WaterWalkingBoots, 1);
            waterWalkingBoots.AddIngredient(ItemID.Leather, 6);
            waterWalkingBoots.AddIngredient(ItemID.SharkFin, 2);
            waterWalkingBoots.AddIngredient(ItemID.BlackThread, 2);
            waterWalkingBoots.AddTile(TileID.TinkerersWorkbench);
            waterWalkingBoots.Register();

            Recipe iceSkates = Recipe.Create(ItemID.IceSkates, 1);
            iceSkates.AddIngredient(ItemID.Leather, 6);
            iceSkates.AddIngredient(ItemID.SilverBar, 10);
            iceSkates.AddIngredient(ItemID.BlackThread, 2);
            iceSkates.AddTile(TileID.TinkerersWorkbench);
            iceSkates.Register();

            Recipe iceSkates2 = Recipe.Create(ItemID.IceSkates, 1);
            iceSkates2.AddIngredient(ItemID.Leather, 6);
            iceSkates2.AddIngredient(ItemID.TungstenBar, 10);
            iceSkates2.AddIngredient(ItemID.BlackThread, 2);
            iceSkates2.AddTile(TileID.TinkerersWorkbench);
            iceSkates2.Register();

            Recipe aglet = Recipe.Create(ItemID.Aglet, 1);
            aglet.AddIngredient(ItemID.GoldBar, 6);
            aglet.AddTile(TileID.TinkerersWorkbench);
            aglet.Register();

            Recipe aglet2 = Recipe.Create(ItemID.Aglet, 1);
            aglet2.AddIngredient(ItemID.PlatinumBar, 6);
            aglet2.AddTile(TileID.TinkerersWorkbench);
            aglet2.Register();

            Recipe obsidianRose = Recipe.Create(ItemID.ObsidianRose, 1);
            obsidianRose.AddIngredient(ItemID.JungleRose, 1);
            obsidianRose.AddIngredient(ItemID.Obsidian, 10);
            obsidianRose.AddTile(TileID.TinkerersWorkbench);
            obsidianRose.Register();

            Recipe cloudInABottle = Recipe.Create(ItemID.CloudinaBottle, 1);
            cloudInABottle.AddIngredient(ItemID.Bottle, 1);
            cloudInABottle.AddIngredient(ItemID.Cloud, 20);
            cloudInABottle.AddTile(TileID.TinkerersWorkbench);
            cloudInABottle.Register();

            Recipe frogLeg = Recipe.Create(ItemID.FrogLeg, 1);
            frogLeg.AddIngredient(ItemID.Frog, 1);
            frogLeg.Register();

            Recipe divingHelmet = Recipe.Create(ItemID.DivingHelmet, 1);
            divingHelmet.AddIngredient(ItemID.CopperBar, 12);
            divingHelmet.AddIngredient(ItemID.Glass, 20);
            divingHelmet.AddTile(TileID.TinkerersWorkbench);
            divingHelmet.Register();

            Recipe divingHelmet2 = Recipe.Create(ItemID.DivingHelmet, 1);
            divingHelmet2.AddIngredient(ItemID.TinBar, 12);
            divingHelmet2.AddIngredient(ItemID.Glass, 20);
            divingHelmet2.AddTile(TileID.TinkerersWorkbench);
            divingHelmet2.Register();

            Recipe climbingClaws = Recipe.Create(ItemID.ClimbingClaws, 1);
            climbingClaws.AddIngredient(ModContent.ItemType<SteelBar>(), 10);
            climbingClaws.AddIngredient(ItemID.IronBar, 6);
            climbingClaws.AddTile(TileID.TinkerersWorkbench);
            climbingClaws.Register();

            Recipe climbingClaws2 = Recipe.Create(ItemID.ClimbingClaws, 1);
            climbingClaws2.AddIngredient(ModContent.ItemType<SteelBar>(), 10);
            climbingClaws2.AddIngredient(ItemID.LeadBar, 6);
            climbingClaws2.AddTile(TileID.TinkerersWorkbench);
            climbingClaws2.Register();

            Recipe shoeSpikes = Recipe.Create(ItemID.ClimbingClaws, 1);
            shoeSpikes.AddIngredient(ModContent.ItemType<SteelBar>(), 10);
            shoeSpikes.AddIngredient(ItemID.IronBar, 6);
            shoeSpikes.AddTile(TileID.TinkerersWorkbench);
            shoeSpikes.Register();

            Recipe shoeSpikes2 = Recipe.Create(ItemID.ClimbingClaws, 1);
            shoeSpikes2.AddIngredient(ModContent.ItemType<SteelBar>(), 10);
            shoeSpikes2.AddIngredient(ItemID.LeadBar, 6);
            shoeSpikes2.AddTile(TileID.TinkerersWorkbench);
            shoeSpikes2.Register();

            Recipe flyingCarpet = Recipe.Create(ItemID.FlyingCarpet, 1);
            flyingCarpet.AddIngredient(ModContent.ItemType<RedCarpet>(), 15);
            flyingCarpet.AddIngredient(ItemID.SoulofFlight, 2);
            flyingCarpet.AddTile(TileID.TinkerersWorkbench);
            flyingCarpet.Register();
        }
    }
}
