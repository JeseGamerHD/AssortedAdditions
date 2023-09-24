using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles.RangedProj;
using Terraria.DataStructures;

namespace ModdingTutorial.Content.Items.Weapons.Ranged
{
    internal class DraconicBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Shoots a fire bolt");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 58;
            Item.damage = 55;
            Item.knockBack = 5;
            Item.crit = 4;
            Item.shootSpeed = 13;
            Item.useTime = 18;
            Item.useAnimation = 18;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5;
            Item.value = Item.sellPrice(gold: 5);
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<DraconicBowProj>(); 
        }

        public override Vector2? HoldoutOffset() => new(-2, 0); // Alligns the sprite properly

        private int shotsFired; // Used for determining when to consume ammo
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            // Consume ammo every 3 shots, also on the 1st shot
            if(shotsFired % 3 == 0)
            {
                return true;
            }

            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shotsFired++; // Counts up every time a projectile is shot
            return true;
        }

        // Always shoots DragonicBowProj, without this arrow would determine the projectile
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<DraconicBowProj>();
        }
    }
}
