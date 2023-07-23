using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using ModdingTutorial.Content.Items.Placeables.Ores;

namespace ModdingTutorial.Content.Items.Weapons.Ranged
{
    internal class DuneBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 36;

            Item.damage = 30;
            Item.shootSpeed = 8;
            Item.useTime = 20;
            Item.useAnimation = 20;
            
            Item.noMelee = true;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5;
            Item.value = Item.sellPrice(silver: 60);

            Item.useAmmo = AmmoID.Arrow; // Also determines which arrow projectile is fired
            Item.shoot = ProjectileID.WoodenArrowFriendly;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DuneBar>(), 9);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
