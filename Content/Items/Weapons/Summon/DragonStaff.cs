using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using AssortedAdditions.Content.Buffs;
using AssortedAdditions.Content.Projectiles.SummonProj;
using AssortedAdditions.Content.Items.Misc;

namespace AssortedAdditions.Content.Items.Weapons.Summon;

public class DragonStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
    }
    public override void SetDefaults()
    {
        Item.damage = 30;
        Item.knockBack = 2f;
        Item.mana = 20;
        Item.width = 60;
        Item.height = 60;
        Item.useTime = 36;
        Item.useAnimation = 36;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(gold: 3);
        Item.rare = ItemRarityID.LightRed;
        Item.UseSound = SoundID.Item44;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Summon;
        Item.buffType = ModContent.BuffType<DragonStaffBuff>();
        Item.shoot = ModContent.ProjectileType<DragonMinionProj>();
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        position = Main.MouseWorld; // Spawns at cursor position
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        player.AddBuff(Item.buffType, 2);
        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile.originalDamage = Item.damage;

        return false;
    }

	public override void AddRecipes()
	{
		Recipe.Create(ModContent.ItemType<DragonStaff>())
			.AddIngredient(ItemID.PalladiumBar, 10)
			.AddIngredient(ModContent.ItemType<DragonScale>(), 6)
			.AddTile(TileID.Anvils)
			.Register();
	}
}