using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.GlobalItems
{
    // This class is used for either buffing some item or nerfing it 
    internal class ItemBuffOrNerf : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            // Make the golden fishing rod a bit better
            if(entity.type == ItemID.GoldenFishingRod)
            {
                entity.fishingPole = 75; // from 50% => 75%
            }
        }
    }
}
