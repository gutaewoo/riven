﻿using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SPrediction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nechrito_Diana
{
    class Modes
    {
        public static void ComboLogic()
        {
            var t = TargetSelector.GetTarget(Spells._q.Range, TargetSelector.DamageType.Magical);
            var test = Spells._q.GetArcSPrediction(t).CastPosition;
            var target = TargetSelector.GetTarget(900, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (target.Health < Dmg.ComboDmg(target) || MenuConfig.Misaya)
                {
                    if ((Program.Player.Distance(target.Position) <= 800f) && (Program.Player.Distance(target.Position) >= 680f))
                    {
                        if (t != null)
                        {
                            if (Spells._r.IsReady())
                            {
                                Spells._r.Cast(t);
                            }
                           else if (Spells._q.IsReady())
                            {
                                var pos = Spells._q.GetSPrediction(target).CastPosition;
                               Utility.DelayAction.Add(15, ()=> Spells._q.Cast(test));
                            }
                            
                        }
                    }
                }
                if (Spells._q.IsReady() && Spells._q.GetPrediction(target).Hitchance >= HitChance.High && (Program.Player.Distance(target.Position) <= 825f) && !MenuConfig.Misaya)
                {
                    if (t != null)
                    {
                        var pos = Spells._q.GetSPrediction(t).CastPosition;
                        Spells._q.Cast(test);
                    }
                }
                if (Spells._r.IsReady() && (Program.Player.Distance(target.Position) <= 800f))
                {
                    if (t != null && t.HasBuff("dianamoonlight") && MenuConfig.ComboR)
                    {
                        Spells._r.Cast(t);
                    }
                    else if (!MenuConfig.ComboR && t != null)
                    { Utility.DelayAction.Add(60, () => Spells._r.Cast(t)); }
                }
                if (Spells._w.IsReady() && (Program.Player.Distance(target.Position) <= Program.Player.AttackRange + 30))
                    Spells._w.Cast(target);
                if (MenuConfig.ComboE && Program.Player.ManaPercent > 25)
                {
                    if (Spells._e.IsReady() && (Program.Player.Distance(target.Position) <= Spells._e.Range - 45 || target.CountEnemiesInRange(Spells._e.Range) > 1 || target.IsDashing() || !target.IsFacing(Program.Player)))
                        Spells._e.Cast(target);
                }
            }
        }
        public static void HarassLogic()
        {
            var target = TargetSelector.GetTarget(800, TargetSelector.DamageType.Magical);
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                if (Spells._q.IsReady() && (Program.Player.Distance(target.Position) <= 700f))
                {
                    var t = TargetSelector.GetTarget(Spells._q.Range, TargetSelector.DamageType.Magical);
                    if (t != null)
                    {
                        Spells._q.SPredictionCast(target, HitChance.High);
                    }
                }
                if (Spells._w.IsReady() && (Program.Player.Distance(target.Position) <= Program.Player.AttackRange))
                    Spells._w.Cast(target);
            }
        }
        public static void JungleLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var mobs = MinionManager.GetMinions(800 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
           MinionOrderTypes.MaxHealth);
                if (mobs.Count == 0)
                    return;

                if (Spells._q.IsReady() && Spells._r.IsReady())
                {
                    var m = MinionManager.GetMinions(800 + Program.Player.AttackRange, MinionTypes.All, MinionTeam.Neutral,
          MinionOrderTypes.MaxHealth);
                    if (m != null && MenuConfig.jnglQR && (Program.Player.Distance(m[0].Position) <= 700f) && (Program.Player.Distance(m[0].Position) >= 400f) && Program.Player.ManaPercent > 20)
                    {
                        Spells._q.Cast(m[0]);
                        Spells._r.Cast(m[0]);
                    }
                }
                if (Spells._w.IsReady() && (Program.Player.Distance(mobs[0].Position) <= 300f) && MenuConfig.jnglW)
                    Spells._w.Cast(mobs[0].ServerPosition);

                if (Spells._e.IsReady())
                {
                    var minion = MinionManager.GetMinions(Program.Player.Position, Spells._e.Range);
                    foreach (var m in mobs)
                    {
                        if (m.IsAttackingPlayer && Program.Player.HealthPercent <= MenuConfig.jnglE.Value)
                            Spells._e.Cast(m);
                    }
                }
            }
        }
        public static void LaneLogic()
        {
            if (MenuConfig._orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var minions = MinionManager.GetMinions(800f).FirstOrDefault();
                if (minions == null)
                    return;

                if (Spells._w.IsReady() && MenuConfig.LaneW && (Program.Player.Distance(minions.Position) <= 250f))
                {
                    Spells._w.Cast();
                }

                if (Spells._q.IsReady() && MenuConfig.LaneQ && Program.Player.ManaPercent >= 45 && (Program.Player.Distance(minions.Position) <= 550f))
                {
                    var minion = MinionManager.GetMinions(Program.Player.Position, Spells._w.Range);
                    foreach (var m in minion)
                    {
                        if (m.Health < Spells._q.GetDamage(m) && minion.Count > 2 && !MenuConfig._orbwalker.InAutoAttackRange(m))
                            Spells._q.Cast(m);
                    }
                }
            }
        }
        public static void Game_OnUpdate(EventArgs args)
        {
            if (MenuConfig.UseSkin)
            {
                Program.Player.SetSkin(Program.Player.CharData.BaseSkinName, MenuConfig.Config.Item("Skin").GetValue<StringList>().SelectedIndex);
            }
            else Program.Player.SetSkin(Program.Player.CharData.BaseSkinName, Program.Player.BaseSkinId);
        }
        public static void Flee()
        {
            if (!MenuConfig.FleeMouse)
            {
                return;
            }
            
            var jump = Program.JumpPos.Where(x => x.Value.Distance(Program.Player.Position) < 300f && x.Value.Distance(Game.CursorPos) < 700f).FirstOrDefault();
            var monster = MinionManager.GetMinions(900f, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.Health).FirstOrDefault();
            var mobs = MinionManager.GetMinions(900, MinionTypes.All, MinionTeam.NotAlly);
            
            if (jump.Value.IsValid())
            {
                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, jump.Value);

                foreach (var junglepos in Program.JunglePos)
                {
                    if (Game.CursorPos.Distance(junglepos) <= 350 && ObjectManager.Player.Position.Distance(junglepos) <= 850 && Spells._q.IsReady() && Spells._r.IsReady())
                    {
                        Spells._q.Cast(junglepos);
                        Spells._r.Cast(monster);
                    }
                }
            }
            else
            {
                ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }

            foreach (var junglepos in Program.JunglePos)
            {
                if (Game.CursorPos.Distance(junglepos) <= 350 && ObjectManager.Player.Position.Distance(junglepos) <= 900 && Spells._q.IsReady() && Spells._r.IsReady())
                {
                    Spells._q.Cast(junglepos);
                    Spells._r.Cast(monster);
                }
                else if (Spells._r.IsReady() && !Spells._q.IsReady() && monster.Distance(Program.Player.Position) > 600f && monster.Distance(Game.CursorPos) <= 350f)
                {
                    Spells._r.Cast(monster);
                }
            }
            if (!mobs.Any()) { return; }

            var mob = mobs.MaxOrDefault(x => x.MaxHealth);

            if (mob.Distance(Game.CursorPos) <= 750 && mob.Distance(Program.Player) >= 475)
            {
                if (Spells._q.IsReady() && Spells._r.IsReady() && Program.Player.Mana > Spells._r.ManaCost + Spells._q.ManaCost && mob.Health > Spells._q.GetDamage(mob))
                {
                    Spells._q.Cast(mob);
                    Spells._r.Cast(mob);
                }
                if (Spells._r.IsReady())
                {
                    Spells._r.Cast(mob);
                }
            }
        }
    }
}