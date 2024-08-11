using EasyTechToolUI.Public.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    public abstract class TransitionCanvasMonoBehaviour<_TransitionCanvasCommonDataBuffer> : MonoBehaviour, CanvasTransitionManager.ITransitionEventSub, IModuleStateUpdate where _TransitionCanvasCommonDataBuffer : new()
    {
        private static _TransitionCanvasCommonDataBuffer m_commonDataBuffer;

        [Header("Edgy Module Prototypes")]
        [SerializeField] private List<EdgyModulePrototype> m_edgyModulePrototypes;


        public virtual void OnTransition(in string from)
        {

        }

        public virtual void InitializeAwake()
        {
            InitializeModule(null);
        }

        public virtual void InitializeModule(in object moduleInitData)
        {
            InitializeModule(moduleInitData);
        }
        protected void InitializeModule(in List<object> moduleInitDataPerSubModule)
        {
            if(moduleInitDataPerSubModule != null && moduleInitDataPerSubModule.Count == m_edgyModulePrototypes.Count)
            {
                for(int index = 0; index < moduleInitDataPerSubModule.Count; index++)
                {
                    m_edgyModulePrototypes[index].InitializeModule(moduleInitDataPerSubModule[index]);
                }
            }
            else
            {
                foreach(var moduleStateUpdate in m_edgyModulePrototypes)
                {
                    moduleStateUpdate.InitializeModule(null);
                }
            }
        }

        public virtual void UpdateModuleState(in object moduleInitData)
        {
            UpdateModuleState(moduleInitData);
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
    }
}
