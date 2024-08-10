using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    public interface IModuleStateUpdate
    {
        void InitializeModule();
        void UpdateModuleState();
    }
}