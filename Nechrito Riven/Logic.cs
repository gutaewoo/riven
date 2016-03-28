﻿using LeagueSharp;
using LeagueSharp.Common;
using ItemData = LeagueSharp.Common.Data.ItemData;

namespace NechritoRiven
{
    class Logic
    {
        public const string IsFirstR = "RivenFengShuiEngine";
        public const string IsSecondR = "RivenIzunaBlade";
        public static bool _forceQ;
        public static bool _forceW;
        public static bool _forceR;
        public static bool _forceR2;
        public static bool _forceItem;
        public static float _lastQ;
        public static float _lastR;
        public static AttackableUnit _qtarget;
        public static int WRange => Program.Player.HasBuff("RivenFengShuiEngine")
            ? 330
            : 265;

        public static bool InWRange(AttackableUnit t) => t != null && t.IsValidTarget(WRange);


        public static bool InQRange(GameObject target)
        {
            return target != null && (Program.Player.HasBuff("RivenFengShuiEngine")
                ? 330 >= Program.Player.Distance(target.Position)
                : 265 >= Program.Player.Distance(target.Position));
        }


        public static void ForceSkill()
        {
            if (_forceQ && _qtarget != null && _qtarget.IsValidTarget(Spells._e.Range + Program.Player.BoundingRadius + 70) &&
                Spells._q.IsReady())
                Spells._q.Cast(_qtarget.Position);
            if (_forceW) Spells._w.Cast();
            if (_forceR && Spells._r.Instance.Name == IsFirstR) Spells._r.Cast();
            if (_forceItem && Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) Items.UseItem(Item);
            if (_forceR2 && Spells._r.Instance.Name == IsSecondR)
            {
                var target = TargetSelector.GetSelectedTarget();
                if (target != null) Spells._r.Cast(target.Position);
            }
        }

        public static void ForceItem()
        {
            if (Items.CanUseItem(Item) && Items.HasItem(Item) && Item != 0) _forceItem = true;
            Utility.DelayAction.Add(500, () => _forceItem = false);
        }

        public static void ForceR()
        {
            _forceR = Spells._r.IsReady() && Spells._r.Instance.Name == IsFirstR;
            Utility.DelayAction.Add(500, () => _forceR = false);
        }

        public static void ForceR2()
        {
            _forceR2 = Spells._r.IsReady() && Spells._r.Instance.Name == IsSecondR;
            Utility.DelayAction.Add(500, () => _forceR2 = false);
        }

        public static void ForceW()
        {
            _forceW = Spells._w.IsReady();
            Utility.DelayAction.Add(500, () => _forceW = false);
        }

        public static void ForceCastQ(AttackableUnit target)
        {
            _forceQ = true;
            _qtarget = target;
        }



        public static bool HasItem()
            => ItemData.Tiamat_Melee_Only.GetItem().IsReady() || ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady();

        public static void CastYoumoo()
        {
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady()) ItemData.Youmuus_Ghostblade.GetItem().Cast();
        }

        public static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.Contains("ItemTiamatCleave")) _forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) _forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) _forceW = false;
            if (args.SData.Name == Program.IsFirstR) _forceR = false;
            if (args.SData.Name == Program.IsSecondR) _forceR2 = false;
        }

        public static int Item
            =>
                Items.CanUseItem(3077) && Items.HasItem(3077)
                    ? 3077
                    : Items.CanUseItem(3074) && Items.HasItem(3074) ? 3074 : 0;
    }
}