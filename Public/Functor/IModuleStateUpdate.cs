using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    /// <summary>
    /// Interface that defines the methods necessary to manage the UI module
    /// </summary>
    public interface IModuleStateUpdate
    {
        /// <summary>
        /// You can call the module when you initialize it
        /// </summary>
        /// <param name="moduleInitData">This refers to custom data to be used when initializing modules. In some cases, null is also allowed</param>
        /// <param name="attachedCanvasTransitionManagerGuid">This is the Guid of the CanvasTransitionManager to which the module belongs</param>
        void InitializeModule(in object moduleInitData, in Guid attachedCanvasTransitionManagerGuid);
        /// <summary>
        /// You can call the module when you updating it
        /// </summary>
        /// <param name="moduleUpdateData">This refers to custom data to be used when updating modules. In some cases, null is also allowed</param>
        void UpdateModuleState(in object moduleUpdateData);
    }
}