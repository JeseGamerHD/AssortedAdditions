using ModdingTutorial.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.Players
{
    // This class is used for doing stuff to the player when they wear an accessory
    // E.g. dodge damage when a shield is active
    internal class AccessoryFlags : ModPlayer
    {
        // These flags are set in the accessory file
        public bool isWearingEchoChamber;

        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            // When player has this buff they are using an accessory which grants a free hit (summons a projectile shield)
            if (Player.HasBuff(ModContent.BuffType<EchoChamberBuff>()))
            {
                Player.AddBuff(ModContent.BuffType<EchoChamberDebuff>(), 4500); // 75 second cooldown
                // When player is given the debuff the buff ends and the protective barrier disappears
                
                // Give the player some immunity frames so they won't be instantly damaged
                Player.immune = true;
                Player.immuneTime = 60;
                Player.immuneNoBlink = false;

                // Spawn some dust when the shield breaks
                for (int i = 0; i < 30; i++)
                {
                    Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height,
                        DustID.Electric, 0, 0, 100, default, 1f);
                    dust.noGravity = true;
                    dust.velocity *= 6f;

                    dust = Dust.NewDustDirect(Player.position, Player.width, Player.height,
                        DustID.Vortex, 0f, 0f, 100, default, 2f);
                    dust.noGravity = true;
                }

                // Play a shield breaking sound
                SoundStyle echoChamberBreak = new SoundStyle("ModdingTutorial/Assets/Sounds/ProjectileSound/EchoChamberBreak");
                echoChamberBreak = echoChamberBreak with
                {
                    Volume = 1f,
                    MaxInstances = 1
                };
                SoundEngine.PlaySound(echoChamberBreak, Player.position);

                return true;
            }
            
            return false;
        }

        // Reset flags
        public override void ResetEffects()
        {
            isWearingEchoChamber = false;
        }
    }
}
