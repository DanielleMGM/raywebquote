using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGM_Transformer
{
    public partial class AdminUploadPrices : System.Web.UI.Page
    {
        // Import.
        protected void btnImport_Click(object sender, EventArgs e)
        {
            Prices p = new Prices();

            p.Import(1);          // Custom Aluminum.
            p.Import(2);          // Custom Copper.
            bool retval = p.Import(3);          // Stock / Reps.

            string sMsg = "";
            if (retval)
                sMsg = "Imported.";
            else
                sMsg = "No data to import.";

            Response.Write("<script>alert('" + sMsg + "');</script>");
        }
    }
}