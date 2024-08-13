using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI.ItemViewList
{
    /// <summary>
    /// UI module that enables users to freely add, remove, and manage items based on Unity's ScrollView
    /// </summary>
    public abstract class ItemViewList : EdgyModulePrototype
    {
        /// <summary>
        /// Interface that defines methods for managing items in the UI module
        /// </summary>
        public interface IItemStateUpdate
        {
            /// <summary>
            /// Method for initializing items in the UI module
            /// </summary>
            /// <param name="itemViewList">Instance of ItemViewList where the current item is placed</param>
            /// <param name="itemInitData">Parameters place in custom data that can be used to initialize the current item. Null is also possible if desired</param>
            void InitializeItem(in ItemViewList itemViewList, in object itemInitData);

            /// <summary>
            /// A method that can be invoked when updating the status of the current item
            /// </summary>
            /// <param name="itemUpdateData">A parameter spot for custom data that can be used to update the status of the current item. Null is also possible if desired</param>
            void UpdateItemState(in object itemUpdateData);
        }

        /// <summary>
        /// Classes describing items for ItemViewList
        /// </summary>
        public abstract class Item : MonoBehaviour, IItemStateUpdate, IItemSelectedAction
        {
            private ItemViewList m_itemViewList;


            /// <summary>
            /// A reference to the ItemViewList instance where the current item is placed.
            /// </summary>
            public ItemViewList AttahcedItemViewList
            {
                get
                {
                    return m_itemViewList;
                }
            }

            public virtual void InitializeItem(in ItemViewList itemViewList, in object itemInitData)
            {
                m_itemViewList = itemViewList;

                UpdateItemState(itemInitData);
            }

            public virtual void UpdateItemState(in object itemUpdateData)
            {
                if(m_itemViewList != null)
                {
                    bool bisItemSelected = m_itemViewList.CurSelectedItemIndex == m_itemViewList.GetItemIndex(this);

                    OnItemSelected(itemUpdateData, bisItemSelected);
                }
            }

            /// <summary>
            /// Method called when you want to remove the current item from ItemViewList
            /// </summary>
            public virtual void RemoveItem()
            {
                m_itemViewList.RemoveItem(this);
            }
            /// <summary>
            /// Method called when you want to select the current item in ItemViewList
            /// </summary>
            public virtual void SelectItem()
            {
                m_itemViewList.CurSelectedItemIndex = m_itemViewList.GetItemIndex(this);

                m_itemViewList.UpdateModuleState(null);

                CanvasTransitionManagerBuffer.GetCanvasTransitionManager(m_itemViewList.AttachedCanvasTransitionManagerGuid).OpenCanvas(m_itemViewList.GetItemIndex(this));
            }

            public abstract void OnItemSelected(in object itemData, in bool bisSelected);
        }


        [Header("Item View Item Prefab")]
        [SerializeField] private GameObject m_prefab_item;

        [Header("Item View Item Parent")]
        [SerializeField] private Transform m_transform_itemParent;

        private List<Item> m_items = new List<Item>();

        private int m_curSelectedItemIndex = 0;

        private Guid m_attachedCanvasTransitionManagerGuid;


        public List<Item> Items
        {
            get
            {
                return m_items;
            }
        }
        public int ItemCount
        {
            get
            {
                return m_items.Count;
            }
        }

        public int CurSelectedItemIndex
        {
            get
            {
                return m_curSelectedItemIndex;
            }
            set
            {
                if (0 <= value && value < ItemCount)
                {
                    m_curSelectedItemIndex = value;
                }
            }
        }

        public Guid AttachedCanvasTransitionManagerGuid
        {
            get
            {
                return m_attachedCanvasTransitionManagerGuid;
            }
        }

        public virtual void AddItem()
        {
            AddItem(null);
        }
        protected void AddItem(in object itemInitData)
        {
            GameObject newItem = Instantiate(m_prefab_item, m_transform_itemParent);
            Item item = newItem.GetComponent<Item>();

            item.InitializeItem(this, itemInitData);

            m_items.Add(item);
        }

        internal void RemoveItem(in Item itemComponentClass)
        {
            m_items.Remove(itemComponentClass);
        }

        public virtual void RemoveItemAt(in int index)
        {
            m_items.RemoveAt(index);
        }

        public virtual void ClearItems()
        {
            foreach (var itemComponentClass in m_items)
            {
                Destroy(itemComponentClass.gameObject);
            }
            m_items.Clear();
        }

        public virtual int GetItemIndex(in Item itemComponentClass)
        {
            return m_items.IndexOf(itemComponentClass);
        }

        public override void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid)
        {
            m_attachedCanvasTransitionManagerGuid = attachedCanvasTransitionManagerGuid;

            InitializeModule(moduleInitData as List<object>);
        }
        protected void InitializeModule(in List<object> moduleInitDataPerItem)
        {
            if (moduleInitDataPerItem != null && moduleInitDataPerItem.Count == m_items.Count)
            {
                for (int index = 0; index < m_items.Count; index++)
                {
                    m_items[index].InitializeItem(this, moduleInitDataPerItem[index]);
                }
            }
            else
            {
                foreach (var item in m_items)
                {
                    item.InitializeItem(this, null);
                }
            }
        }

        public override void UpdateModuleState(in object moduleUpdateData)
        {
            UpdateModuleState((List<object>)moduleUpdateData);
        }
        protected void UpdateModuleState(in List<object> moduleUpdateDataPerItem)
        {
            if (moduleUpdateDataPerItem != null && moduleUpdateDataPerItem.Count == m_items.Count)
            {
                for (int index = 0; index < m_items.Count; index++)
                {
                    m_items[index].UpdateItemState(moduleUpdateDataPerItem[index]);
                }
            }
            else
            {
                foreach (var item in m_items)
                {
                    item.UpdateItemState(null);
                }
            }
        }
    }
}