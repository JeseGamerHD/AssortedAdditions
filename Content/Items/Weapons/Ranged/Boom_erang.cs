using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AssortedAdditions.Content.Projectiles.RangedProj;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class Boom_erang : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 32;
            Item.damage = 35;
            Item.knockBack = 6f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.shootSpeed = 12f;

            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;

			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<Boom_erangProj>();
        }

        // Only one can be shot at a time
        // New one can be thrown once the first one has returned
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WoodenBoomerang);
            recipe.AddIngredient(ItemID.Dynamite, 30);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
