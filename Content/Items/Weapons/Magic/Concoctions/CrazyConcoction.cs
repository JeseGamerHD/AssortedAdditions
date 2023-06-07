using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles.MagicProj.ConcoctionsProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Magic.Concoctions;

internal class CrazyConcoction : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("What could go wrong...");
    }
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Magic;
        Item.damage = 80;
        Item.channel = true;
        Item.mana = 6;
        Item.rare = ItemRarityID.Yellow;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.UseSound = SoundID.Item18;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(gold: 18);
        Item.shoot = ModContent.ProjectileType<LightConcoctionProj>();
        Item.shootSpeed = 15;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        int potion = Main.rand.Next(1, 7); // Random number between 1-6.
        switch (potion)
        {
            case 1: // Thrown projectile depends on the random number
                type = ModContent.ProjectileType<ExplosiveConcoctionProj>();
                break;

            case 2:
                type = ModContent.ProjectileType<BouncingConcoctionProj>();
                break;

            case 3:
                type = ModContent.ProjectileType<FrozenConcoctionProj>();
                break;

            case 4:
                type = ModContent.ProjectileType<IchorConcoctionProj>();
                break;

            case 5:
                type = ModContent.ProjectileType<ShadowConcoctionProj>();
                break;

            case 6:
                type = ModContent.ProjectileType<LightConcoctionProj>();
                break;
        }

    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<ExplosiveConcoction>(), 1);
        recipe.AddIngredient(ModContent.ItemType<BouncingConcoction>(), 1);
        recipe.AddIngredient(ModContent.ItemType<IchorConcoction>(), 1);
        recipe.AddIngredient(ModContent.ItemType<FrozenConcoction>(), 1);
        recipe.AddIngredient(ModContent.ItemType<ShadowConcoction>(), 1);
        recipe.AddIngredient(ModContent.ItemType<LightConcoction>(), 1);
        recipe.AddTile(TileID.AlchemyTable);
        recipe.Register();
    }
}
