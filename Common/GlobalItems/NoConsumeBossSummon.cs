using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.GlobalItems
{
    // Majority of boss summon items are no longer consumables
    // Only voodoo dolls, lacewing and truffle worm are used up since they function differently
    internal class NoConsumeBossSummon : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            if( entity.type == ItemID.Abeemination ||
                entity.type == ItemID.BloodySpine ||
                entity.type == ItemID.BloodySpine ||
                entity.type == ItemID.CelestialSigil ||
                entity.type == ItemID.DeerThing ||
                entity.type == ItemID.QueenSlimeCrystal||
                entity.type == ItemID.LihzahrdPowerCell ||
                entity.type == ItemID.MechanicalEye ||
                entity.type == ItemID.MechanicalSkull ||
                entity.type == ItemID.MechanicalWorm ||
                entity.type == ItemID.SlimeCrown ||
                entity.type == ItemID.SuspiciousLookingEye ||
                entity.type == ItemID.WormFood )
            {
                entity.consumable = false;
            }
        }
    }
}
