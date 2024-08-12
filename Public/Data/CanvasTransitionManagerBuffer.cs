using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    public static class CanvasTransitionManagerBuffer
    {
        private static Dictionary<Guid, CanvasTransitionManager> m_canvasTransitonManagerTable = new Dictionary<Guid, CanvasTransitionManager>();


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