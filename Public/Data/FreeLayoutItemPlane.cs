using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI.FreeLayoutItemPlane
{
    public abstract class FreeLayoutItemPlane : EdgyModulePrototype
    {
        public interface IItemStateUpdate
        {
            void InitializeItem(in FreeLayoutItemPlane freeLayoutItemPlane, in object itemInitData);

            void UpdateItemState(in object itemUpdateData);
        }

        public abstract class Item : MonoBehaviour, IItemStateUpdate, IItemSelectedAction
        {
            private FreeLayoutItemPlane m_freeLayoutItemPlane;


            public FreeLayoutItemPlane AttahcedFreeLayoutItemPlane
            {
                get
                {
                    return m_freeLayoutItemPlane;
                }
            }

            public void InitializeItem(in FreeLayoutItemPlane freeLayoutItemPlane, in object itemInitData)
            {
                m_freeLayoutItemPlane = freeLayoutItemPlane;
            }

            public void UpdateItemState(in object itemUpdateData)
            {
                if (m_freeLayoutItemPlane != null)
                {
                    bool bisItemSelected = m_freeLayoutItemPlane.CurSelectedItemIndex == m_freeLayoutItemPlane.GetItemIndex(this);

                    OnItemSelected(itemUpdateData, bisItemSelected);
                }
            }

            public virtual void RemoveItem()
            {
                m_freeLayoutItemPlane.RemoveItem(this);
            }
            public virtual void SelectItem(bool bdoCanvasTransiton = false)
            {
                m_freeLayoutItemPlane.CurSelectedItemIndex = m_freeLayoutItemPlane.GetItemIndex(this);

                m_freeLayoutItemPlane.UpdateModuleState(null);

                if (bdoCanvasTransiton)
                {
                    CanvasTransitionManagerBuffer.GetCanvasTransitionManager(m_freeLayoutItemPlane.AttachedCanvasTransitionManagerGuid).OpenCanvas(m_freeLayoutItemPlane.GetItemIndex(this));
                }
            }

            public abstract void OnItemSelected(in object itemData, in bool bisSelected);
        }

        /// <summary>
        /// This event is called when item addition is faced on limit
        /// </summary>
        public delegate void MaxItemCountEvent();


        [Header("Item Grid Plane Prefab")]
        [SerializeField] private GameObject m_prefab_item;

        [Header("Item Spawn Poses")]
        [SerializeField] List<Transform> m_itemSpawnPoses;

        protected MaxItemCountEvent MaxItemCountEventHandler { get; set; }

        private List<Item> m_items = new List<Item>();

        private int m_curSelectedItemIndex;

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
            if(ItemCount >= m_itemSpawnPoses.Count)
            {
                MaxItemCountEventHandler();
            }

            GameObject newItem = Instantiate(m_prefab_item, m_itemSpawnPoses[ItemCount]);
            Item item = newItem.GetComponent<Item>();

            item.InitializeItem(this, itemInitData);

            m_items.Add(item);

            UpdateModuleState(null);
        }

        internal void RemoveItem(in Item itemComponentClass)
        {
            Destroy(itemComponentClass.gameObject);
            m_items.Remove(itemComponentClass);

            for(int index = 0; index < ItemCount; index++)
            {
                m_items[index].transform.SetParent(m_itemSpawnPoses[index]);
                m_items[index].GetComponent<RectTransform>().SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            UpdateModuleState(null);
        }

        public virtual void RemoveItemAt(in int index)
        {
            if (m_curSelectedItemIndex == ItemCount - 1)
            {
                m_curSelectedItemIndex -= 1;
            }

            Destroy(m_items[index].gameObject);
            m_items.RemoveAt(index);

            UpdateModuleState(null);
        }

        public virtual void ClearItems()
        {
            foreach(Item item in m_items)
            {
                Destroy(item.gameObject);
            }
            m_items.Clear();

            UpdateModuleState(null);
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
            ClearItems();

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
            UpdateModuleState(moduleUpdateData as List<object>);
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