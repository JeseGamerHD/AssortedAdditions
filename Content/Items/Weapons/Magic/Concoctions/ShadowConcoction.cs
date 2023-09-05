using ModdingTutorial.Content.Projectiles.MagicProj.ConcoctionsProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Magic.Concoctions;

internal class ShadowConcoction : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Magic;
        Item.damage = 65;
        Item.channel = true;
        Item.mana = 6;
        Item.rare = ItemRarityID.LightRed;
        Item.useTime = 27;
        Item.useAnimation = 27;
        Item.UseSound = SoundID.Item18;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(gold: 10);
        Item.shoot = ModContent.ProjectileType<ShadowConcoctionProj>();
        Item.shootSpeed = 15;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.GravitationPotion, 1);
        recipe.AddIngredient(ItemID.SoulofNight, 15);
        recipe.AddIngredient(ItemID.Deathweed, 5);
        recipe.AddTile(TileID.ImbuingStation);
        recipe.Register();
    }
}
