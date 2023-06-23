using ModdingTutorial.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Melee
{
    internal class SteelSword : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 35;
            Item.knockBack = 6;
            Item.crit = 2;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 15);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
