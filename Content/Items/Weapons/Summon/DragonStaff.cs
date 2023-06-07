using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ModdingTutorial.Content.Buffs;
using ModdingTutorial.Content.Projectiles.SummonProj;

namespace ModdingTutorial.Content.Items.Weapons.Summon;

public class DragonStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        Tooltip.SetDefault("Summons a dragon to fight for you.");
        ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 40;
        Item.knockBack = 1f;
        Item.mana = 20;
        Item.width = 42;
        Item.height = 42;
        Item.useTime = 36;
        Item.useAnimation = 36;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(gold: 4);
        Item.rare = ItemRarityID.Orange;
        Item.UseSound = SoundID.Item44;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Summon;
        Item.buffType = ModContent.BuffType<DragonStaffBuff>();
        Item.shoot = ModContent.ProjectileType<DragonMinionProj>();
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        position = Main.MouseWorld;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        player.AddBuff(Item.buffType, 2);

        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile.originalDamage = Item.damage;

        return false;
    }
}