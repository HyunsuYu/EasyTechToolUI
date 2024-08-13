using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    /// <summary>
    /// A buffer that helps store and identify all CanvasTransitionManagers present in the program.
    /// </summary>
    public static class CanvasTransitionManagerBuffer
    {
        private static Dictionary<Guid, CanvasTransitionManager> m_canvasTransitonManagerTable = new Dictionary<Guid, CanvasTransitionManager>();


        /// <summary>
        /// Method that returns the corresponding CanvasTransitionManager according to the given Guid
        /// </summary>
        /// <param name="canvasTransitionManagerGuid">Guid corresponding to the CanvasTranstionManager you want to find</param>
        /// <returns>CanvasTransitionManager instance found via Guid. If there is no appropriate instance corresponding to Guid, null is returned</returns>
        public static CanvasTransitionManager GetCanvasTransitionManager(in Guid canvasTransitionManagerGuid)
        {
            if (m_canvasTransitonManagerTable.ContainsKey(canvasTransitionManagerGuid))
            {
                return m_canvasTransitonManagerTable[canvasTransitionManagerGuid];
            }
            else
            {
                Debug.LogError("CanvasTransitionManagerBuffer: GetCanvasTransitionManager: CanvasTransitionManager not found");
                return null;
            }
        }
        /// <summary>
        /// Method to register a new CanvasTransitionManager
        /// </summary>
        /// <param name="canvasTransitionManagerGuid">Guid of the CanvasTransitionManager instance you wish to register</param>
        /// <param name="canvasTransitionManager">The CanvasTransitionManager instance you wish to register</param>
        public static void AddCanvasTransitionManager(in Guid canvasTransitionManagerGuid, in CanvasTransitionManager canvasTransitionManager)
        {
            if (m_canvasTransitonManagerTable.ContainsKey(canvasTransitionManagerGuid))
            {
                Debug.LogError("CanvasTransitionManagerBuffer: AddCanvasTransitionManager: CanvasTransitionManager already exists");
            }
            else
            {
                m_canvasTransitonManagerTable.Add(canvasTransitionManagerGuid, canvasTransitionManager);
            }
        }

        public static List<Guid> CanvasTransitionManagerGuids
        {
            get
            {
                return m_canvasTransitonManagerTable.Keys.ToList();
            }
        }
        public static List<CanvasTransitionManager> CanvasTransitionManagers
        {
            get
            {
                return m_canvasTransitonManagerTable.Values.ToList();
            }
        }
    }
}