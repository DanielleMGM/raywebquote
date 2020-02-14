using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace MGM_Transformer
{
    public partial class ArticlesBody : System.Web.UI.UserControl
    {
        Quotes q = new Quotes();

        void Session_Start(object sender, EventArgs e)
        {
            Response.Redirect("http://www.mgmtransformer.com");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void LoadGrid()
        {
            string sArticles = "SELECT ArticleId, ArticleNo, ArticleName, CONVERT(VARCHAR(10), ArticleDate, 101) AS ArticleDate FROM Articles ORDER BY ArticleNo desc";

            dsArticles.SelectCommand = sArticles;
            gvArticles.DataSource = dsArticles;
            gvArticles.DataBind();
        }

        protected void gvArticles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewArticle")
            {
                int iArticleID = Convert.ToInt32(e.CommandArgument);

                LaunchArticle(iArticleID);
            }
        }

        protected void LaunchArticle(int iArticleID)
        {

            string sFileName = q.ArticleFileName(iArticleID);

            string sPathDefault = "https://MGMQuotation.MGMTransformer.com//MGMQuotation//Did-You-Know//";
            string sPathFull = sPathDefault + sFileName;


            // Open PDF in another browser.
            ResponseHelper.Redirect(sPathFull, "_blank", "");
        }

        protected void gvArticles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvArticles.PageIndex = e.NewPageIndex;

            LoadGrid();
        }

    }
}