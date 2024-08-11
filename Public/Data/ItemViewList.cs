using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI.ItemViewList
{
    public interface IItemStateUpdate<_ItemComponentClass> where _ItemComponentClass : MonoBehaviour, IItemStateUpdate<_ItemComponentClass>
    {
        void InitializeItem(in ItemViewList<_ItemComponentClass> itemViewList, in object itemInitData);

        void UpdateItemState();
    }

    public abstract class ItemViewListItem : MonoBehaviour, IItemStateUpdate<ItemViewListItem>
    {
        private ItemViewList<ItemViewListItem> m_itemViewList;


        /// <summary>
        /// If you want to initialize item with init data, must be override this method and implement the custom initialization.
        /// </summary>
        /// <param name="itemViewList"></param>
        /// <param name="itemInitData"></param>
        [Obsolete]
        public void InitializeItem(in ItemViewList<ItemViewListItem> itemViewList, in object itemInitData) { }

        [Obsolete]
        public void UpdateItemState() { }

        public virtual void RemoveItem()
        {
            m_itemViewList.RemoveItem(this);
        }
        public virtual void SelectItem()
        {
            ItemViewList<ItemViewListItem>.CurSelectedItemIndex = m_itemViewList.GetItemIndex(this);
        }
    }

    public abstract class ItemViewList<_ItemComponentClass> : MonoBehaviour, IModuleStateUpdate where _ItemComponentClass : MonoBehaviour, IItemStateUpdate<_ItemComponentClass>
    {
        [Header("Item View Item Prefab")]
        [SerializeField] private GameObject m_prefab_item;

        [Header("Item View Item Parent")]
        [SerializeField] private Transform m_transform_itemParent;

        private List<_ItemComponentClass> m_itemComponentClasses = new List<_ItemComponentClass>();

        private static ItemViewList<_ItemComponentClass> m_instance;

        private static int m_curSelectedItemIndex = 0;


        private void Awake()
        {
            m_instance = this;
        }

        public static ItemViewList<_ItemComponentClass> Instance
        {
            get
            {
                return m_instance;
            }
        }

        public static int CurSelectedItemIndex
        {
            get
            {
                return m_curSelectedItemIndex;
            }
            set
            {
                if(0 <= value && value < Instance.ItemCount)
                {
                    m_curSelectedItemIndex = value;
                }
            }
        }

        public int ItemCount
        {
            get
            {
                return m_itemComponentClasses.Count;
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
            _ItemComponentClass itemComponentClass = newItem.GetComponent<_ItemComponentClass>();

            itemComponentClass.InitializeItem(this, itemInitData);
            itemComponentClass.UpdateItemState();

            m_itemComponentClasses.Add(itemComponentClass);
        }

        internal void RemoveItem(in _ItemComponentClass itemComponentClass)
        {
            m_itemComponentClasses.Remove(itemComponentClass);
        }

        public virtual void RemoveItemAt(in int index)
        {
            m_itemComponentClasses.RemoveAt(index);
        }

        public virtual void ClearItems()
        {
            foreach (var itemComponentClass in m_itemComponentClasses)
            {
                Destroy(itemComponentClass.gameObject);
            }
            m_itemComponentClasses.Clear();
        }

        public virtual int GetItemIndex(in _ItemComponentClass itemComponentClass)
        {
            return m_itemComponentClasses.IndexOf(itemComponentClass);
        }

        public virtual void InitializeModule()
        {

        }
        public virtual void UpdateModuleState()
        {

        }
    }
}