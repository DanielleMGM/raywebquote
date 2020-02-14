using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace MGM_Transformer
{
    public class UIHelper
    {
        /// <summary>
        /// Get Row for OnRowCommand.
        /// </summary>
        /// <param name="currentControl"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Control FindAncestor(Control currentControl, System.Type type)
        {

            Control control = currentControl;

            while (control != null && control.GetType() != type)
            {
                control = control.Parent;

            } return control;

        }
    }
}