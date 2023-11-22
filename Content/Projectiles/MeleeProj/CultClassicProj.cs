using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
    internal class CultClassicProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            /* The following sets are only applicable to yoyo that use aiStyle 99. */

            // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
            // Vanilla values range from 3f (Wood) to 16f (Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;

            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player. 
            // Vanilla values range from 130f (Wood) to 400f (Terrarian), and defaults to 200f.
            // Divide the value by 16 to get the reach in tiles
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 380f; // 23.75 tiles

            // YoyosTopSpeed is top speed of the yoyo Projectile.
            // Vanilla values range from 9f (Wood) to 17.5f (Terrarian), and defaults to 10f.
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 15.5f;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = -1;

            Projectile.friendly = true;

            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.DamageType = DamageClass.MeleeNoSpeed; // MeleeNoSpeed means the item will not scale with attack speed.
        }

        // notes for aiStyle 99: 
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.

        private bool setOnce = false;
        public override bool PreAI()
        {
            // Spawn a child projectile (only once per yoyo)
            // The child (CultClassicOrb.cs) will stay on top of this projectile
            Player owner = Main.player[Projectile.owner];
            if (!setOnce)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0, ModContent.ProjectileType<CultClassicOrb>(), Projectile.damage, Projectile.knockBack, owner.whoAmI);
                Main.projectile[proj].ai[0] = Projectile.whoAmI;
                setOnce = true;
            }

            return true;
        }
    }
}
