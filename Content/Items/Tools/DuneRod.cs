using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using ModdingTutorial.Content.Items.Placeables.Ores;

namespace ModdingTutorial.Content.Items.Tools;

public class DuneRod : ModItem
{
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.WoodFishingPole); // Most defaults can be copied
        Item.value = Item.sellPrice(silver: 30);
        Item.rare = ItemRarityID.Orange;
        Item.fishingPole = 35; // Fishing power
        Item.shootSpeed = 14f; // Speed at which the bobbers are launched.
        
        Item.shoot = ModContent.ProjectileType<Projectiles.DuneRodBobber>(); // Has an unique bobber
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<DuneBar>(), 8);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}