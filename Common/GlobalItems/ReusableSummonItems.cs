using System.Collections.Generic;
using ModdingTutorial.Common.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.GlobalItems
{
    // Majority of boss/event summon items are no longer consumables
    internal class ReusableSummonItems : GlobalItem
    {
        // Only voodoo dolls, lacewing and truffle worm are used up since they function differently
        private static readonly List<int> noConsumeBoss = new List<int>()
        {
            ItemID.Abeemination,
            ItemID.BloodySpine,
            ItemID.CelestialSigil,
            ItemID.DeerThing,
            ItemID.QueenSlimeCrystal,
            ItemID.LihzahrdPowerCell,
            ItemID.MechanicalEye,
            ItemID.MechanicalSkull,
            ItemID.MechanicalWorm,
            ItemID.SlimeCrown,
            ItemID.SuspiciousLookingEye,
            ItemID.WormFood
        };

        private static readonly List<int> noConsumeEvent = new List<int>()
        {
            ItemID.BloodMoonStarter,
            ItemID.GoblinBattleStandard,
            ItemID.NaughtyPresent,
            ItemID.PirateMap,
            ItemID.PumpkinMoonMedallion,
            ItemID.SnowGlobe,
            ItemID.SolarTablet
        };

        public override void SetDefaults(Item entity)
        {
            // This can be toggled ON/OFF in the configs
            if (ModContent.GetInstance<VanillaChangeToggle>().NoConsumeBossSummon)
            {
                if (noConsumeBoss.Contains(entity.type))
                {
                    entity.consumable = false;
                }
            }

            if(ModContent.GetInstance<VanillaChangeToggle>().NoConsumeEventSummon)
            {
                if (noConsumeEvent.Contains(entity.type))
                {
                    entity.consumable = false;
                }
            }
        }
    }
}
