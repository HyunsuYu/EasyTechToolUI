using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    public abstract class EdgyModulePrototype : MonoBehaviour, IModuleStateUpdate
    {
        public abstract void InitializeModule(in object moduleInitData);
        public abstract void UpdateModuleState(in object moduleUpdateData);
    }
}