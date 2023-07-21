using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Buffs;
using ModdingTutorial.Content.Projectiles.SummonProj;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace ModdingTutorial.Content.Items.Weapons.Summon
{
    internal class SunScepter : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.mana = 20;
            Item.damage = 36;
            Item.knockBack = 4f;

            Item.noMelee = true;

            Item.rare = ItemRarityID.Pink;
            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;
            Item.value = Item.sellPrice(gold: 5);
            Item.buffType = ModContent.BuffType<SunScepterBuff>();
            Item.shoot = ModContent.ProjectileType<SunScepterMinion>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld; // Spawns at cursor position
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);

            // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            // Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
            return false;
        }
    }
}
