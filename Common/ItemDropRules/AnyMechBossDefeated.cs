using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace ModdingTutorial.Common.ItemDropRules
{
    internal class AnyMechBossDefeated : IItemDropRuleCondition
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
    }
}
