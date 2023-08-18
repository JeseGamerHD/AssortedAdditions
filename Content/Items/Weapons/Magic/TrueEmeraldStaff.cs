using ModdingTutorial.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Items.Misc;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Items.Weapons.Magic
{
    internal class TrueEmeraldStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 58;
            Item.knockBack = 6;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.shootSpeed = 9;
            Item.mana = 5;
            Item.scale = 2f;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 4);
            Item.UseSound = SoundID.Item43;
            Item.shoot = ModContent.ProjectileType<TrueEmeraldStaffProj>();

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
            recipe.AddIngredient(ItemID.LargeEmerald, 1);
            recipe.AddIngredient(ItemID.EmeraldStaff, 1);
            recipe.AddIngredient(ModContent.ItemType<MagicEssence>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
