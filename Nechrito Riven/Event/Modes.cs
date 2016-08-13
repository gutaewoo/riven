#region

using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using NechritoRiven.Core;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Event
{
    internal class Modes : Core.Core
    {
        // Laneclear
        public static void OnDoCastLc(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
        }

        // Jungle, Combo etc.
        public static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spellName = args.SData.Name;
            if (!sender.IsMe || !Orbwalking.IsAutoAttack(spellName)) return;
            qTarget = (Obj_AI_Base)args.Target;

            if (args.Target is Obj_AI_Minion)
            {
                Jungleclear();
                if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var minions = MinionManager.GetMinions(600f).FirstOrDefault();
                    if (minions == null)
                        return;

                    if (Spells.E.IsReady() && MenuConfig.LaneE)
                        Spells.E.Cast(minions.ServerPosition);

                    if (Spells.Q.IsReady() && MenuConfig.LaneQ)
                    {
                        Utility.DelayAction.Add(1, () => ForceCastQ(minions));
                        Usables.CastHydra();
                    }

                    if (Spells.W.IsReady() && MenuConfig.LaneW)
                    {
                        var minion = MinionManager.GetMinions(Player.Position, Spells.W.Range);
                        foreach (var m in minion)
                        {
                            if (m.Health < Spells.W.GetDamage(m) && minion.Count > 2)
                                Utility.DelayAction.Add(70, () => Spells.W.Cast(m));
                        }
                    }
                }
                
            }

            var @base = args.Target as Obj_AI_Turret;
            if (@base != null)

                if (@base.IsValid && args.Target != null && Spells.Q.IsReady() && MenuConfig.LaneQ &&
                    _orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear) ForceCastQ(@base);

            var hero = args.Target as Obj_AI_Hero;
            if (hero == null) return;
            var target = hero;

            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {

                if (Spells.E.IsReady())
                {
                    Spells.E.Cast(target.Position);
                    Usables.CastHydra();
                }

                if (Spells.W.IsReady() && InWRange(target))
                {
                    Usables.CastHydra();
                    Spells.W.Cast();
                }

                if (Spells.Q.IsReady())
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }

                if (Spells.R.IsReady() && Qstack == 2 && Spells.R.Instance.Name == IsSecondR)
                    Spells.R.Cast(target.Position);
            }
            
            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.FastHarass)
            {
                if (Spells.W.IsReady() && InWRange(target))
                {
                    Usables.CastHydra();
                    Spells.W.Cast();
                }
                else if (Spells.Q.IsReady())
                {
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }
            }

            if (_orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
              {

                if (Qstack == 2 && Spells.Q.IsReady())
                {
                    Usables.CastHydra();
                    ForceItem();
                    Utility.DelayAction.Add(1, () => ForceCastQ(target));
                }
              }
        }

        public static void QMove()
        {

            if (!MenuConfig.QMove)
            {
                return;
            }

            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (Spells.Q.IsReady())
            {
                Utility.DelayAction.Add(47, () => Spells.Q.Cast(Game.CursorPos));
            }
           

        }

        public static void Jungleclear()
        {
            if (_orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear) return;

            var mobs = MinionManager.GetMinions(Player.Position, 600f, MinionTypes.All,
                MinionTeam.Neutral, MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (mobs == null)
                return;

            // JUNGLE
            if (Spells.E.IsReady() && MenuConfig.jnglE)
            {
                Spells.E.Cast(mobs);
                Usables.CastHydra();
            }

            if (Spells.Q.IsReady() && MenuConfig.jnglQ)
            {
                ForceItem();
                Utility.DelayAction.Add(1, () => ForceCastQ(mobs));
            }
            if (Spells.W.IsReady() && MenuConfig.jnglW)
            {
                ForceItem();
                Spells.W.Cast(mobs);
            }
        }
        
        public static void Laneclear()
        {
            var minions = MinionManager.GetMinions(Player.AttackRange + 380);

            if (minions == null)
            {
                return;
            }

            foreach (var m in minions)
            {
                if (m.UnderTurret()) continue;

                if (Spells.E.IsReady() && MenuConfig.LaneE)
                {
                    Spells.E.Cast(m);
                }

                if(!Spells.W.IsReady() || !MenuConfig.LaneW || !InWRange(m) || Player.IsWindingUp || m.Health > Spells.W.GetDamage(m)) continue;

                Spells.W.Cast(m);
            }
        }
        public static void Combo()
        {
            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR && MenuConfig.AlwaysR &&
                Target != null) ForceR();

            if (Spells.W.IsReady() && InWRange(Target) && Target != null) Spells.W.Cast();

            if (Spells.R.IsReady() && Spells.R.Instance.Name == IsFirstR && Spells.W.IsReady() && Target != null &&
                Spells.E.IsReady() && Target.IsValidTarget() && !Target.IsZombie && (Dmg.IsKillableR(Target) || MenuConfig.AlwaysR))
            {
                if (!InWRange(Target))
                {
                    Spells.E.Cast(Target.Position);
                    ForceR();
                    Utility.DelayAction.Add(200, ForceW);
                    Utility.DelayAction.Add(30, () => ForceCastQ(Target));
                }
            }

            else if (Spells.W.IsReady() && Spells.E.IsReady())
            {
                if (Target.IsValidTarget() && Target != null && !Target.IsZombie && !InWRange(Target))
                {
                    Spells.E.Cast(Target.Position);
                    if (InWRange(Target))
                    Utility.DelayAction.Add(100, ForceW);
                    Utility.DelayAction.Add(30, () => ForceCastQ(Target));
                }
            }
            else if (Spells.E.IsReady())
            {
                if (Target != null && Target.IsValidTarget() && !Target.IsZombie && !InWRange(Target))
                {
                    Spells.E.Cast(Target.Position);
                }
            }
        }



        public static void FastHarass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (Spells.Q.IsReady() && Qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    Utility.DelayAction.Add(1, ForceW);
                }
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(400, TargetSelector.DamageType.Physical);
            if (Spells.Q.IsReady() && Spells.W.IsReady() && Spells.E.IsReady() && Qstack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    Utility.DelayAction.Add(1, ForceW);
                }
            }
            if (Spells.Q.IsReady() && Spells.E.IsReady() && Qstack == 3 && !Orbwalking.CanAttack() && Orbwalking.CanMove(5))
            {
                var epos = Player.ServerPosition +
                          (Player.ServerPosition - target.ServerPosition).Normalized() * 300;
                Spells.E.Cast(epos);
                Utility.DelayAction.Add(190, () => Spells.Q.Cast(epos));
            }
        }

        public static void Flee()
        {
            if (MenuConfig.WallFlee)
            {
                var end = Player.ServerPosition.Extend(Game.CursorPos, Spells.Q.Range);
                var IsWallDash = FleeLogic.IsWallDash(end, Spells.Q.Range);

                var Eend = Player.ServerPosition.Extend(Game.CursorPos, Spells.E.Range);
                var WallE = FleeLogic.GetFirstWallPoint(Player.ServerPosition, Eend);
                var WallPoint = FleeLogic.GetFirstWallPoint(Player.ServerPosition, end);
                Player.GetPath(WallPoint);

                if (Spells.Q.IsReady() && Qstack < 3)
                { Spells.Q.Cast(Game.CursorPos); }


                if (IsWallDash && Qstack == 3 && WallPoint.Distance(Player.ServerPosition) <= 800)
                {
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                    if (WallPoint.Distance(Player.ServerPosition) <= 600)
                    {
                        ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                        if (WallPoint.Distance(Player.ServerPosition) <= 45)
                        {
                            if (Spells.E.IsReady())
                            {
                                Spells.E.Cast(WallE);
                            }
                            if (Qstack == 3 && end.Distance(Player.Position) <= 260 && IsWallDash && WallPoint.IsValid())
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, WallPoint);
                                Spells.Q.Cast(WallPoint);
                            }

                        }
                    }
                }
            }
            else
            {
                var enemy = HeroManager.Enemies.Where(hero => hero.IsValidTarget(Player.HasBuff("RivenFengShuiEngine")
                           ? 70 + 195 + Player.BoundingRadius
                           : 70 + 120 + Player.BoundingRadius) && Spells.W.IsReady());

                var x = Player.Position.Extend(Game.CursorPos, 300);
                var objAiHeroes = enemy as Obj_AI_Hero[] ?? enemy.ToArray();
                if (Spells.W.IsReady() && objAiHeroes.Any()) foreach (var target in objAiHeroes) if (InWRange(target)) Spells.W.Cast();
                if (Spells.Q.IsReady() && !Player.IsDashing()) Spells.Q.Cast(Game.CursorPos);
                if (Spells.E.IsReady() && !Player.IsDashing()) Spells.E.Cast(x);
            }
        }
    }
}
