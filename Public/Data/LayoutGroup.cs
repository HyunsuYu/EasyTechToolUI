using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI.LayoutGroup
{
    public sealed class LayoutGroup : EdgyModulePrototype
    {
        [Header("EdgyModulePrototypes")]
        [SerializeField] private List<EdgyModulePrototype> m_edgyModulePrototypes;


        public List<EdgyModulePrototype> EdgyModulePrototypes
        {
            get
            {
                return m_edgyModulePrototypes;
            }
        }

        public override void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid)
        {
            foreach(var edgyModulePrototype in m_edgyModulePrototypes)
            {
                edgyModulePrototype.InitializeModule(moduleInitData, attachedCanvasTransitionManagerGuid);
            }
        }

        public override void UpdateModuleState(in object moduleUpdateData)
        {
            foreach (var edgyModulePrototype in m_edgyModulePrototypes)
            {
                edgyModulePrototype.UpdateModuleState(moduleUpdateData);
            }
        }
    }
}