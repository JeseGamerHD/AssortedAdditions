using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;

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

            // Check if one is already spawned
            // if not, spawn a new one, if yes then kill the previous one and spawn a new one
            
            int canSpawn = CheckIfExists();
			if (canSpawn == -1)
            {
				// Don't spawn on multiplayer clients
				if (Main.netMode != NetmodeID.MultiplayerClient)
                {
					int npc = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, NPCID.SkeletonMerchant);
					NetMessage.SendData(MessageID.SyncNPC, number: npc); // Spawns on the server, needs to be synced
				}
            }
            else
            {
				if (Main.netMode == NetmodeID.SinglePlayer)
                {
					DespawnSkeletonMerchant(canSpawn);

					int npc = NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, NPCID.SkeletonMerchant);
					NetMessage.SendData(MessageID.SyncNPC, number: npc); // Spawns on the server, needs to be synced
				}
                else
                {
                    var message = Mod.GetPacket();
                    message.Write((byte)AssortedAdditions.MessageType.DespawnSkeletonMerchant);
                    message.Write(canSpawn);
                    message.Send();

					var message2 = Mod.GetPacket(); // Send a message to the server telling that it should spawn the merchant
					message2.Write((byte)AssortedAdditions.MessageType.SpawnSkeletonMerchant);
					message2.Write((int)Projectile.position.X);
					message2.Write((int)Projectile.position.Y);
					message2.Send();
				}
			}
        }

        public static void DespawnSkeletonMerchant(int index)
        {
			Main.npc[index].life = 0;
			Main.npc[index].active = false;

			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.SyncNPC, number: index);
			}
		}

        private int CheckIfExists()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.SkeletonMerchant)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
