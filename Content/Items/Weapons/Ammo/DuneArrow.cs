using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using ModdingTutorial.Content.Projectiles.RangedProj;
using ModdingTutorial.Content.Items.Placeables;

namespace ModdingTutorial.Content.Items.Weapons.Ammo
{
    internal class DuneArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 34;
            Item.damage = 12;
            Item.maxStack = 999;
            Item.shootSpeed = 6.5f;

            Item.consumable = true;

            Item.value = Item.sellPrice(copper: 20);
            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<DuneArrowProj>();
            Item.ammo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(25); // Makes 25 per craft
            recipe.AddIngredient(ModContent.ItemType<DuneOre>(), 1);
            recipe.AddRecipeGroup("Wood");
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
