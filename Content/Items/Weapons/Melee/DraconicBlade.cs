using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles.MeleeProj;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Melee
{
    internal class DraconicBlade : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 70;
            Item.height = 72;
            //Item.scale = 0.75f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 47;
            Item.knockBack = 6;
            Item.crit = 5;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<DraconicBladeProj>();
        }

        // Shoot a projectile every 2 swings
        int shoot;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            shoot++;
            if(shoot % 2 == 0)
            {
                return true;
            }
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //Emit dusts when the sword is swung
            int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width - 60, hitbox.Height - 60,
          DustID.Flare, 0, 0, 150, default, 3f);

            Main.dust[dust].noGravity = true;

            // if player is facing right
            if (player.direction == 1)
            {
                if (Main.dust[dust].velocity.X < 0)
                {
                    Main.dust[dust].velocity.X *= -1; // Stop dust from going left
                }
                Main.dust[dust].velocity.X *= 4; // Speed up dust to the right
            }
            // If player is facing left
            else
            {
                if (Main.dust[dust].velocity.X > 0)
                {
                    Main.dust[dust].velocity.X *= -1; // Stop dust from going right
                }
                Main.dust[dust].velocity.X *= 4; // Speed up dust to the left
            }
        }

        // Sets enemies on fire
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }
    }
}
