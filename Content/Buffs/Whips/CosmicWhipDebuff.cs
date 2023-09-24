using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Buffs.Whips
{
    internal class CosmicWhipDebuff : ModBuff
    {

        public override string Texture => "ModdingTutorial/Content/Buffs/Whips/MotivatorDebuff";

        public static readonly int TagDamage = 15;

        public override void SetStaticDefaults()
        {
            // This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
            // Other mods may check it for different purposes.
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
            Main.debuff[Type] = true;
        }
    }

    public class CosmicWhipDebuffNPC : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            // Only player attacks should benefit from this buff, hence the NPC and trap checks.
            if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated)
                return;

            // SummonTagDamageMultiplier scales down tag damage for some specific minion and sentry projectiles for balance purposes.
            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
            if (npc.HasBuff<CosmicWhipDebuff>())
            {
                // Apply a flat bonus to every hit
                modifiers.FlatBonusDamage += MotivatorDebuff.TagDamage * projTagMultiplier;
            }
        }
    }
}
