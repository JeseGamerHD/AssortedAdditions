using System.Collections.Generic;
using ModdingTutorial.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles
{
    internal class EchoChamberProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 15;
        }

        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.penetrate = -1;
            Projectile.light = 0.5f;
            Projectile.aiStyle = 0;
            Projectile.alpha = 150;
            Projectile.timeLeft = 200;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override bool? CanCutTiles()
        {
            return false; // won't destroy plants, pots, etc...
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index); // is drawn over the player
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // This projectile is a shield that tanks one hit for the player
            // After taking that hit a cooldown (debuff) is activated removing the buff that keeps the projectile alive
            // Taking off the accessory which spawns this projectile will also remove the buff and thus kill the projectile
            if (player.HasBuff(ModContent.BuffType<EchoChamberDebuff>()) ||
                !player.HasBuff(ModContent.BuffType<EchoChamberBuff>()))
            {
                Projectile.Kill();
            }

            // Keep projectile on top of player
            Projectile.Center = player.Center; 

            // Loop through the sprite frames
            int frameSpeed = 5;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            // Play an activation sound for the player
            // This plays only once since timeLeft gets set to 2 after the first run of the AI() and is kept at 2
            if (player.whoAmI == Main.myPlayer && Projectile.timeLeft > 2)
            {
                SoundStyle echoChamberActivate = new SoundStyle("ModdingTutorial/Assets/Sounds/ProjectileSound/EchoChamberActivate");
                SoundEngine.PlaySound(echoChamberActivate, player.position);
            }

            Projectile.timeLeft = 2; // Stays alive as long as buff is active and there is no debuff
        }
    }
}
