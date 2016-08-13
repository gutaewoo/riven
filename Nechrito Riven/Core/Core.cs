using LeagueSharp;
using LeagueSharp.Common;

namespace NechritoRiven.Core
{
    internal partial class Core
    {
        public static AttackableUnit qTarget;

        public const string IsFirstR = "RivenFengShuiEngine";
        public const string IsSecondR = "RivenIzunaBlad";

        public static int Qstack = 1;

        public static Orbwalking.Orbwalker _orbwalker;
        public static Obj_AI_Hero Player => ObjectManager.Player;
        public static Obj_AI_Hero Target => TargetSelector.GetTarget(250 + Player.AttackRange + 70, TargetSelector.DamageType.Physical);
    }
}
