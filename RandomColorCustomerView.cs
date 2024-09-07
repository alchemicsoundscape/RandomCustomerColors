using Kitchen; 
using KitchenLib.Utils;
using KitchenMods;
using RandomCustomerColors.Components;
using MessagePack; 
using Unity.Collections;
using System.Collections.Generic;
using Unity.Entities; 
using UnityEngine;
using System.Linq;
using System;
using RandomCustomerColors.Preferences;

namespace RandomCustomerColors.Views
{
    [UpdateAfter(typeof(CustomerCreationGroup))]
    public class RandomColorCustomerView : UpdatableObjectView<RandomColorCustomerView.CustomerColorViewData>
    {
        public class CustomerColor : IncrementalViewSystemBase<CustomerColorViewData>, IModSystem
        { 
            private EntityQuery _newCustomerQuery;
            private EntityQuery _colorizedCustomersQuery;

            protected override void Initialise()
            { 
                base.Initialise();

                try
                {
                    _newCustomerQuery = GetEntityQuery(new QueryHelper()
                        .All(typeof(CCustomer), typeof(CPosition), typeof(CLinkedView), typeof(CRequiresView), typeof(CBelongsToGroup))
                        .None(typeof(CCustomerColor))
                    );

                    _colorizedCustomersQuery = GetEntityQuery(new QueryHelper().All(typeof(CCustomerColor), typeof(CLinkedView), typeof(CRequiresView))); 
                }
                catch (Exception e)
                {
                    Mod.LogError(e.Message);
                }
            }

            protected override void OnUpdate()
            {
                if (_newCustomerQuery.IsEmpty)
                    return;
                  
                var currenRandomizedCustomers = GetEntityQuery(new QueryHelper().All(typeof(CCustomerColor)));
                NativeArray<CLinkedView> linkedViews = _newCustomerQuery.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                NativeArray<Entity> customers = _newCustomerQuery.ToEntityArray(Allocator.Temp);
                NativeArray<CRequiresView> customerType = _newCustomerQuery.ToComponentDataArray<CRequiresView>(Allocator.Temp);
                NativeArray<CBelongsToGroup> group = _newCustomerQuery.ToComponentDataArray<CBelongsToGroup>(Allocator.Temp);
                 
                for (int i = 0; i < linkedViews.Length; i++)
                {  
                    if(!((customerType[i].Type.Equals(ViewType.Customer) && CustomerColorPreferences.CustomerPreference.Get()) || (customerType[i].Type.Equals(ViewType.CustomerCat) && CustomerColorPreferences.CatPreference.Get())))
                    {
                        linkedViews.Dispose();
                        customers.Dispose();
                        customerType.Dispose();
                        group.Dispose();
                         
                        EntityManager.AddComponent<CCustomerColor>(customers[i]); 
                        return;
                    }
                      
                    if (CustomerColorPreferences.RandomByGroupPreference.Get())
                    {  
                       foreach(var member in customers)
                        { 
                            EntityManager.AddComponent(member, typeof(CCustomerColor)); 
                        }  
                    }
                    else
                    {
                        Color c = UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0.5f, 1f, 1f, 1f); 
                        SendUpdate(linkedViews[i], new CustomerColorViewData
                        {
                            r = c.r,
                            g = c.g,
                            b = c.b
                        }, MessageType.SpecificViewUpdate); 
                        EntityManager.AddComponent<CCustomerColor>(customers[i]);

                        linkedViews.Dispose();
                        customers.Dispose();
                        customerType.Dispose();
                        group.Dispose(); 
                        return;
                    } 
                }
                 
                NativeArray<CCustomerColor> customerColors = _colorizedCustomersQuery.ToComponentDataArray<CCustomerColor>(Allocator.Temp); 

                NativeArray<Entity> customerColorsEntity = _colorizedCustomersQuery.ToEntityArray(Allocator.Temp); 

                NativeArray<CLinkedView> customerColorLinkedViews = _colorizedCustomersQuery.ToComponentDataArray<CLinkedView>(Allocator.Temp); 

                List<ColorCoordinator> groupedCustomers = new List<ColorCoordinator>();
                 
                for (int i = 0; i < customerColorsEntity.Length; i++)
                {
                    var ctx = new EntityContext(EntityManager);
                    var groupId = ctx.Get<CBelongsToGroup>(customerColorsEntity[i]).Group.Index; 

                    if (groupedCustomers.Any(gc => gc.groupId.Equals(groupId)))
                    { 
                        var thisGc = groupedCustomers.First(gc => gc.groupId.Equals(groupId)); 
                        var colorCoord = new ColorCoordinator()
                        {
                            entity = customerColorsEntity[i],
                            groupId = groupId,
                            color = thisGc.color
                        }; 
                        groupedCustomers.Add(colorCoord); 
                    }
                    else
                    { 
                        groupedCustomers.Add(new ColorCoordinator()
                        {
                            entity = customerColorsEntity[i],
                            groupId = groupId,
                            color = UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0.5f, 1f, 1f, 1f)
                        });
                    }
                } 

                foreach(var customerGroup in groupedCustomers)
                { 
                    var comp = GetComponent<CCustomerColor>(customerGroup.entity); 
                    if (!comp.hasChangedColor)
                    {
                        Mod.LogInfo("Changing entity color - " + customerGroup.ToString());
                        SendUpdate(customerColorLinkedViews[groupedCustomers.IndexOf(customerGroup)], new CustomerColorViewData()
                        {
                            r = customerGroup.color.r,
                            g = customerGroup.color.g,
                            b = customerGroup.color.b,
                        }, MessageType.SpecificViewUpdate);
                        comp.hasChangedColor = true;
                        Set(customerGroup.entity, comp);
                        Mod.LogInfo("Done Changing entity color - " + customerGroup.ToString());

                    }
                } 
                customerColors.Dispose();
                customerColorsEntity.Dispose();
                customerColorLinkedViews.Dispose();
            }
        }

        public struct ColorCoordinator
        {
            public Entity entity;
            public Color color;
            public int groupId;
        }

        [MessagePackObject]
        public struct CustomerColorViewData : ISpecificViewData, IViewData.ICheckForChanges<CustomerColorViewData>
        {
            [Key(0)] public float r;
            [Key(1)] public float g;
            [Key(2)] public float b;  
            public IUpdatableObject GetRelevantSubview(IObjectView view)
            { 
                return view.GameObject.HasComponent<RandomColorCustomerView>() ? view.GetSubView<RandomColorCustomerView>() : view.GameObject.AddComponent<RandomColorCustomerView>();
            }

            public bool IsChangedFrom(CustomerColorViewData check)
            { 
                return r != check.r || b != check.b || g != check.g;
            }
        }

        protected override void UpdateData(CustomerColorViewData data)
        { 
            Material material = new Material(MaterialUtils.GetExistingMaterial("Customer"));
            material.SetColor("_Color0", new Color(data.r, data.g, data.b));
            GetComponentInChildren<SkinnedMeshRenderer>().materials = new Material[1] { material };
        } 
    }
} 