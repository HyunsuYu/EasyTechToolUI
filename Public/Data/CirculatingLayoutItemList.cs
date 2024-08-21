using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI.CirculatingLayoutItemList
{
    public abstract class CirculatingLayoutItemList : EdgyModulePrototype
    {
        public interface IItemStateUpdate
        {
            void InitializeItem(in CirculatingLayoutItemList circulatingLayoutItemList, in int absoluteItemIndex, in object itemInitData);

            void UpdateItemState(in object itemUpdateData);
        }

        public abstract class Item : MonoBehaviour, IItemStateUpdate, IItemSelectedAction
        {
            private CirculatingLayoutItemList m_circulatingLayoutItemList;

            private int m_absoluteItemIndex = -1;


            public CirculatingLayoutItemList AttahcedCirculatingLayoutItemList
            {
                get
                {
                    return m_circulatingLayoutItemList;
                }
            }

            public int AbsoluteItemIndex
            {
                get
                {
                    return m_absoluteItemIndex;
                }
            }

            public virtual void InitializeItem(in CirculatingLayoutItemList circulatingLayoutItemList, in int absoluteItemIndex, in object itemInitData)
            {
                m_circulatingLayoutItemList = circulatingLayoutItemList;

                m_absoluteItemIndex = absoluteItemIndex;
            }

            public virtual void UpdateItemState(in object itemUpdateData)
            {
                if (m_circulatingLayoutItemList != null)
                {
                    bool bisItemSelected = m_circulatingLayoutItemList.CurAbsoluteSelectedItemIndex == m_absoluteItemIndex;

                    OnItemSelected(itemUpdateData, bisItemSelected);
                }
            }

            public virtual void RemoveItem()
            {
                m_circulatingLayoutItemList.RemoveItem(this);
            }
            public virtual void SelectItem(bool bdoCanvasTransiton = false)
            {
                m_circulatingLayoutItemList.CurAbsoluteSelectedItemIndex = m_absoluteItemIndex;

                m_circulatingLayoutItemList.UpdateModuleState(null);

                m_circulatingLayoutItemList.ReorderItem();

                if(bdoCanvasTransiton)
                {
                    CanvasTransitionManagerBuffer.GetCanvasTransitionManager(m_circulatingLayoutItemList.AttachedCanvasTransitionManagerGuid).OpenCanvas(m_absoluteItemIndex);
                }
            }

            public abstract void OnItemSelected(in object itemData, in bool bisSelected);
        }


        [Header("Item Grid Plane Prefab")]
        [SerializeField] private GameObject m_prefab_item;

        [Header("Item View Item Parent")]
        [SerializeField] private Transform m_transform_itemParent;

        private List<Item> m_items = new List<Item>();

        private int m_curSelectedAbsoluteItemIndex = 0;

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

        public int CurAbsoluteSelectedItemIndex
        {
            get
            {
                return m_curSelectedAbsoluteItemIndex;
            }
            set
            {
                if (0 <= value && value < ItemCount)
                {
                    m_curSelectedAbsoluteItemIndex = value;
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
            GameObject itemObject = Instantiate(m_prefab_item, m_transform_itemParent);
            Item item = itemObject.GetComponent<Item>();

            item.InitializeItem(this, ItemCount - 1, itemInitData);

            m_items.Add(item);

            UpdateModuleState(null);
        }

        internal void RemoveItem(in Item itemComponentClass)
        {
            Destroy(itemComponentClass.gameObject);
            m_items.Remove(itemComponentClass);

            UpdateModuleState(null);
        }

        public virtual void RemoveItemAt(in int index)
        {
            if(m_curSelectedAbsoluteItemIndex == ItemCount - 1)
            {
                m_curSelectedAbsoluteItemIndex -= 1;
            }

            Destroy(m_items[index].gameObject);
            m_items.RemoveAt(index);

            UpdateModuleState(null);
        }

        public virtual void ClearItems()
        {
            m_curSelectedAbsoluteItemIndex = 0;

            foreach (Item item in m_items)
            {
                Destroy(item.gameObject);
            }
            m_items.Clear();

            UpdateModuleState(null);
        }

        internal void ReorderItem()
        {
            for (int index = 0; index < ItemCount; index++)
            {
                if(CurAbsoluteSelectedItemIndex != m_items[index].AbsoluteItemIndex)
                {
                    m_items[index].transform.SetSiblingIndex(ItemCount - 1);
                }
                else
                {
                    break;
                }
            }
        }

        public override void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid)
        {
            m_attachedCanvasTransitionManagerGuid = attachedCanvasTransitionManagerGuid;

            InitializeModule(moduleInitData as List<object>);
        }
        protected void InitializeModule(in List<object> moduleInitDataPerItem)
        {
            ClearItems();

            if(moduleInitDataPerItem != null && moduleInitDataPerItem.Count == m_items.Count)
            {
                for (int index = 0; index < m_items.Count; index++)
                {
                    m_items[index].InitializeItem(this, index, moduleInitDataPerItem[index]);
                }
            }
            else
            {
                for (int index = 0; index < m_items.Count; index++)
                {
                    m_items[index].InitializeItem(this, index, null);
                }
            }
        }

        public override void UpdateModuleState(in object moduleUpdateData)
        {
            UpdateModuleState(moduleUpdateData as List<object>);
        }
        public void UpdateModuleState(in List<object> moduleUpdateDataPerItem)
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