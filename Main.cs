using KitchenLib;
using System.Reflection;

namespace RandomCustomerColors
{
    public class Main : BaseMod
    { 
        public Main()
            : base("freshpepperino.PlateUp.RandomCustomerColors", "Random Customer Colors", "freshpepperino", "1.0.0", ">=1.1.1", Assembly.GetExecutingAssembly()) { }
        protected override void OnPostActivate(KitchenMods.Mod mod)
        {

        }
    }
}
