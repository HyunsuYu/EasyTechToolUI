using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    /// <summary>
    /// Class that describe each page that is the subject of the screen
    /// </summary>
    /// <typeparam name="_TransitionCanvasCommonDataBuffer">Dedicated data buffer for TransitionCanvasMonoBehaviour</typeparam>
    public abstract class TransitionCanvasMonoBehaviour<_TransitionCanvasCommonDataBuffer> : EdgyModulePrototype, CanvasTransitionManager.ITransitionEventSub where _TransitionCanvasCommonDataBuffer : new()
    {
        private static _TransitionCanvasCommonDataBuffer m_commonDataBuffer;

        [Header("Edgy Module Prototypes")]
        [SerializeField] private List<EdgyModulePrototype> m_edgyModulePrototypes;

        private Guid m_attachedCanvasTransitionManagerGuid;


        /// <summary>
        /// This refers to the list of EdgyModulePrototypes for the management of UI elements on the current page
        /// </summary>
        public List<EdgyModulePrototype> EdgyModulePrototypes
        {
            get
            {
                return m_edgyModulePrototypes;
            }
        }
        /// <summary>
        /// Guid for Canvas Transition Manager who manages the current page
        /// </summary>
        public Guid AttachedCanvasTransitionManagerGuid
        { 
            get
            {
                return m_attachedCanvasTransitionManagerGuid;
            }
        }

        /// <summary>
        /// Reference to a dedicated data buffer for the current page
        /// </summary>
        public static _TransitionCanvasCommonDataBuffer CommonDataBuffer
        {
            get
            {
                if (m_commonDataBuffer == null)
                {
                    m_commonDataBuffer = new _TransitionCanvasCommonDataBuffer();
                }
                return m_commonDataBuffer;
            }
        }

        public abstract void OnTransition(in string from);

        public virtual void InitializeAwake(in Guid attachedCanvasTransitionManagerGuid)
        {
            InitializeModule(null, attachedCanvasTransitionManagerGuid);
        }

        public override void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid)
        {
            m_attachedCanvasTransitionManagerGuid = attachedCanvasTransitionManagerGuid;

            InitializeModule(moduleInitData as List<object>);
        }
        protected void InitializeModule(in List<object> moduleInitDataPerSubModule)
        {
            if(moduleInitDataPerSubModule != null && moduleInitDataPerSubModule.Count == m_edgyModulePrototypes.Count)
            {
                for(int index = 0; index < moduleInitDataPerSubModule.Count; index++)
                {
                    m_edgyModulePrototypes[index].InitializeModule(moduleInitDataPerSubModule[index], m_attachedCanvasTransitionManagerGuid);
                }
            }
            else
            {
                foreach(var moduleStateUpdate in m_edgyModulePrototypes)
                {
                    moduleStateUpdate.InitializeModule(null, m_attachedCanvasTransitionManagerGuid);
                }
            }
        }

        public override void UpdateModuleState(in object moduleInitData)
        {
            UpdateModuleState(moduleInitData as List<object>);
        }
        protected void UpdateModuleState(in List<object> moduleUpdateDataPerSubModule)
        {
            if (moduleUpdateDataPerSubModule != null && moduleUpdateDataPerSubModule.Count == m_edgyModulePrototypes.Count)
            {
                for (int index = 0; index < moduleUpdateDataPerSubModule.Count; index++)
                {
                    m_edgyModulePrototypes[index].UpdateModuleState(moduleUpdateDataPerSubModule[index]);
                }
            }
            else
            {
                foreach (var moduleStateUpdate in m_edgyModulePrototypes)
                {
                    moduleStateUpdate.UpdateModuleState(null);
                }
            }
        }
    }
}
