using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;


namespace EasyTechToolUI.ItemGridPlane
{
    public abstract class ItemGridPlane : EdgyModulePrototype
    {
        public interface IItemStateUpdate
        {
            void InitializeItem(in ItemGridPlane itemGridPlane, in int spawnedItemIndex, in object itemInitData);

            void UpdateItemState(in object itemUpdateData);
        }

        public enum AbstractItemPlacementRegulation : byte
        {
            NoRegulation,
            WarningWhenInvalid,
            LogErrorWhenInvalid,
            SliceItemSizeToFit
        }

        public abstract class Item : MonoBehaviour, IItemStateUpdate, IItemSelectedAction
        {
            private ItemGridPlane m_itemGridPlane;

            private int m_spawnedItemIndex;


            public ItemGridPlane AttahcedItemGridPlane
            {
                get
                {
                    return m_itemGridPlane;
                }
            }
            public int SpawnedItemIndex
            {
                get
                {
                    return m_spawnedItemIndex;
                }
            }

            public virtual void InitializeItem(in ItemGridPlane itemGridPlane, in int spawnedItemIndex, in object itemInitData)
            {
                m_itemGridPlane = itemGridPlane;

                m_spawnedItemIndex = spawnedItemIndex;
            }

            public virtual void UpdateItemState(in object itemUpdateData)
            {
                if (m_itemGridPlane != null)
                {
                    bool bisItemSelected = m_itemGridPlane.CurSelectedItemPos == m_itemGridPlane.GetItemPos(this);

                    OnItemSelected(itemUpdateData, bisItemSelected);
                }
            }

            public virtual void SelectItem(bool bdoCanvasTransiton = false)
            {
                m_itemGridPlane.CurSelectedItemPos = m_itemGridPlane.GetItemPos(this);

                m_itemGridPlane.UpdateModuleState(null);

                if (bdoCanvasTransiton)
                {
                    CanvasTransitionManagerBuffer.GetCanvasTransitionManager(m_itemGridPlane.AttachedCanvasTransitionManagerGuid).OpenCanvas(m_spawnedItemIndex);
                }
            }

            public abstract void OnItemSelected(in object itemData, in bool bisSelected);
        }


        [Header("Item Grid Plane Prefab")]
        [SerializeField] private GameObject m_prefab_item;

        [Header("Item Grid Plane Parent")]
        [SerializeField] private Transform m_transform_itemParent;

        [Header("Item Grid Plane Spawn Item Settings")]
        [SerializeField] private Vector2Int m_spawnItemPlaneSize;
        [SerializeField] private Vector2 m_abstractItemCellSize;
        [SerializeField] private Vector2 m_abstractItemSpacing;

        private Vector2Int m_adjustedItemPlaneSize;

        [Header("Regulation of Abstract Item Placement")]
        [SerializeField] private AbstractItemPlacementRegulation m_abstractItemPlacementRegulation = AbstractItemPlacementRegulation.NoRegulation;

        private Item[,] m_itemPlane;

        private GridLayoutGroup m_gridLayoutGroup;

        private Vector2Int m_curSelectedItemPos = Vector2Int.zero;

        private Guid m_attachedCanvasTransitionManagerGuid;


        public Item[,] ItemPlane
        {
            get
            {
                return m_itemPlane;
            }
        }
        public Vector2Int ItemPlaneSize
        {
            get
            {
                return m_spawnItemPlaneSize;
            }
            set
            {
                if(value.x < 1 || value.y < 1)
                {
                    Debug.LogError("Invalid Item Plane Size");
                    return;
                }
                m_spawnItemPlaneSize = value;
            }
        }
        public Vector2 AbstractItemCellSize
        {
            get
            {
                return m_abstractItemCellSize;
            }
            set
            {
                if (value.x < 1.0f || value.y < 1.0f)
                {
                    Debug.LogError("Invalid Item Cell Size");
                    return;
                }
                m_abstractItemCellSize = value;
            }
        }
        public Vector2 AbstractItemSpacing
        {
            get
            {
                return m_abstractItemSpacing;
            }
            set
            {
                if (value.x < 0.0f || value.y < 0.0f)
                {
                    Debug.LogError("Invalid Item Spacing");
                    return;
                }
                m_abstractItemSpacing = value;
            }
        }

        public Vector2Int AdjustedItemPlaneSize
        {
            get
            {
                return m_adjustedItemPlaneSize;
            }
        }

        public AbstractItemPlacementRegulation AbstractItemPlacementRegulationType
        {
            get
            {
                return m_abstractItemPlacementRegulation;
            }
            set
            {
                if (Enum.IsDefined(typeof(AbstractItemPlacementRegulation), value))
                {
                    m_abstractItemPlacementRegulation = value;
                }
            }
        }

        public Vector2Int CurSelectedItemPos
        {
            get
            {
                return m_curSelectedItemPos;
            }
            set
            {
                if (0 <= value.x && value.x < m_adjustedItemPlaneSize.x &&
                    0 <= value.y && value.y < m_adjustedItemPlaneSize.y)
                {
                    m_curSelectedItemPos = value;
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

        public virtual void ResizeItemPlane(in Vector2Int newitemPlaneSize)
        {
            if(newitemPlaneSize.x < 1 || newitemPlaneSize.y < 1)
            {
                Debug.LogError("Invalid Item Plane Size");
                return;
            }

            ClearItemPlane();

            m_spawnItemPlaneSize = newitemPlaneSize;

            InitializeModule(null);
        }
        public virtual void ClearItemPlane()
        {
            if(m_itemPlane != null)
            {
                for (int coord_y = 0; coord_y < m_adjustedItemPlaneSize.y; coord_y++)
                {
                    for (int coord_x = 0; coord_x < m_adjustedItemPlaneSize.x; coord_x++)
                    {
                        Destroy(m_itemPlane[coord_y, coord_x].gameObject);
                        m_itemPlane[coord_y, coord_x] = null;
                    }
                }
                m_itemPlane = null;
            }
        }

        public virtual Vector2Int GetItemPos(in Item itemComponentClass)
        {
            for (int coord_y = 0; coord_y < m_adjustedItemPlaneSize.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < m_adjustedItemPlaneSize.x; coord_x++)
                {
                    if (m_itemPlane[coord_y, coord_x] == itemComponentClass)
                    {
                        return new Vector2Int(coord_x, coord_y);
                    }
                }
            }

            return -Vector2Int.one;
        }

        public override void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid)
        {
            m_attachedCanvasTransitionManagerGuid = attachedCanvasTransitionManagerGuid;

            m_gridLayoutGroup = m_transform_itemParent.GetComponent<GridLayoutGroup>();

            InitializeModule(moduleInitData as object[,]);
        }
        protected void InitializeModule(in object[,] moduleInitDataPerItem)
        {
            ClearItemPlane();

            bool bisInitDataValid = true;
            if (moduleInitDataPerItem == null)
            {
                bisInitDataValid = false;
            }
            else
            {
                if (moduleInitDataPerItem.GetLength(0) != m_spawnItemPlaneSize.y ||
                    moduleInitDataPerItem.GetLength(1) != m_spawnItemPlaneSize.x)
                {
                    bisInitDataValid = false;
                }
            }

            #region
            Vector2 parentPlaneBaseSize = m_transform_itemParent.GetComponent<RectTransform>().sizeDelta;

            bool bisItemXSizeValid = true;
            bool bisItemYSizeValid = true;
            if (m_abstractItemCellSize.x * m_spawnItemPlaneSize.x + m_abstractItemSpacing.x * (m_spawnItemPlaneSize.x - 1) > parentPlaneBaseSize.x)
            {
                bisItemXSizeValid = false;
            }
            if (m_abstractItemCellSize.y * m_spawnItemPlaneSize.y + m_abstractItemSpacing.y * (m_spawnItemPlaneSize.y - 1) > parentPlaneBaseSize.y)
            {
                bisItemYSizeValid = false;
            }

            m_adjustedItemPlaneSize = m_spawnItemPlaneSize;
            switch (m_abstractItemPlacementRegulation)
            {
                case AbstractItemPlacementRegulation.NoRegulation:
                    break;

                case AbstractItemPlacementRegulation.WarningWhenInvalid:
                    if(!bisItemXSizeValid || !bisItemYSizeValid)
                    {
                        Debug.LogWarning("Invalid Item Initialization Data");
                    }
                    break;

                case AbstractItemPlacementRegulation.LogErrorWhenInvalid:
                    if(!bisItemXSizeValid || !bisItemYSizeValid)
                    {
                        m_adjustedItemPlaneSize = Vector2Int.zero;
                        Debug.LogError("Invalid Item Initialization Data");
                    }
                    break;

                case AbstractItemPlacementRegulation.SliceItemSizeToFit:
                    {
                        if (!bisItemXSizeValid)
                        {
                            while(true)
                            {
                                m_adjustedItemPlaneSize.x -= 1;
                                if (!(m_abstractItemCellSize.x * m_adjustedItemPlaneSize.x + m_abstractItemSpacing.x * (m_adjustedItemPlaneSize.x - 1) > parentPlaneBaseSize.x))
                                {
                                    break;
                                }
                            }
                        }

                        if(!bisItemYSizeValid)
                        {
                            m_adjustedItemPlaneSize.y -= 1;
                            if (!(m_abstractItemCellSize.y * m_adjustedItemPlaneSize.y + m_abstractItemSpacing.y * (m_adjustedItemPlaneSize.y - 1) > parentPlaneBaseSize.y))
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
            #endregion

            #region
            switch (m_gridLayoutGroup.constraint)
            {
                case GridLayoutGroup.Constraint.Flexible:
                    Debug.LogError("Using GridLayoutGroup.Constraint.Flexible may produce different results than expected");
                    break;

                case GridLayoutGroup.Constraint.FixedColumnCount:
                    m_gridLayoutGroup.constraintCount = m_adjustedItemPlaneSize.x;
                    break;

                case GridLayoutGroup.Constraint.FixedRowCount:
                    m_gridLayoutGroup.constraintCount = m_adjustedItemPlaneSize.y;
                    break;
            }
            #endregion

            m_itemPlane = new Item[m_adjustedItemPlaneSize.y, m_adjustedItemPlaneSize.x];
            for(int coord_y = 0; coord_y < m_adjustedItemPlaneSize.y; coord_y++)
            {
                for(int coord_x = 0; coord_x < m_adjustedItemPlaneSize.x; coord_x++)
                {
                    GameObject newItem = Instantiate(m_prefab_item, m_transform_itemParent);
                    Item item = newItem.GetComponent<Item>();

                    int spawnedItemIndex = coord_y * m_adjustedItemPlaneSize.x + coord_x;

                    if (bisInitDataValid)
                    {
                        item.InitializeItem(this, spawnedItemIndex, moduleInitDataPerItem[coord_y, coord_x]);
                    }
                    else
                    {
                        item.InitializeItem(this, spawnedItemIndex, null);
                    }

                    m_itemPlane[coord_y, coord_x] = item;
                }
            }
        }

        public override void UpdateModuleState(in object moduleUpdateData)
        {
            UpdateModuleState(moduleUpdateData as object[,]);
        }
        protected void UpdateModuleState(in object[,] moduleUpdateDataPerItem)
        {
            bool bisUpdateDataValid = true;
            if (moduleUpdateDataPerItem == null)
            {
                bisUpdateDataValid = false;
            }
            else
            {
                if (moduleUpdateDataPerItem.GetLength(0) != m_spawnItemPlaneSize.y ||
                    moduleUpdateDataPerItem.GetLength(1) != m_spawnItemPlaneSize.x)
                {
                    bisUpdateDataValid = false;
                }
            }

            for (int coord_y = 0; coord_y < m_adjustedItemPlaneSize.y; coord_y++)
            {
                for (int coord_x = 0; coord_x < m_adjustedItemPlaneSize.x; coord_x++)
                {
                    if (bisUpdateDataValid)
                    {
                        m_itemPlane[coord_y, coord_x].UpdateItemState(moduleUpdateDataPerItem[coord_y, coord_x]);
                    }
                    else
                    {
                        m_itemPlane[coord_y, coord_x].UpdateItemState(null);
                    }
                }
            }
        }
    }
}