using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles.PetProj;
using AssortedAdditions.Content.Buffs.Pets;

namespace AssortedAdditions.Content.Items.Pets
{
    internal class FireDragonEgg : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Master;
            Item.master = true;
            Item.value = Item.sellPrice(gold: 3);

            Item.DefaultToVanitypet(ModContent.ProjectileType<BabyFireDragon>(), ModContent.BuffType<BabyFireDragonBuff>());
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Buff applied here, the buff then spawns the pet projectile
            player.AddBuff(Item.buffType, 2);

            return false;
        }
    }
}
