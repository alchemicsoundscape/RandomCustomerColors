using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenRandomCustomerColor
{
    public class SettingsMenu<T> : KLMenu<T>
    {
        public SettingsMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        public override void Setup(int player_id)
        {
            AddLabel("Randomize customer colors");
            Add(new Option<bool>(new List<bool> { true, false }, Mod.CustomerPreference.Get(), new List<string> { "On", "Off" })).OnChanged += delegate (object _, bool newVal)
            {
                Mod.CustomerPreference.Set(newVal);
            };
            AddInfo("Allows colors to be randomized for customers.");

            AddLabel("Randomize cat colors");
            Add(new Option<bool>(new List<bool> { true, false }, Mod.CatPreference.Get(), new List<string> { "On", "Off" })).OnChanged += delegate (object _, bool newVal)
            {
                Mod.CatPreference.Set(newVal);
            };
            AddInfo("Allows colors to be randomized for cats.");

            AddLabel("Randomize color by group");
            Add(new Option<bool>(new List<bool> { true, false }, Mod.RandomByGroupPreference.Get(), new List<string> { "On", "Off" })).OnChanged += delegate (object _, bool newVal)
            {
                Mod.RandomByGroupPreference.Set(newVal);
            }; 
            AddInfo("Randomizes groups to all have the same color.");

            AddInfo("Setting changes will not apply to customers and cats that already have colors.");
            AddButton("Apply", delegate
            {
                Mod.PreferenceManager.Save();
                RequestPreviousMenu();
            });

            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate
            {
                RequestPreviousMenu();
            });
        }
    }
}
