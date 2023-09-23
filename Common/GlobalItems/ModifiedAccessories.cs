using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Items.Placeables.Ores;
using ModdingTutorial.Content.Items.Placeables.Blocks;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Accessories;

namespace ModdingTutorial.Common.GlobalItems
{
    // This class gives many of the vanilla accessories alternative sources (crafting recipes)
    // Also modifies some behaviour
    internal class ModifiedAccessories : GlobalItem
    {
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
            climbingClaws.AddRecipeGroup("IronBar", 6);
            climbingClaws.AddTile(TileID.TinkerersWorkbench);
            climbingClaws.Register();

            Recipe shoeSpikes = Recipe.Create(ItemID.ShoeSpikes, 1);
            shoeSpikes.AddIngredient(ModContent.ItemType<SteelBar>(), 10);
            shoeSpikes.AddRecipeGroup("IronBar", 6);
            shoeSpikes.AddTile(TileID.TinkerersWorkbench);
            shoeSpikes.Register();

            Recipe flyingCarpet = Recipe.Create(ItemID.FlyingCarpet, 1);
            flyingCarpet.AddIngredient(ModContent.ItemType<RedCarpet>(), 15);
            flyingCarpet.AddIngredient(ItemID.SoulofFlight, 2);
            flyingCarpet.AddTile(TileID.TinkerersWorkbench);
            flyingCarpet.Register();

            // HEALTH AND MANA
            // ******************************************************************************
            Recipe bandOfRegeneration = Recipe.Create(ItemID.BandofRegeneration, 1);
            bandOfRegeneration.AddIngredient(ItemID.LifeCrystal, 1);
            bandOfRegeneration.AddIngredient(ItemID.Ruby, 3);
            bandOfRegeneration.AddTile(TileID.TinkerersWorkbench);
            bandOfRegeneration.Register();

            Recipe bandOfStarpower = Recipe.Create(ItemID.BandofStarpower, 1);
            bandOfStarpower.AddIngredient(ItemID.ManaCrystal, 1);
            bandOfStarpower.AddIngredient(ItemID.Sapphire, 3);
            bandOfStarpower.AddTile(TileID.TinkerersWorkbench);
            bandOfStarpower.Register();


            // COMBAT
            // ******************************************************************************
            Recipe obsidianRose = Recipe.Create(ItemID.ObsidianRose, 1);
            obsidianRose.AddIngredient(ItemID.JungleRose, 1);
            obsidianRose.AddIngredient(ItemID.Obsidian, 10);
            obsidianRose.AddTile(TileID.TinkerersWorkbench);
            obsidianRose.Register();

            Recipe shackle = Recipe.Create(ItemID.Shackle, 1);
            shackle.AddIngredient(ItemID.Chain, 15);
            shackle.AddTile(TileID.TinkerersWorkbench);
            shackle.Register();

            Recipe feralClaws = Recipe.Create(ItemID.FeralClaws, 1);
            feralClaws.AddIngredient(ItemID.ClimbingClaws, 1);
            feralClaws.AddIngredient(ItemID.Vine, 10);
            feralClaws.AddTile(TileID.TinkerersWorkbench);
            feralClaws.Register();

            Recipe magicQuiver = Recipe.Create(ItemID.MagicQuiver, 1);
            magicQuiver.AddIngredient(ItemID.Leather, 6);
            magicQuiver.AddIngredient(ItemID.PixieDust, 6);
            magicQuiver.AddTile(TileID.TinkerersWorkbench);
            magicQuiver.Register();

            Recipe magmaStone = Recipe.Create(ItemID.MagmaStone, 1);
            magmaStone.AddIngredient(ItemID.StoneBlock, 30);
            magmaStone.AddIngredient(ItemID.LavaBucket, 1);
            magmaStone.AddTile(TileID.TinkerersWorkbench);
            magmaStone.Register();

            Recipe pocketMirror = Recipe.Create(ItemID.PocketMirror, 1);
            pocketMirror.AddIngredient(ItemID.GoldBar, 6);
            pocketMirror.AddIngredient(ItemID.MagicMirror, 1);
            pocketMirror.AddTile(TileID.TinkerersWorkbench);
            pocketMirror.Register();

            Recipe pocketMirror2 = Recipe.Create(ItemID.PocketMirror, 1);
            pocketMirror2.AddIngredient(ItemID.PlatinumBar, 6);
            pocketMirror2.AddIngredient(ItemID.MagicMirror, 1);
            pocketMirror2.AddTile(TileID.TinkerersWorkbench);
            pocketMirror2.Register();

            Recipe sharkToothNecklace = Recipe.Create(ItemID.SharkToothNecklace, 1);
            sharkToothNecklace.AddIngredient(ModContent.ItemType<SharkTooth>(), 3);
            sharkToothNecklace.AddIngredient(ItemID.Chain, 5);
            sharkToothNecklace.AddTile(TileID.TinkerersWorkbench);
            sharkToothNecklace.Register();

            Recipe ankhCharm = Recipe.Create(ItemID.AnkhCharm, 1);
            ankhCharm.AddIngredient(ItemID.ArmorBracing);
            ankhCharm.AddIngredient(ModContent.ItemType<MedicatedDressing>(), 1);
            ankhCharm.AddIngredient(ItemID.ThePlan);
            ankhCharm.AddIngredient(ItemID.CountercurseMantra);
            ankhCharm.AddIngredient(ItemID.ReflectiveShades);
            ankhCharm.AddTile(TileID.TinkerersWorkbench);
            ankhCharm.Register();

            // CONSTRUCTION
            // ******************************************************************************
            Recipe presserator = Recipe.Create(ItemID.ActuationAccessory, 1); // Presserator
            presserator.AddRecipeGroup("IronBar", 10);
            presserator.AddIngredient(ItemID.Actuator, 25);
            presserator.AddTile(TileID.TinkerersWorkbench);
            presserator.Register();
        }

        // Prevent the player from equipping some similar accessories to prevent cheesing through limits in
        // Modded accessories' CanAccessoryBeEquippedWith().
        // E.g Player could equip both necklaces and then swap one for a medkit and get insane speeds
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if( (equippedItem.type == ItemID.PanicNecklace && incomingItem.type == ItemID.SweetheartNecklace ) 
                || equippedItem.type == ItemID.SweetheartNecklace && incomingItem.type == ItemID.PanicNecklace)
            {
                return false;
            }

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }
    }
}
