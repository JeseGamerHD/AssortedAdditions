using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles
{
    internal class SkeletonPotionProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 32;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.aiStyle = 2;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item107, Projectile.position);
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.Asphalt, 0, 0, 100, default, 3.5f);
                dust.noGravity = true;
                dust.velocity *= 3f;


                dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.Smoke, 0, 0, 100, default, 3.5f);
                dust.noGravity = true;
                dust.velocity *= 7f;


                dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.Obsidian, 0f, 0f, 100, default, 2.5f);
                dust.noGravity = true;
            }

            // Don't spawn on multiplayer clients
            // If one is already alive, teleport it to the player
            // else spawn a new one
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (SummonOrTeleport())
                {
                    NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, NPCID.SkeletonMerchant);
                }

            }
        }

        private bool SummonOrTeleport()
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.SkeletonMerchant)
                {
                    Main.npc[i].position = Projectile.position;
                    return false;
                }
            }

            return true;
        }
    }
}
