using KitchenMods; 
using Unity.Entities; 

namespace RandomCustomerColors.Components
{
    public struct CCustomerColor : IComponentData, IModComponent 
    {
        public bool hasChangedColor;
    }
}