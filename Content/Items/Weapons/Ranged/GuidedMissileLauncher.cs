using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Placeables;
using ModdingTutorial.Content.Items.Weapons.Ammo;
using ModdingTutorial.Content.Projectiles.RangedProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Ranged;

internal class GuidedMissileLauncher : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("Launches controllable missiles");
    }

    public override void SetDefaults()
    {
        Item.damage = 65;
        Item.knockBack = 4;
        Item.DamageType = DamageClass.Ranged;

        Item.width = 20;
        Item.height = 20;
        Item.channel = true;

        Item.useTime = 20;
        Item.useAnimation = 20;

        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;

        Item.value = Item.buyPrice(gold: 80);
        Item.rare = ItemRarityID.Yellow;

        Item.autoReuse = true;

        Item.shootSpeed = 10f;
        Item.shoot = ModContent.ProjectileType<GuidedMissileProj>();
        Item.useAmmo = ModContent.ItemType<GuidedMissile>();

        Item.UseSound = SoundID.Item10;
    }
    public override Vector2? HoldoutOffset() => new(-10, 2); // Alligns the sprite properly

    // Needed so that the projectile shoots from the muzzle
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
        if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
        {
            if (player.direction == -1) // If the player is facing left
            {
                muzzleOffset.Y += 15; // Adjust the offset
                muzzleOffset.X -= 20;
            }

            position += muzzleOffset;
        }
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<ControlChip>(), 1);
        recipe.AddIngredient(ItemID.IllegalGunParts, 2);
        recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 20);
        recipe.AddIngredient(ItemID.Megashark, 1);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}
