using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles.MagicProj;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
    internal class DraconicTome : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Summons a flaming circle");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 29;
            Item.height = 30;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; // Don't want the book to deal damage

            Item.DamageType = DamageClass.Magic;
            Item.damage = 60;
            Item.knockBack = 6f;
            Item.crit = 4;
            Item.autoReuse = false;

            Item.UseSound = SoundID.Item20;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 5);

            Item.shoot = ModContent.ProjectileType<DraconicTomeProj>();
            Item.shootSpeed = 0; // Won't move
            Item.mana = 20;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }


        public override bool CanUseItem(Player player)
        {
            // Removes previous projectiles, only one of this can exist
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<DraconicTomeProj>())
                {
                    Main.projectile[i].Kill();
                }
            }

            return true;
        }
    }
}
