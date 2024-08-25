using EasyTechToolUI.ItemViewList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;


namespace EasyTechToolUI.SliderWrapper
{
    public abstract class SliderWrapper : EdgyModulePrototype
    {
        public enum SliderWrapperOption : byte
        {
            NpOption,
            ForCanvasTransition,
            ForLayoutGroupUpdate,
            ForOtherInstruction
        }


        [Header("Slider")]
        [SerializeField] private Slider m_slider;

        [Header("Option")]
        [SerializeField] private SliderWrapperOption m_option;

        [Header("Layout Group")]
        [SerializeField] private List<LayoutGroup.LayoutGroup> m_layoutGroups;
        [SerializeField] private bool m_bdoLayoutGroupTransition = false;

        private Guid m_attachedCanvasTransitionManagerGuid;


        public SliderWrapperOption Option
        {
            get
            {
                return m_option;
            }
            set
            {
                if (Enum.IsDefined(typeof(SliderWrapperOption), value))
                {
                    m_option = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("The value is not defined in the SliderWrapperOption enum");
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

        /// <summary>
        /// If true, each LayoutGroup is treated as if it were a separate page. However, if false, each LayoutGroup is treated as if it is a UI area belonging to one page.
        /// </summary>
        /// <remarks>If false, UpdateModuleState() of all modules of all LayoutGroup is called. At this time, the object parameter is filled with the value of the slider, so each module will need appropriate code to process this.</remarks>
        public bool BdoLayoutGroupTransition
        {
            get
            {
                return m_bdoLayoutGroupTransition;
            }
        }

        public int CurSliderValue
        {
            get
            {
                return (int)m_slider.value;
            }
            set
            {
                if(m_slider.minValue <= value && value <= m_slider.maxValue)
                {
                    m_slider.value = value;
                }
            }
        }

        /// <summary>
        /// This method must be added to Slider's onValueChanged event
        /// </summary>
        public void OnSliderValueChanged()
        {
            switch (m_option)
            {
                case SliderWrapperOption.ForCanvasTransition:
                    CanvasTransitionManagerBuffer.GetCanvasTransitionManager(m_attachedCanvasTransitionManagerGuid).OpenCanvas(CurSliderValue);
                    break;

                case SliderWrapperOption.ForLayoutGroupUpdate:
                    if(BdoLayoutGroupTransition)
                    {
                        if (m_layoutGroups.Count != m_slider.maxValue)
                        {
                            Debug.LogError("The number of layout groups is not equal to the maximum value of the slider");
                        }
                        else
                        {
                            for (int index = 0; index < m_layoutGroups.Count; index++)
                            {
                                foreach (EdgyModulePrototype edgyModulePrototype in m_layoutGroups[index].EdgyModulePrototypes)
                                {
                                    if (index == CurSliderValue)
                                    {
                                        edgyModulePrototype.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        edgyModulePrototype.gameObject.SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for(int index = 0; index < m_layoutGroups.Count; index++)
                        {
                            foreach (EdgyModulePrototype edgyModulePrototype in m_layoutGroups[index].EdgyModulePrototypes)
                            {
                                edgyModulePrototype.UpdateModuleState(CurSliderValue);
                            }
                        }
                    }
                    break;

                case SliderWrapperOption.ForOtherInstruction:
                    ExcuteOtherInstruction(CurSliderValue);
                    break;
            }
        }

        protected abstract void ExcuteOtherInstruction(in int value);

        public override void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid)
        {
            m_attachedCanvasTransitionManagerGuid = attachedCanvasTransitionManagerGuid;

            m_slider.minValue = 0;
            m_slider.maxValue = CanvasTransitionManagerBuffer.CanvasTransitionManagers.Count - 1;
        }
    }
}