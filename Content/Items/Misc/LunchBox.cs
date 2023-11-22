using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;

namespace AssortedAdditions.Content.Items.Misc
{
    internal class LunchBox : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 34;
            Item.useAnimation = 40;
            Item.useTime = 40;

            Item.consumable = false;

            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item2;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.sellPrice(silver: 75);
        }

        public override void UpdateInventory(Player player)
        {
            player.AddBuff(BuffID.WellFed2, 2);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new(Mod, "Tooltip0", "[c/FFF014:Gives the Plenty Satisfied buff while in any personal inventory]"));
            tooltips.Add(new(Mod, "Tooltip1", "[c/FFF014:Overrides other food buffs]"));
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("Fruit");
            recipe.AddRecipeGroup("AssortedAdditions:Drinks");
            recipe.AddIngredient(ItemID.CobaltBar, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddRecipeGroup("Fruit", 3);
            recipe2.AddRecipeGroup("AssortedAdditions:Drinks");
            recipe2.AddIngredient(ItemID.PalladiumBar, 5);
            recipe2.AddTile(TileID.WorkBenches);
            recipe2.Register();
        }
    }
}
