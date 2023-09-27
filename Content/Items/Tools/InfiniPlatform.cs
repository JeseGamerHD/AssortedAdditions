using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Tiles.CraftingStations;
using ModdingTutorial.Content.Items.Misc;

namespace ModdingTutorial.Content.Items.Tools
{
    internal class InfiniPlatform : ModItem
    {
        public override void SetStaticDefaults()
        {

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 9));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(TileID.Platforms);
            Item.consumable = false;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WoodPlatform, 999);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
