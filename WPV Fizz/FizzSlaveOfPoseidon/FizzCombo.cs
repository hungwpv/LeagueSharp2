using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace WPVFizz
{
   internal class FizzCombo
    {
        public static void OnGameUpdate(EventArgs args)
        {
            WPVFizz.Orbwalker.SetAttack(true);
            var target = SimpleTs.GetTarget(WPVFizz.R.Range, SimpleTs.DamageType.Magical);
            if ((WPVFizz.Menu.Item("forceR").GetValue<KeyBind>().Active) && target != null)
            {
                WPVFizz.R.Cast(target, true);
            }

            if (Orbwalking.OrbwalkingMode.Combo == WPVFizz.Orbwalker.ActiveMode)
            {
                Combo();
            }
            if (Orbwalking.OrbwalkingMode.Mixed == WPVFizz.Orbwalker.ActiveMode)
            {
                Harass();
            }
            if (Orbwalking.OrbwalkingMode.LaneClear == WPVFizz.Orbwalker.ActiveMode)
            {
                WaveClear();
            }
            if (Orbwalking.OrbwalkingMode.LastHit == WPVFizz.Orbwalker.ActiveMode)
            {
                Freeze();
            }
            if (WPVFizz.Menu.Item("JungleActive").GetValue<KeyBind>().Active)
            {
                Jungle();
            }
        }

        private static void Combo()
        {
            var useQ = WPVFizz.Menu.Item("useQ").GetValue<bool>();
            var useW = WPVFizz.Menu.Item("useW").GetValue<bool>();
            var useE = WPVFizz.Menu.Item("useE").GetValue<bool>();
            var useR = WPVFizz.Menu.Item("useR").GetValue<bool>();
            var useDfg = WPVFizz.Menu.Item("useDFG").GetValue<bool>();
            var target = SimpleTs.GetTarget(800, SimpleTs.DamageType.Magical);
            //var UseIgniteCombo = WPVFizz.Menu.Item("useIgnite").GetValue<bool>();
            if (target != null)
            {
                if (target.IsValidTarget(WPVFizz.Q.Range) && useQ && WPVFizz.Q.IsReady())
                {
                    WPVFizz.Q.CastOnUnit(target, WPVFizz.PacketCast);
                    return;
                }
                if (useDfg)
                {
                    if (Items.CanUseItem(3128) && Items.HasItem(3128)) Items.UseItem(3128, target);
                }
                if (target.IsValidTarget(WPVFizz.R.Range) && useR && WPVFizz.R.IsReady())
                {
                   WPVFizz.R.Cast(target, true);
                }
                if (target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(WPVFizz.Player)) && useW && WPVFizz.W.IsReady())
                {
                    WPVFizz.W.Cast(Game.CursorPos, WPVFizz.PacketCast);
                    return;
                }
                if (target.IsValidTarget(800) && useE && WPVFizz.E.IsReady() && WPVFizz.UseEAgain)
                {
                    if (target.IsValidTarget(370 + 250) && WPVFizz.eSpell.Name == "FizzJump")
                    {
                        WPVFizz.E.Cast(target, true);
                        WPVFizz.UseEAgain = false;
                        Utility.DelayAction.Add(250, () => WPVFizz.UseEAgain = true);
                    }
                    if (target.IsValidTarget(370 + 150) && target.IsValidTarget(330) == false && WPVFizz.eSpell.Name == "fizzjumptwo")
                    {
                        WPVFizz.E.Cast(target, true);
                    }
                }

            }
        }

        private static void Harass()
        {
            var useQ = WPVFizz.Menu.Item("useQ").GetValue<bool>();
            var useW = WPVFizz.Menu.Item("useW").GetValue<bool>();
            var useE = WPVFizz.Menu.Item("useE").GetValue<bool>();
            var target = SimpleTs.GetTarget(800, SimpleTs.DamageType.Magical);

            if (target != null)
            {
                if (target.IsValidTarget(WPVFizz.Q.Range) && useQ && WPVFizz.Q.IsReady())
                {
                    WPVFizz.Q.CastOnUnit(target, WPVFizz.PacketCast);
                    return;
                }
                if (target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(WPVFizz.Player)) && useW && WPVFizz.W.IsReady())
                {
                    WPVFizz.W.Cast(Game.CursorPos, WPVFizz.PacketCast);
                    return;
                }
                if (target.IsValidTarget(800) && useE && WPVFizz.E.IsReady() && WPVFizz.UseEAgain)
                {
                    if (target.IsValidTarget(370 + 330) && WPVFizz.eSpell.Name == "FizzJump")
                    {
                        WPVFizz.E.Cast(target, true);
                        WPVFizz.UseEAgain = false;
                        Utility.DelayAction.Add(250, () => WPVFizz.UseEAgain = true);
                    }
                    if (target.IsValidTarget(370 + 270) && target.IsValidTarget(330) == false && WPVFizz.eSpell.Name == "fizzjumptwo")
                    {
                        WPVFizz.E.Cast(target, true);
                    }
                }
            }
        }

        private static void WaveClear()
        {
            var useQ = WPVFizz.Menu.Item("useQFarm").GetValue<StringList>().SelectedIndex == 1 || WPVFizz.Menu.Item("useQFarm").GetValue<StringList>().SelectedIndex == 2;
            var useW = WPVFizz.Menu.Item("useWFarm").GetValue<StringList>().SelectedIndex == 1 || WPVFizz.Menu.Item("useWFarm").GetValue<StringList>().SelectedIndex == 2;
            var useE = WPVFizz.Menu.Item("UseEWC").GetValue<bool>();
            var jungleMinions = MinionManager.GetMinions(ObjectManager.Player.Position, WPVFizz.E.Range);

            if (jungleMinions.Count > 0)
            {
                foreach (var minion in jungleMinions)
                {
                    if (minion.IsValidTarget(WPVFizz.E.Range) && WPVFizz.E.IsReady() && useE && WPVFizz.eSpell.Name == "FizzJump")
                    {
                        var ePoint = WPVFizz.E.GetCircularFarmLocation(jungleMinions);
                        WPVFizz.E.Cast(ePoint.Position, true);
                    }
                    if (minion.IsValidTarget(WPVFizz.Q.Range) && useQ && WPVFizz.Q.IsReady() && WPVFizz.Q.GetDamage(minion) > minion.Health)
                    {
                        WPVFizz.Q.CastOnUnit(minion, WPVFizz.PacketCast);
                    }
                    if (minion.IsValidTarget(Orbwalking.GetRealAutoAttackRange(WPVFizz.Player)) && useW && WPVFizz.W.IsReady())
                    {
                        WPVFizz.W.Cast(Game.CursorPos, WPVFizz.PacketCast);
                    }
                }

            }
        }

        private static void Freeze()
        {
            var useQ = WPVFizz.Menu.Item("useQFarm").GetValue<StringList>().SelectedIndex == 0 || WPVFizz.Menu.Item("useQFarm").GetValue<StringList>().SelectedIndex == 2;
            var useW = WPVFizz.Menu.Item("useWFarm").GetValue<StringList>().SelectedIndex == 0 || WPVFizz.Menu.Item("useWFarm").GetValue<StringList>().SelectedIndex == 2;
            var jungleMinions = MinionManager.GetMinions(ObjectManager.Player.Position, WPVFizz.E.Range);

            if (jungleMinions.Count > 0)
            {
                foreach (var minion in jungleMinions)
                {
                    if (minion.IsValidTarget(WPVFizz.Q.Range) && useQ && WPVFizz.Q.IsReady() && WPVFizz.Q.GetDamage(minion) > minion.Health)
                    {
                        WPVFizz.Q.CastOnUnit(minion, WPVFizz.PacketCast);
                    }
                    if (minion.IsValidTarget(Orbwalking.GetRealAutoAttackRange(WPVFizz.Player)) && useW && WPVFizz.W.IsReady())
                    {
                        WPVFizz.W.Cast(Game.CursorPos, WPVFizz.PacketCast);
                    }
                }
            }
        }

        private static void Jungle()
        {
            var useQ = WPVFizz.Menu.Item("UseQJung").GetValue<bool>();
            var useW = WPVFizz.Menu.Item("UseWJung").GetValue<bool>();
            var useE = WPVFizz.Menu.Item("UseEJung").GetValue<bool>();
            var jungleMinions = MinionManager.GetMinions(ObjectManager.Player.Position, WPVFizz.E.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (jungleMinions.Count > 0)
            {
                foreach (var minion in jungleMinions)
                {
                    if (minion.IsValidTarget(WPVFizz.E.Range) && WPVFizz.E.IsReady() && WPVFizz.eSpell.Name == "FizzJump" && useE)
                    {
                        WPVFizz.E.Cast(minion);
                    }
                    if (minion.IsValidTarget(WPVFizz.Q.Range) && useQ && WPVFizz.Q.IsReady())
                    {
                        WPVFizz.Q.CastOnUnit(minion, WPVFizz.PacketCast);
                    }
                    if (minion.IsValidTarget(Orbwalking.GetRealAutoAttackRange(WPVFizz.Player)) && useW && WPVFizz.W.IsReady())
                    {
                        WPVFizz.W.Cast(Game.CursorPos, WPVFizz.PacketCast);
                    }
                }
            }
        }

       
    }
}
