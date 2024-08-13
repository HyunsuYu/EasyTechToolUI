using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UIElements;


namespace EasyTechToolUI
{
    /// <summary>
    /// EasyTechToolUI's top administrator is responsible for initializing, updating, and transition of pages
    /// </summary>
    public class CanvasTransitionManager : EdgyModulePrototype
    {
        /// <summary>
        /// Define the methods that must be implemented when each page is transition by the CanvasTransitionManager
        /// </summary>
        public interface ITransitionEventSub
        {
            /// <summary>
            /// Called when the screen switches to the page that inherited the corresponding ITransitionEventSub
            /// </summary>
            /// <param name="from" >Name of the screen before it was switched to the current screen</param>
            void OnTransition(in string from);

            /// <summary>
            /// The page that inherited ITTransitionEventSub is called by the appropriate CanvasTransitionManager when it is initialized for the first time
            /// </summary>
            /// <param name="attachedCanvasTransitionManagerGuid"></param>
            void InitializeAwake(in Guid attachedCanvasTransitionManagerGuid);
        }


        [Header("Canvas Settings")]
        [SerializeField] private List<Canvas> m_canvases;
        [SerializeField] private List<string> m_canvasNames;

        [Header("First Screen Canvas Index")]
        [SerializeField] private int m_firstScreenCanvasIndex = 0;

        [Header("Page Transition Controll")]
        [SerializeField] private EdgyModulePrototype m_pageTransitionControll;

        private Dictionary<string, ITransitionEventSub> m_canvasTransitionEventSub;
        private string m_prevCanvasName;

        private List<IModuleStateUpdate> m_moduleStateUpdates = new List<IModuleStateUpdate>();

        private int m_curCanvasIndex = 0;

        private Guid m_canvasTransitionManagerGuid;


        private void Awake()
        {
            m_canvasTransitionManagerGuid = Guid.NewGuid();
            CanvasTransitionManagerBuffer.AddCanvasTransitionManager(m_canvasTransitionManagerGuid, this);

            if (m_canvases.Count != m_canvasNames.Count)
            {
                Debug.LogError("CanvasTransitionManager: Awake: Canvas count and Canvas name count does not match");
            }

            m_canvasTransitionEventSub = new Dictionary<string, ITransitionEventSub>();
            for (int index = 0; index < m_canvases.Count; index++)
            {
                m_canvasTransitionEventSub.Add(m_canvasNames[index], m_canvases[index].GetComponent<ITransitionEventSub>());
                m_moduleStateUpdates.Add(m_canvases[index].GetComponent<IModuleStateUpdate>());
            }

            foreach (string canvasName in m_canvasNames)
            {
                m_canvasTransitionEventSub[canvasName].InitializeAwake(m_canvasTransitionManagerGuid);
            }

            InitializeModule(null as object, m_canvasTransitionManagerGuid);
            UpdateModuleState(null as object);

            if (m_canvases.Count == 0)
            {
                m_firstScreenCanvasIndex = -1;
            }

            foreach (Canvas canvas in m_canvases)
            {
                canvas.gameObject.SetActive(false);
            }

            if(m_firstScreenCanvasIndex == -1)
            {
                throw new Exception("CanvasTransitionManager: Awake: FirstScreenCanvasIndex not set");
            }
            m_canvases[m_firstScreenCanvasIndex].gameObject.SetActive(true);

            m_prevCanvasName = m_canvasNames[m_firstScreenCanvasIndex];
        }

        public List<Canvas> Canvases
        {
            get
            {
                return m_canvases;
            }
        }
        public List<string> CanvasNames
        {
            get
            {
                return m_canvasNames;
            }
        }

        public int FirstScreenCanvasIndex
        {
            get
            {
                return m_firstScreenCanvasIndex;
            }
            set
            {
                if(0 <= value && value < m_canvasTransitionEventSub.Values.Count)
                {
                    m_firstScreenCanvasIndex = value;
                }
            }
        }

        /// <summary>
        /// It refers to the EdgeModulePrototype for managing UI elements to be used for page transition
        /// </summary>
        public EdgyModulePrototype PageTransitionControll
        {
            get
            {
                return m_pageTransitionControll;
            }
        }

        public Dictionary<string, ITransitionEventSub> CanvasTransitionEventSub
        {
            get
            {
                return m_canvasTransitionEventSub;
            }
        }
        public string PrevCanvasName
        {
            get
            {
                return m_prevCanvasName;
            }
        }

        public int CurCanvasIndex
        {
            get
            {
                return m_curCanvasIndex;
            }
        }
        public Guid CanvasTransitionManagerGuid
        {
            get
            {
                return m_canvasTransitionManagerGuid;
            }
        }

        /// <summary>
        /// Transition to canvas on the corresponding m_canvas using the given int as index
        /// </summary>
        /// <param name="canvasIndex">Index of the page you want to switch to</param>
        public virtual void OpenCanvas(int canvasIndex)
        {
            OpenCanvas(canvasIndex, null);
        }
        protected void OpenCanvas(in int canvasIndex, in object moduleUpdateData)
        {
            if (canvasIndex == -1)
            {
                Debug.LogError("CanvasTransitionManager: OpenCanvas: CanvasType not found");
                return;
            }
            else if (m_canvasNames.Count <= canvasIndex)
            {
                Debug.LogError("CanvasTransitionManager: OpenCanvas: CanvasType not found");
                return;
            }

            foreach (Canvas canvas in m_canvases)
            {
                canvas.gameObject.SetActive(false);
            }
            m_canvases[canvasIndex].gameObject.SetActive(true);
            if (m_canvasTransitionEventSub[m_canvasNames[canvasIndex]] != null)
            {
                m_canvasTransitionEventSub[m_canvasNames[canvasIndex]].OnTransition(m_prevCanvasName);
                m_moduleStateUpdates[canvasIndex].UpdateModuleState(moduleUpdateData);
            }

            m_prevCanvasName = m_canvasNames[canvasIndex];

            m_curCanvasIndex = canvasIndex;
            m_pageTransitionControll.UpdateModuleState(null);
        }

        public override void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid)
        {
            InitializeModule(moduleInitData as List<object>);
        }
        protected void InitializeModule(in List<object> moduleInitDataPerCanvas)
        {
            m_pageTransitionControll.InitializeModule(null, m_canvasTransitionManagerGuid);

            if(moduleInitDataPerCanvas != null && moduleInitDataPerCanvas.Count == m_canvases.Count)
            {
                for (int index = 0; index < m_canvases.Count; index++)
                {
                    m_moduleStateUpdates[index].InitializeModule(moduleInitDataPerCanvas[index], m_canvasTransitionManagerGuid);
                }
            }
            else
            {
                foreach (var moduleStateUpdate in m_moduleStateUpdates)
                {
                    moduleStateUpdate.InitializeModule(null, m_canvasTransitionManagerGuid);
                }
            }
        }

        public override void UpdateModuleState(in object moduleUpdateData)
        {
            UpdateModuleState(null as List<object>);
        }
        protected void UpdateModuleState(in List<object> moduleUpdateDataPerCanvas)
        {
            m_pageTransitionControll.UpdateModuleState(null);

            if (moduleUpdateDataPerCanvas != null && moduleUpdateDataPerCanvas.Count == m_canvases.Count)
            {
                for (int index = 0; index < m_canvases.Count; index++)
                {
                    m_moduleStateUpdates[index].UpdateModuleState(moduleUpdateDataPerCanvas[index]);
                }
            }
            else
            {
                foreach (var moduleStateUpdate in m_moduleStateUpdates)
                {
                    moduleStateUpdate.UpdateModuleState(null);
                }
            }
        }
    }
}
