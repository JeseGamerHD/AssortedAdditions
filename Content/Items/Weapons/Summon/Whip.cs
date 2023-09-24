using ModdingTutorial.Content.Projectiles.SummonProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Weapons.Summon
{
    internal class Whip : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<WhipProj>(), 12, 1, 5);
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Blue;
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Rope, 30);
            recipe.Register();
        }
    }
}
