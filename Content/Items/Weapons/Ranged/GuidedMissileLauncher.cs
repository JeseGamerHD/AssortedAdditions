using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Placeables.Ores;
using AssortedAdditions.Content.Items.Weapons.Ammo;
using AssortedAdditions.Content.Projectiles.RangedProj;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Weapons.Ranged;

internal class GuidedMissileLauncher : ModItem
{
    public override void SetDefaults()
    {
		Item.width = 56;
        Item.height = 28;
		Item.useTime = 20;
		Item.useAnimation = 20;
		Item.damage = 65;
        Item.knockBack = 4;
        Item.shootSpeed = 10f;

        Item.channel = true;
		Item.autoReuse = true;
		Item.noMelee = true;

		Item.useStyle = ItemUseStyleID.Shoot;
		Item.DamageType = DamageClass.Ranged;
		Item.value = Item.buyPrice(gold: 80);
        Item.rare = ItemRarityID.Yellow;
        Item.shoot = ModContent.ProjectileType<GuidedMissileProj>();
        Item.useAmmo = ModContent.ItemType<GuidedMissile>();
        Item.UseSound = SoundID.Item10;
    }
    public override Vector2? HoldoutOffset() => new(-20, -5); // Alligns the sprite properly

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
        recipe.AddIngredient(ModContent.ItemType<ControlChip>(), 1);
        recipe.AddIngredient(ItemID.IllegalGunParts, 2);
        recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 20);
        recipe.AddIngredient(ItemID.Megashark, 1);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}
