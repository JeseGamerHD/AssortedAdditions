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
        for (int i = 0; i < Main.projectile.Length; i++) // Looking through player's summons
        {
            Projectile proj = Main.projectile[i];
            if (proj.active && proj.type == ModContent.ProjectileType<DragonMinionProj>() && proj.owner == player.whoAmI)
            {  // Only one of this minion can exist
               // This section despawns the first minion when summoning another
                proj.active = false;
                break;
            }

            // If a different minion exists it will also be replaced by the dragon if no slots are available
            // If there are slots dragon will spawn without replacing/removing minions
            /* Currently acts weirdly, the dragon will remove a previous minion even when there is a slot available
               this only happens if no dragon has been summoned yet. Probably has something to do with the for loop... */
            if (proj.type != ModContent.ProjectileType<DragonMinionProj>() && proj.active && proj.owner == player.whoAmI)
            {
                if(player.numMinions < player.maxMinions + 1)
                {
                    proj.active = false;
                    break;
                }
            }
        }

        player.AddBuff(Item.buffType, 2);
        var projectile2 = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile2.originalDamage = Item.damage;

        return false;

    }
}