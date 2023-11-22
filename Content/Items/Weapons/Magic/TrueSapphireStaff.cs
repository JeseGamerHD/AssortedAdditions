using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Tiles.CraftingStations;
using AssortedAdditions.Content.Projectiles.MagicProj;
using AssortedAdditions.Content.Items.Misc;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
    internal class TrueSapphireStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 80;
            Item.knockBack = 8;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.shootSpeed = 3;
            Item.mana = 15;
            Item.scale = 2f;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 4);
            Item.UseSound = SoundID.Item43;
            Item.shoot = ModContent.ProjectileType<TrueSapphireStaffProj>();

            Item.autoReuse = true;
            Item.noMelee = true;
            Item.staff[Type] = true; // Makes the player hold the item like gem staves
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset; // Projectiles come out of the muzzle properly using this
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LargeSapphire, 1);
            recipe.AddIngredient(ItemID.SapphireStaff, 1);
            recipe.AddIngredient(ModContent.ItemType<MagicEssence>(), 5);
            recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            recipe.Register();
        }
    }
}
