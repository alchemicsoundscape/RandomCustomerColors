using KitchenMods; 
using Unity.Entities; 

namespace KitchenRandomCustomerColor.Components
{
    public struct CCustomerColor : IComponentData, IModComponent 
    {
        public bool hasChangedColor;
    }
}