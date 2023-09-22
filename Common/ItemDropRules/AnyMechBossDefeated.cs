using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace ModdingTutorial.Common.ItemDropRules
{
    // No longer used since 1.4.4 added an easier way to add conditions to npcLoot
    // Left as a example incase needed later
/*    internal class AnyMechBossDefeated : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return NPC.downedMechBossAny;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return "Drops after defeating one of the mechanical bosses";
        }
    }*/
}
