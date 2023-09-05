using ModdingTutorial.Content.Projectiles.MagicProj.ConcoctionsProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Magic.Concoctions;

internal class BouncingConcoction : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Magic;
        Item.damage = 80;
        Item.channel = true;
        Item.mana = 6;
        Item.rare = ItemRarityID.LightRed;
        Item.useTime = 27;
        Item.useAnimation = 27;
        Item.UseSound = SoundID.Item18;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(gold: 5);
        Item.shoot = ModContent.ProjectileType<BouncingConcoctionProj>();
        Item.shootSpeed = 6f;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.NightOwlPotion, 1);
        recipe.AddIngredient(ItemID.CursedFlame, 10);
        recipe.AddTile(TileID.ImbuingStation);
        recipe.Register();

        Recipe recipe2 = CreateRecipe(); // 2nd recipe for worlds without corruption
        recipe2.AddIngredient(ItemID.NightOwlPotion, 1);
        recipe2.AddIngredient(ItemID.Ichor, 10);
        recipe2.AddIngredient(ItemID.GreenMushroom, 1);
        recipe2.AddTile(TileID.ImbuingStation);
        recipe2.Register();
    }
}
