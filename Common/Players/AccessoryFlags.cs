using ModdingTutorial.Content.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
        public bool isWearingMedkit;

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

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            // When wearing a medkit and its buff has not been activated
            if (isWearingMedkit && !Player.HasBuff(ModContent.BuffType<MedkitBuff>()))
            {
                // 50/50 chance to activate
                if(Main.rand.NextBool())
                {
                    // Activate the buff and heal the player
                    Player.AddBuff(ModContent.BuffType<MedkitBuff>(), 18000);
                    Player.Heal(150);
                    return false; // Stop player from dying
                }
                else
                {
                    return true;
                }
            }

            // In any other case the player should die
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        // Reset flags
        public override void ResetEffects()
        {
            isWearingEchoChamber = false;
            isWearingMedkit = false;
        }
    }
}
