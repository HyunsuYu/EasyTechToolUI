using EasyTechToolUI.Public.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI.ItemViewList
{
    public abstract class ItemViewList : EdgyModulePrototype
    {
        public interface IItemStateUpdate
        {
            void InitializeItem(in ItemViewList itemViewList, in object itemInitData);

            void UpdateItemState(in object itemUpdateData);
        }

        public abstract class Item : MonoBehaviour, IItemStateUpdate
        {
            private ItemViewList m_itemViewList;


            public virtual void InitializeItem(in ItemViewList itemViewList, in object itemInitData)
            {
                m_itemViewList = itemViewList;
            }

            public virtual void UpdateItemState(in object itemUpdateData)
            {

            }

            public virtual void RemoveItem()
            {
                m_itemViewList.RemoveItem(this);
            }
            public virtual void SelectItem()
            {
                m_itemViewList.CurSelectedItemIndex = m_itemViewList.GetItemIndex(this);
            }
        }


        [Header("Item View Item Prefab")]
        [SerializeField] private GameObject m_prefab_item;

        [Header("Item View Item Parent")]
        [SerializeField] private Transform m_transform_itemParent;

        private List<Item> m_items = new List<Item>();

        private static int m_curSelectedItemIndex = 0;


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
        public int ItemCount
        {
            get
            {
                return m_items.Count;
            }
        }

        /// <summary>
        /// If you want to add item with init data, must be override this method and pass the init data to AddItem(in object itemInitData) method.
        /// </summary>
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

        public override void InitializeModule(in object moduleInitData)
        {

        }
        public override void UpdateModuleState(in object moduleUpdateData)
        {

        }
    }
}