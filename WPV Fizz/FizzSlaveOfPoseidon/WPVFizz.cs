using LeagueSharp;
using LeagueSharp.Common;
using System;
using Color = System.Drawing.Color;

namespace WPVFizz
{
    internal class WPVFizz
    {
        public static Menu Menu;
        public static Orbwalking.Orbwalker Orbwalker;
        public static Spell Q, W, E, E2, R;
        public static bool PacketCast = false;
        public static Spellbook spellBook = ObjectManager.Player.Spellbook;
        public static SpellDataInst eSpell = spellBook.GetSpell(SpellSlot.E);
        public static bool UseEAgain;
        public static Obj_AI_Hero Player;
        public static void OnGameLoad(EventArgs args)
        {
            //Check 
            if (ObjectManager.Player.BaseSkinName != "Fizz") return;

            Q = new Spell(SpellSlot.Q, 560);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 370);
            E2 = new Spell(SpellSlot.E, 370);
            R = new Spell(SpellSlot.R, 1275);           

            E.SetSkillshot(0.5f, 120, 1300, false, SkillshotType.SkillshotCircle);
            E2.SetSkillshot(0.5f, 400, 1300, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.5f, 250f, 1200f, false, SkillshotType.SkillshotLine);
            UseEAgain = true;
            // Setup Main Menu
            SetupMenu();

            Drawing.OnDraw += OnDraw;
            Game.OnGameUpdate += FizzCombo.OnGameUpdate;
            Game.PrintChat("<font color='#00BFFF'>WPV Fizz - by HungWPV - </font>Loaded");
        }

        private static void OnDraw(EventArgs args)
        {
            var drawQ = Menu.Item("drawQ").GetValue<bool>();
            var drawW = Menu.Item("drawW").GetValue<bool>();
            var drawE = Menu.Item("drawE").GetValue<bool>();
            var drawR = Menu.Item("drawR").GetValue<bool>();

            var qColor = Menu.Item("qColor").GetValue<Circle>().Color;
            var wColor = Menu.Item("wColor").GetValue<Circle>().Color;
            var eColor = Menu.Item("eColor").GetValue<Circle>().Color;
            var rColor = Menu.Item("rColor").GetValue<Circle>().Color;

            var position = ObjectManager.Player.Position;

            if (drawQ)
                Utility.DrawCircle(position, Q.Range, qColor);

            if (drawW)
                Utility.DrawCircle(position, W.Range, wColor);

            if (drawE)
                Utility.DrawCircle(position, E.Range, eColor);

            if (drawR)
                Utility.DrawCircle(position, R.Range, rColor);
        }

        private static void SetupMenu()
        {
            Menu = new Menu("[WPV Fizz]", "cmFizz", true);

            // Target Selector
            var tsMenu = new Menu("[WPV Fizz] - Target Selector", "cmFizzTs");
            SimpleTs.AddToMenu(tsMenu);
            Menu.AddSubMenu(tsMenu);

            // Orbwalker
            var orbwalkerMenu = new Menu("[WPV Fizz] - Orbwalker", "cmFizzOrbwalker");
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Menu.AddSubMenu(orbwalkerMenu);

            // Combo settings
            var comboMenu = new Menu("[WPV Fizz] - Combo", "cmFizzCombo");
            comboMenu.AddItem(new MenuItem("useQ", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("useW", "Use W").SetValue(true));
            comboMenu.AddItem(new MenuItem("useE", "Use E").SetValue(true));
            comboMenu.AddItem(new MenuItem("useR", "Use R").SetValue(true));
            comboMenu.AddItem(new MenuItem("forceR", "Force R Cast").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));
            //comboMenu.AddItem(new MenuItem("useIgnite", "Use ignite in combo").SetValue(true));
            Menu.AddSubMenu(comboMenu);

             // Harass Settings
            var harassMenu = new Menu("[WPV Fizz] - Harass", "cmFizzHarass");
            harassMenu.AddItem(new MenuItem("useQHarass", "Use Q").SetValue(false));
            harassMenu.AddItem(new MenuItem("useWHarass", "Use W").SetValue(false));
            harassMenu.AddItem(new MenuItem("useEHarass", "Use E").SetValue(true));
            Menu.AddSubMenu(harassMenu);

            // Items
            var itemsMenu = new Menu("[WPV Fizz] - Items", "cmFizzItems");
            itemsMenu.AddItem(new MenuItem("useDFG", "Use DFG").SetValue(true));
            Menu.AddSubMenu(itemsMenu);

            //Farm
            var farmMenu = new Menu("[WPV Fizz] - Farm", "cmFizzFarm");
            farmMenu.AddItem(new MenuItem("useQFarm", "Q").SetValue(new StringList(new[] { "Freeze", "WaveClear", "Both", "None" }, 1)));
            farmMenu.AddItem(new MenuItem("useWFarm", "W").SetValue(new StringList(new[] { "Freeze", "WaveClear", "Both", "None" }, 3)));
            farmMenu.AddItem(new MenuItem("UseEWC", "Use E WC").SetValue(true));
            farmMenu.AddItem(new MenuItem("JungleActive", "Jungle Clear!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
            farmMenu.AddItem(new MenuItem("UseQJung", "Use Q").SetValue(false));
            farmMenu.AddItem(new MenuItem("UseWJung", "Use W").SetValue(true));
            farmMenu.AddItem(new MenuItem("UseEJung", "Use E").SetValue(true));
            Menu.AddSubMenu(farmMenu);

            // Misc
            var miscMenu = new Menu("[WPV Fizz] - Misc", "cmFizzMisc");
            miscMenu.AddItem(new MenuItem("packetCast", "Use packets for spells").SetValue(true));
            Menu.AddSubMenu(miscMenu);

            //Drawing
            var drawingMenu = new Menu("[WPV Fizz] - Drawing", "cmFizzDrawing");
            drawingMenu.AddItem(new MenuItem("drawQ", "Draw Q").SetValue(true));
            drawingMenu.AddItem(new MenuItem("drawE", "Draw E").SetValue(true));
            drawingMenu.AddItem(new MenuItem("drawR", "Draw R").SetValue(true));
            drawingMenu.AddItem(new MenuItem("qColor", "Q Color").SetValue(new Circle(true, Color.Gray)));
            drawingMenu.AddItem(new MenuItem("eColor", "E Color").SetValue(new Circle(true, Color.Gray)));
            drawingMenu.AddItem(new MenuItem("rColor", "R Color").SetValue(new Circle(true, Color.Gray)));
            Menu.AddSubMenu(drawingMenu);

            Menu.AddToMainMenu();
        }

    }
}
