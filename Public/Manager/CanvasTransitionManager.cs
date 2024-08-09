using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    public class CanvasTransitionManager : MonoBehaviour
    {
        public interface ITranssitionEventSub
        {
            void OnTransition(in string from);

            void Initialize();
        }


        [SerializeField] private List<Canvas> m_canvases;
        [SerializeField] private List<string> m_canvasNames;
        [SerializeField] private int m_firstScreenCanvasIndex;

        private Dictionary<string, ITranssitionEventSub> m_canvasTransitionEventSub;
        private string m_prevCanvasName;

        private static CanvasTransitionManager m_instance;


        public static CanvasTransitionManager Instance
        {
            get
            {
                return m_instance;
            }
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
        public Dictionary<string, ITranssitionEventSub> CanvasTransitionEventSub
        {
            get
            {
                return m_canvasTransitionEventSub;
            }
        }

        private void Awake()
        {
            if(m_instance != null)
            {
                Debug.LogError("More than one instance of Canvas Transition Manager exists in the game");
            }
            m_instance = this;

            if(m_canvases.Count != m_canvasNames.Count)
            {
                Debug.LogError("CanvasTransitionManager: Awake: Canvas count and Canvas name count does not match");
            }

            m_canvasTransitionEventSub = new Dictionary<string, ITranssitionEventSub>();
            for (int index = 0; index < m_canvases.Count; index++)
            {
                m_canvasTransitionEventSub.Add(m_canvasNames[index], m_canvases[index].GetComponent<ITranssitionEventSub>());
            }

            foreach(string canvasName in m_canvasNames)
            {
                m_canvasTransitionEventSub[canvasName].Initialize();
            }

            foreach (Canvas canvas in m_canvases)
            {
                canvas.gameObject.SetActive(false);
            }
            m_canvases[m_firstScreenCanvasIndex].gameObject.SetActive(true);

            m_prevCanvasName = m_canvasNames[m_firstScreenCanvasIndex];
        }

        public void OpenCanvas(int canvasIndex)
        {
            if (canvasIndex == -1)
            {
                Debug.LogError("CanvasTransitionManager: OpenCanvas: CanvasType not found");
                return;
            }
            else if(m_canvasNames.Count <= canvasIndex)
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
            }

            m_prevCanvasName = m_canvasNames[canvasIndex];
        }
    }
}
