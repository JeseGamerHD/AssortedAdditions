using ModdingTutorial.Content.Projectiles.MagicProj.ConcoctionsProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Magic.Concoctions;

internal class IchorConcoction : ModItem
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
        Item.value = Item.sellPrice(gold: 5);
        Item.shoot = ModContent.ProjectileType<IchorConcoctionProj>();
        Item.shootSpeed = 15;
        Item.autoReuse = true;
        Item.noUseGraphic = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.SpelunkerPotion, 1);
        recipe.AddIngredient(ItemID.Ichor, 10);
        recipe.AddTile(TileID.ImbuingStation);
        recipe.Register();

        Recipe recipe2 = CreateRecipe(); // 2nd recipe for worlds without crimson
        recipe2.AddIngredient(ItemID.SpelunkerPotion, 1);
        recipe2.AddIngredient(ItemID.CursedFlame, 10);
        recipe2.AddIngredient(ItemID.YellowMarigold, 1);
        recipe2.AddTile(TileID.ImbuingStation);
        recipe2.Register();
    }
}
