using ModdingTutorial.Content.Projectiles.MagicProj.ConcoctionsProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Magic.Concoctions;

internal class ExplosiveConcoction : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("Will burst into flames");
        DisplayName.SetDefault("Volatile Concoction");
    }
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Magic;
        Item.damage = 35;
        Item.channel = true;
        Item.mana = 6;
        Item.rare = ItemRarityID.LightRed;
        Item.useTime = 27;
        Item.useAnimation = 27;
        Item.UseSound = SoundID.Item18;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(gold: 2, silver: 75);
        Item.shoot = ModContent.ProjectileType<ExplosiveConcoctionProj>();
        Item.shootSpeed = 15;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.InfernoPotion, 1);
        recipe.AddIngredient(ItemID.Fireblossom, 5);
        recipe.AddTile(TileID.ImbuingStation);
        recipe.Register();
    }
}
