﻿using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Menus;

namespace NechritoRiven.Event
{
    class Anim : Core.Core
    {
        private static int ExtraDelay => Game.Ping/2;

        private static bool SafeReset =>
                _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Flee &&
                _orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None

        public static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (!sender.IsMe) return;

            switch (args.Animation)
            {
                case "Spell1a":
                    lastQ = Utils.GameTimeTickCount;
                    Qstack = 2;

                    if (SafeReset)
                    {
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + ExtraDelay, Reset);
                    }
                    break;
                case "Spell1b":
                    lastQ = Utils.GameTimeTickCount;
                    Qstack = 3;

                    if (SafeReset)
                    {
                        Utility.DelayAction.Add(MenuConfig.Qd * 10 + ExtraDelay, Reset);
                    }
                    break;
                case "Spell1c":
                    lastQ = Utils.GameTimeTickCount;
                    Qstack = 1;

                    if (SafeReset)
                    {
                        Utility.DelayAction.Add(MenuConfig.Qld * 10 + ExtraDelay, Reset);
                    }
                    break;
            }
        }
        private static void Reset()
        {
            Game.SendEmote(Emote.Dance);
            Orbwalking.LastAATick = 0;
            Player.IssueOrder(GameObjectOrder.MoveTo, Player.Position.Extend(Game.CursorPos, Player.Distance(Game.CursorPos) + 10));
        }
    }
}
