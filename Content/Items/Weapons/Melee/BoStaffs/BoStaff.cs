using ModdingTutorial.Content.Projectiles.MeleeProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Weapons.Melee.BoStaffs
{
    // Other types of BoStaff inherit this class, only changing the recipe depending on the wood type
    // Projectile visuals change inside BoStaffSpin depending on which staff is being used
    internal class BoStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 82;
            Item.height = 82;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.damage = 4;
            Item.knockBack = 4;
            Item.shootSpeed = 5f;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.White;
            Item.shoot = ModContent.ProjectileType<BoStaffSpin>();
            Item.value = Item.sellPrice(copper: 20);
            Item.DamageType = DamageClass.Melee;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }

    // All the other variants are under here:
    internal class MahoganyBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RichMahogany, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    internal class AshBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AshWood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    internal class BorealBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BorealWood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    internal class DynastyBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DynastyWood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    internal class EbonWoodBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Ebonwood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    internal class PalmBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PalmWood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    internal class PearlWoodBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Pearlwood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    internal class ShadeWoodBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Shadewood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    internal class SpookyBoStaff : BoStaff
    {
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpookyWood, 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
