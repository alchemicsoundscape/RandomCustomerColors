using KitchenLib;
using System.Reflection;

namespace RandomCustomerColors
{
    public class Main : BaseMod
    { 
        public Main()
            : base("freshpepperino.PlateUp.CustomerColor", "Colorful Customers", "freshpepperino", "1.0.0", ">=1.1.1", Assembly.GetExecutingAssembly()) { }
        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            AddGameDataObject<CustomerColor>();
        }
    }
}
