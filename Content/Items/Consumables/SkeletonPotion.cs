using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Projectiles;

namespace ModdingTutorial.Content.Items.Consumables
{
    internal class SkeletonPotion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.useAnimation = 15;
            Item.useTime = 15;

            Item.consumable = true;

            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Quest;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<SkeletonPotionProj>();
            Item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Bone, 30);
            recipe.AddIngredient(ItemID.TeleportationPotion, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }
}
