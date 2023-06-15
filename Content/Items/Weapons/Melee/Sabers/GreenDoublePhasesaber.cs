using ModdingTutorial.Content.Projectiles.MeleeProj.DoublePhasesaberProj;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Melee.Sabers
{
    internal class GreenDoublePhasesaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to throw the weapon");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.value = Item.sellPrice(gold: 2);

            Item.DamageType = DamageClass.Melee;
            Item.damage = 60;
            Item.knockBack = 5f;
            Item.noMelee = true;

            Item.noUseGraphic = true; // Spawns a projectile that spins so hide item
            Item.channel = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.shoot = ModContent.ProjectileType<DoublePhasesaberSpin>();
            Item.shootSpeed = 5f;
        }

        // Left click will throw the saber
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        // Can either be spinning or thrown, not both
        // Also limits the amount that spawns to 1
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 & player.ownedProjectileCounts[Item.shoot] < 1)
            {
                Item.autoReuse = true;
                Item.shootSpeed = 12f;
                Item.channel = false;
                Item.shoot = ModContent.ProjectileType<DoublePhasesaberThrow>();
            }
            else if (player.ownedProjectileCounts[Item.shoot] < 1)
            {
                Item.shoot = ModContent.ProjectileType<DoublePhasesaberSpin>();
                Item.shootSpeed = 5f;
                Item.channel = true;
            }

            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GreenPhasesaber, 1);
            recipe.AddIngredient(ItemID.WirePipe, 1); // Junction Box from Mechanic NPC
            recipe.AddIngredient(ItemID.Lens, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
