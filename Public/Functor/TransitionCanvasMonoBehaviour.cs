using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    public abstract class TransitionCanvasMonoBehaviour<_TransitionCanvasCommonDataBuffer> : MonoBehaviour, CanvasTransitionManager.ITranssitionEventSub where _TransitionCanvasCommonDataBuffer : new()
    {
        private static _TransitionCanvasCommonDataBuffer m_commonDataBuffer;


        public virtual void OnTransition(in string from)
        {

        }

        public virtual void Initialize()
        {

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
