using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


namespace EasyTechToolUI
{
    /// <summary>
    /// The interface defined by the method for processing when an item in the UI element is selected
    /// </summary>
    public interface IItemSelectedAction
    {
        /// <summary>
        /// Called when one of the UI elements to which the item belongs is selected
        /// </summary>
        /// <param name="itemData">A parameter seat for custom data that can be used as needed</param>
        /// <param name="bisSelected">Indicates whether the current item has been selected</param>
        void OnItemSelected(in object itemData, in bool bisSelected);
    }
}