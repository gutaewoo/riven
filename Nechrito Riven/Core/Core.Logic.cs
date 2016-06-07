#region

using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace NechritoRiven.Core
{
   internal partial class Core
    {
        public static AttackableUnit QTarget;
        public static bool forceQ;
        public static bool forceW;
        public static bool forceR;
        public static bool forceR2;
        public static bool forceItem;
        public static float lastQ;
        public static float lastR;
        
        private static int Item
            =>
                Items.CanUseItem(3077) && Items.HasItem(3077)
                    ? 3077
                    : Items.CanUseItem(3074) && Items.HasItem(3074) ? 3074 : 0;

        public static void ForceW()
        {
            forceW = Spells.W.IsReady();
            Utility.DelayAction.Add(500, () => forceW = false);
        }

        public static void ForceQ(AttackableUnit target)
        {
            forceQ = true;
            QTarget = target;
        }
        public static void ForceSkill()
        {
            if (forceQ && qTarget != null && qTarget.IsValidTarget(Spells.E.Range + Player.BoundingRadius + 70) &&
                Spells.Q.IsReady())
                Spells.Q.Cast(qTarget.Position);
            if (forceW) Spells.W.Cast();
            if (forceR && Spells.R.Instance.Name == IsFirstR) Spells.R.Cast();
            if (forceItem && Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) Items.UseItem(Item);
            if (forceR2 && Spells.R.Instance.Name == IsSecondR)
            {
            }
        }
        public static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.Contains("ItemTiamatCleave")) forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) forceW = false;
            if (args.SData.Name == IsFirstR) forceR = false;
            if (args.SData.Name == IsSecondR) forceR2 = false;
        }

        public static int WRange => Player.HasBuff("RivenFengShuiEngine")
          ? 330
          : 265;

        public static bool InWRange(Obj_AI_Base t) => t != null && t.IsValidTarget(WRange);

        public static bool InQRange(GameObject target)
        {
            return target != null && (Player.HasBuff("RivenFengShuiEngine")
                ? 330 >= Player.Distance(target.Position)
                : 265 >= Player.Distance(target.Position));
        }
        public static void ForceItem()
        {
            if (Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) forceItem = true;
            Utility.DelayAction.Add(500, () => forceItem = false);
        }

        public static void ForceR()
        {
            forceR = Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR;
            Utility.DelayAction.Add(500, () => forceR = false);
        }

        public static void ForceR2()
        {
            forceR2 = Spells.R.IsReady() && Spells.R.Instance.Name == IsSecondR;
            Utility.DelayAction.Add(500, () => forceR2 = false);
        }
        public static void ForceCastQ(AttackableUnit target)
        {
            forceQ = true;
            qTarget = target;
        }

        public static void FlashW()
        {
            var target = TargetSelector.GetSelectedTarget();
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                Utility.DelayAction.Add(10, () => Player.Spellbook.CastSpell(Spells.Flash, target.Position));
            }
        }
    }
}
