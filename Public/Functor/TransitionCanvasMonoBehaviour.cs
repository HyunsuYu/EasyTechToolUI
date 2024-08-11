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

        private List<IModuleStateUpdate> m_moduleStateUpdates = new List<IModuleStateUpdate>();


        public virtual void OnTransition(in string from)
        {

        }

        public virtual void InitializeAwake()
        {

        }

        public virtual void InitializeModule()
        {
            foreach(var moduleStateUpdate in m_moduleStateUpdates)
            {
                moduleStateUpdate.InitializeModule();
            }
        }
        public virtual void UpdateModuleState()
        {
            foreach (var moduleStateUpdate in m_moduleStateUpdates)
            {
                moduleStateUpdate.UpdateModuleState();
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
