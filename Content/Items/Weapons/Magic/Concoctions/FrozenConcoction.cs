using ModdingTutorial.Content.Items.Placeables.Ores;
using ModdingTutorial.Content.Projectiles.MagicProj.ConcoctionsProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Magic.Concoctions;

internal class FrozenConcoction : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Magic;
        Item.damage = 60;
        Item.channel = true;
        Item.mana = 6;
        Item.rare = ItemRarityID.LightRed;
        Item.useTime = 27;
        Item.useAnimation = 27;
        Item.UseSound = SoundID.Item18;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(gold: 10);
        Item.shoot = ModContent.ProjectileType<FrozenConcoctionProj>();
        Item.shootSpeed = 10f;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.FrostCore, 1);
        recipe.AddIngredient(ItemID.Shiverthorn, 5);
        recipe.AddIngredient(ModContent.ItemType<Permafrost>(), 5);
        recipe.AddTile(TileID.ImbuingStation);
        recipe.Register();
    }
}
