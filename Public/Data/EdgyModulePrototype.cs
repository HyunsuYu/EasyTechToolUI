using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    /// <summary>
    /// An abstract class that collects things to manage the UI module located at the bottom of the UI abstraction tree.
    /// </summary>
    public abstract class EdgyModulePrototype : MonoBehaviour, IModuleStateUpdate
    {
        public abstract void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid);
        public abstract void UpdateModuleState(in object moduleUpdateData);
    }
}