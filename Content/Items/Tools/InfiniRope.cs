using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Tools
{
    internal class InfiniRope : ModItem
    {
        public override void SetStaticDefaults()
        {

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 12));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;          
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                Item.shoot = ProjectileID.RopeCoil;
                Item.useTime = 15;
                Item.useAnimation = 15;
                Item.shootSpeed = 10;
                Item.consumable = false;
                Item.autoReuse = false;
            }
            else
            {
                Item.DefaultToPlaceableTile(TileID.Rope);
                Item.useTime = 10;
                Item.useAnimation = 15;
                Item.consumable = false;
                Item.shoot = ProjectileID.None;
                Item.shootSpeed = 0;
            }

            return base.CanUseItem(player);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Rope, 999);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
