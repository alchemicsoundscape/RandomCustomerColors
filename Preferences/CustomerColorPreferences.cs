using KitchenLib.Preferences;

namespace RandomCustomerColors.Preferences
{
    public class CustomerColorPreferences
    { 
        public static PreferenceManager PreferenceManager;
        public static PreferenceBool CustomerPreference;
        public static PreferenceBool CatPreference;
        public static PreferenceBool RandomByGroupPreference;
         
        public static void RegisterPreferences(string mod_guid)
        {
            PreferenceManager = new PreferenceManager(mod_guid);

            CustomerPreference = PreferenceManager.RegisterPreference(new PreferenceBool("EnableCustomerRandomColors", defaultValue: true));
            CatPreference = PreferenceManager.RegisterPreference(new PreferenceBool("EnableCatRandomColors", defaultValue: true));
            RandomByGroupPreference = PreferenceManager.RegisterPreference(new PreferenceBool("EnableRandomizeByGroup", defaultValue: false));
            PreferenceManager.Load();
        }
    }
}