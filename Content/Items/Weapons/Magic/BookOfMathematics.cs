using ModdingTutorial.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Tiles.CraftingStations;

namespace ModdingTutorial.Content.Items.Weapons.Magic
{
    internal class BookOfMathematics : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.damage = 68;
            Item.knockBack = 4f;
            Item.shootSpeed = 4;
            Item.mana = 5;

            Item.noMelee = true; // Don't want the book to deal damage
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.UseSound = SoundID.Item17;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 4);
            Item.shoot = ModContent.ProjectileType<BookOfMathematicsProj>();
        }

        public override void AddRecipes()
        {
            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.SpellTome, 1);
            recipe2.AddIngredient(ItemID.Book, 25);
            recipe2.AddIngredient(ItemID.Ectoplasm, 5);
            recipe2.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            recipe2.Register();
        }
    }
}
