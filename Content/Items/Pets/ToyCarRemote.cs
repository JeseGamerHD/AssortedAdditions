using ModdingTutorial.Content.Buffs;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles.PetProj;

namespace ModdingTutorial.Content.Items.Pets
{
    internal class ToyCarRemote : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 40;
            Item.DefaultToVanitypet(ModContent.ProjectileType<ToyCar>(), ModContent.BuffType<ToyCarBuff>());
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Buff applied here, the buff then spawns the pet projectile
            player.AddBuff(Item.buffType, 2);

            return false;
        }
    }
}
