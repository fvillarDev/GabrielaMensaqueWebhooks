using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GabrielaMensaqueWebhooks
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTestPay_OnClick(object sender, EventArgs e)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString["cmd"] = "_xclick";
            queryString["business"] = "svocos2015-facilitator@gmail.com";
            //queryString["upload"] = "1";

            queryString["item_name"] = "My Cart Item 1";
            queryString["quantity"] = "1";
            queryString["amount"] = "10.00";
            queryString["item_number"] = "GMV00001";
            queryString["button_subtype"] = "services";
            queryString["bn"] = "PP-BuyNowBF:btn_buynowCC_LG.gif:NonHosted";
            queryString["lc"] = "AR";
            queryString["rm"] = "2";
            queryString["no_note"] = "1";
            queryString["no_shipping"] = "1";

            //queryString["shopping_url"] = "http://xpto.com/Client/Checkout";
            queryString["return"] = "http://localhost/MyMasterPage/ConfirmPayment.aspx";
            queryString["notify_url"] = "http://gabrielamensaquevideos.apphb.com/api/IPNPayPal/Receive";

            Response.Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?" + queryString.ToString());
        }
    }
}