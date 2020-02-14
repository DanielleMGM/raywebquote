using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Runtime.InteropServices;


public sealed class DataValidation
{
    #region Fields

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the Cases class.
    /// </summary>
    public DataValidation()
    {
    }
    #endregion

    #region Properties
    #endregion

    #region Methods

    /// <summary>
    /// Validates date.
    /// Returns blank if valid, else error string.
    /// </summary>
    /// <param name="sDate"></param>
    /// <returns></returns>
    public string DateValid(string sDate)
    {
        if (sDate == "")
            return "";

        try
        {
            DateTime dt = Convert.ToDateTime(sDate);

            DateTime dtBegin = Convert.ToDateTime("01/01/2000");
            DateTime dtEnd = Convert.ToDateTime("01/01/2100");

            if (dt < dtBegin || dt > dtEnd)
                return "* Date out of range.";
        }
        catch
        {
            return "* Invalid date.";
        }
        return "";
    }

    /// <summary>
    /// Expect inputs to be valid, not null, and within a reasonable range.
    /// This procedure ensures From less than To, if both exist.
    /// </summary>
    /// <param name="sDateFrom"></param>
    /// <param name="sDateTo"></param>
    /// <returns></returns>
    public string DateFromToValid(DateTime dtFrom, DateTime dtTo)
    {
        if (dtFrom > dtTo)
            return "* From > To Date.";

        return "";
    }


    /// <summary>
    /// Returns properly formatted date, given a valid date string.
    /// </summary>
    /// <param name="sDate"></param>
    /// <returns></returns>
    public string DateFormat(string sDate)
    {
        try
        {
            DateTime dt = Convert.ToDateTime(sDate);
            return dt.ToShortDateString();
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// Returns a Macola date as an integer:  YYYYMMDD
    /// </summary>
    /// <param name="sDate"></param>
    /// <returns></returns>
    public int DateMacola(DateTime dt)
    {
        try
        {
            int i;
            string sDate = dt.ToString("yyyyMMdd");
            int.TryParse(sDate, out i);

            return i;
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    /// Outputs sValue to Output window.
    /// </summary>
    /// <param name="sValue"></param>
    public void Debug(string sValue)
    {
        System.Diagnostics.Debug.WriteLine(sValue);
    }

    /// <summary>
    /// Validates email.
    /// Returns blank if valid, else error string.
    /// </summary>
    public string EmailValid(string Email)
    {
        // Min example:  a@b.cd
        // i = 2
        // j = 4 Position of period relative to @.
        // l = 6

        // Allow empty email without issue.
        if (Email == null || Email == "")
            return "";

        if (Email.IndexOf("<") != -1 || Email.IndexOf(">") != -1)
            return "Brackets not allowed in email.";

        int i = Email.IndexOf("@");
        if (i == -1)
            return "No @ in email.";

        if (i == 0)
            return "@ at start of email.";

        int j = Email.IndexOf(".", i);
        if (j == -1)
            return "No . in email.";

        if (j == 0)
            return ". at start of email.";

         if (j - i == 1)
            return "Nothing between @ and .";

        int l = Email.Length;
        if (i > l - 3)
            return "@ near end of email.";

        if (j > l - 3)
            return ". near end of email.";

        if (l < 6)
            return "Email too short.";

        // Okay.
        return "";
    }

    /// <summary>
    /// Validates a Quantity input.
    /// Returns empty string if valid, a message if not.
    /// </summary>
    /// <param name="sQty"></param>
    /// <returns></returns>
    public string QuantityValid(string sQty, int iMax = 999)
    {
        sQty = sQty.Trim();
        if (String.IsNullOrEmpty(sQty))
        {
            return "";
        }

        double Num;
        bool isNum = double.TryParse(sQty, out Num);

        if (!isNum)
        {
            return "Not a numeric value.";
        }
        Int16 iNum;
        bool isInt = Int16.TryParse(sQty, out iNum);
        if (!isInt)
        {
            return "Not a whole number.";
        }
        
        Int16 i = Convert.ToInt16(sQty);

        if (i < 0)
        {
            return "Expect >= 0.";
        }
        if (i > iMax)
        {
            return "Expect <= " + iMax.ToString() + ".";
        }
        return "";
    }

    /// <summary>
    /// Validates a Price input.
    /// Returns empty string if valid, a message if not.
    /// </summary>
    /// <param name="sQty"></param>
    /// <returns></returns>
    public string PriceValid(string sPrice)
    {
        sPrice = sPrice.Trim();
        if (String.IsNullOrEmpty(sPrice))
        {
            return "";
        }

        double Num;
        bool isNum = double.TryParse(sPrice, out Num);

        if (!isNum)
        {
            return "Not a numeric value.";
        }
        if (Num < 0)
        {
            return "Not a positive number.";
        }

         if (Num > 1000000)
        {
            return "Exceeds $1,000,000.";
        }
        return "";
    }


    /// <summary>
    /// Verifies quote number.
    /// Optional WQ preface.  Optional .99 extension.
    /// Number between 100 and 99,999.   WQ99999.99
    /// </summary>
    /// <param name="sQuoteNo"></param>
    /// <returns></returns>
    public string QuoteNoValid(string sQuoteNo)
    {
        sQuoteNo = sQuoteNo.Trim().ToUpper();
        if (String.IsNullOrEmpty(sQuoteNo))
        {
            return "";
        }

        // Trim away left-hand WQ if any.
        if (sQuoteNo.Substring(0, 2) == "WQ")
            sQuoteNo = sQuoteNo.Substring(2, sQuoteNo.Length - 2);

        double Num;
        bool isNum = double.TryParse(sQuoteNo, out Num);

        if (!isNum)
        {
            return "Try just numbers. e.g.: 9999.";
        }
        if (Num > 99999)
        {
            return "Expect number <= 99,999.";
        }
        if (Num < 100)
        {
            return "Expect number >= 100.";
        }
        return "";
    }


    /// <summary>
    /// Return "" if no issues with User Name.
    /// </summary>
    /// <param name="sUserName"></param>
    /// <returns></returns>
    public string UserNameValid(string sUserName)
    {
        string sInvalid = "";
        sUserName = sUserName.Trim();

        if (sUserName.Length < 3)
        {
            sInvalid = "Too short.";
        }
        else if (sUserName.Length > 20)
        {
            sInvalid = "Too long.";
        }
        else if (sUserName.IndexOf(" ") != -1)
        {
            sInvalid = "No spaces allowed.";
        }

        return sInvalid;
    }

    /// <summary>
    /// Return "" if no issues with User Initials.
    /// </summary>
    /// <param name="sUserName"></param>
    /// <returns></returns>
    public string InitialsValid(string sInitials)
    {
        string sInvalid = "";
        sInitials = sInitials.Trim().ToUpper();

        if (!Regex.IsMatch(sInitials, @"^[a-zA-Z]+$"))
        {
            sInvalid = "Letters only.";
        }

        return sInvalid;
    }

    /// <summary>
    /// Return "" if no issues with Password.
    /// </summary>
    /// <param name="sUserName"></param>
    /// <returns></returns>
    public string PasswordValid(string sPassword)
    {
        string sInvalid = "";
        sPassword = sPassword.Trim();

        if (sPassword.Length < 5)
        {
            sInvalid = "Too short.";
        }
        else if (sPassword.Length > 20)
        {
            sInvalid = "Too long.";
        }
        else if (sPassword.IndexOf(" ") != -1)
        {
            sInvalid = "No spaces allowed.";
        }

        return sInvalid;
    }

    /// <summary>
    /// Validates IP address.
    /// Returns true if valid, false if not.
    /// </summary>
    public bool IPAddressValid(string IPAddress)
    {
        // Min example:  1.2.3.4
        // Max example:  111.222.333.444

        // Allow empty email without issue.
        if (IPAddress == null || IPAddress == "")
            return true;

        string sIP = IPAddress;
        int i = 0;
        int posn = 0;
        posn = sIP.IndexOf(".");        // zero based.

        // Expect period in IPAddress.
        if (posn == -1)
            return false;

        try
        {
            // Expect four sections of numbers <= 999,
            // separated by periods.

            i = Convert.ToInt32(sIP.Substring(0, posn));

            if (i > 999) return false;

            // Trims out first section, which passed inspection.
            // Moves on to the next section.
            // Example:  1.2.3.4
            // posn + 1 = 1 + 1 = 2
            // Length - posn + 1 = 7 - 1 - 1 = 5
            // substring(3, 5)
            sIP = sIP.Substring(posn + 1, sIP.Length - posn - 1);

            // Section 2
            posn = sIP.IndexOf(".");
            i = Convert.ToInt32(sIP.Substring(0, posn));
            if (i > 999) return false;

            sIP = sIP.Substring(posn + 1, sIP.Length - posn - 1);

            // Section 3
            posn = sIP.IndexOf(".");
            i = Convert.ToInt32(sIP.Substring(0, posn));
            if (i > 999) return false;

            sIP = sIP.Substring(posn + 1, sIP.Length - posn - 1);

            // Section 4
            i = Convert.ToInt32(sIP.Substring(0, sIP.Length));
            if (i > 999) return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("IP Address invalid with error: " + ex);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Formats phone number.
    /// Returns formatted phone number if valid, else blank.
    /// </summary>
    public string PhoneFmt(string Phone)
    {
        // Example:
        // (999) 999-9999 x99999  (length=21)
        int len = Phone.Length;

        if (len < 10)       // Need 10 numbers.
            return "";

        bool isNum = false;
        int n;
        string sPhone = "";
        string sChar = " ";

        // Strip numbers out of Phone input string,
        // put them in sPhone output string.
        for (int i = 0; i < len; i++)
        {
            sChar = Phone.Substring(i, 1);

            isNum = int.TryParse(sChar, out n);
            if (isNum)
            {
                sPhone += n.ToString();
            }
        }

        len = sPhone.Length;
        if (len < 10 || len > 15)
            return "";

        string sPhoneOut = "";

        sPhoneOut = "(" + sPhone.Substring(0, 3) + ") " + sPhone.Substring(3, 3) + "-" + sPhone.Substring(6, 4);

        if (sPhone.Length > 10)
        {
            sPhoneOut += " x" + sPhone.Substring(10, len - 10);
        }

        // Okay.
        return sPhoneOut;
    }

    /// <summary>
    /// Returns an integer from a string, if possible, right justified to the NoOfColumns specified.
    /// Example:  "1234", 7 returns "  1,234".
    /// </summary>
    public string NumberFormat(string s, int NoOfColumns, int NoOfPlaces, bool AllowNegatives = false)
    {
        try
        {
            Single sngNo = Convert.ToSingle(s);

            if (sngNo < 0 && AllowNegatives == false)
                return "";

            string fmt;
            switch (NoOfPlaces)
            {
                case 0:
                    fmt = "0,0";
                    break;
                case 1:
                    fmt = "0,0.0";
                    break;
                case 2:
                    fmt = "0,0.00";
                    break;
                default:
                    fmt = "0,0";
                    break;
            }

            string result = sngNo.ToString(fmt, CultureInfo.InvariantCulture);

            // Trim away leading zero.  Not sure why this occurs.
            if (result.Substring(0, 1) == "0")
                result = result.Substring(1);

            int len = result.Length;

            if (len > NoOfColumns && result.Substring(0,1) != "-")
                return "";

            for (int i = len; i < NoOfColumns; i++)
                result = " " + result;

            return result;
        }
        catch
        {
        }

        return "";
    }

    /// <summary>
    /// Return 3 character maximum Rep Initials.
    /// </summary>
    /// <param name="sInitials"></param>
    /// <returns></returns>
    public string InitialsFormat(string sInitials)
    {
        string sInits = "";

        if (sInitials != null)
        {
            sInits = sInitials.ToUpper();
        }

        return sInits;
    }

    /// <summary>
    /// Called by incrementers and decrementers to change the integer quantity in a textbox.
    /// </summary>
    /// <param name="sQuantity"></param>
    /// <param name="bIncrease"></param>
    public string ChangeTextQuantity(string sQuantity, bool bIncrease, int iMax)
    {
        if (sQuantity == "")
            sQuantity = "0";

        int iQty = Convert.ToInt32(sQuantity);

        if (bIncrease)
        {
            if (iQty < iMax)
                iQty++;
        }
        else
        {
            if (iQty > 0)
                iQty--;
        }

        return iQty.ToString();
    }

    /// <summary>
    /// Prepend enough spaces to fill a number out the the total length specified, less a decimal.
    /// </summary>
    /// <param name="sValue"></param>
    /// <param name="iTotalLength"></param>
    /// <returns></returns>
    public string PrependSpaces(string sValue, int iTotalLength)
    {
        if (sValue == null || sValue == "")
            return "";

        sValue = sValue.Trim();

        // Example:  112.5, length 4, should have one space prepended to make it length 4.
        // iPosn = 3
        // iLen = 3
        // iTotalLength = 5
        // iTotalLength - iLen = 5 - 3 = 2

        int iLen = 0;
        int iPosn = sValue.IndexOf(".");
        if (iPosn > 1)
            iLen = iPosn;
        else
            iLen = sValue.Length;

        for (int i = 1; i <= iTotalLength - iLen; i++)
        {
            sValue = " " + sValue;
        }
        return sValue;
    }

    /// <summary>
    /// Return a short date formatted string for various dates, such as previous month end.
    /// </summary>
    /// <param name="sType"></param>
    /// <returns></returns>
    public string DefaultDateString(string sType)
    {
        DateTime dtNow = DateTime.Now;
        DateTime dtMonthBegin = Convert.ToDateTime(dtNow.Month.ToString() + "/1/" + dtNow.Year.ToString());
        DateTime dtPrevMonthEnd = dtMonthBegin.AddDays(-1);
        DateTime dtPrevMonthBegin = Convert.ToDateTime(dtPrevMonthEnd.Month.ToString() + "/1/" + dtPrevMonthEnd.Year.ToString());
        DateTime dtPrevMonday = dtNow.AddDays(-(int)dtNow.DayOfWeek - 6);
        DateTime dtPrevFriday = dtNow.AddDays(-(int)dtNow.DayOfWeek - 2);

        switch (sType)
        {
            case "PrevMonthBegin":
                return dtPrevMonthBegin.ToShortDateString();
            case "PrevMonthEnd":
                return dtPrevMonthEnd.ToShortDateString();
            case "PrevMonday":
                return dtPrevMonday.ToShortDateString();
            case "PrevFriday":
                return dtPrevFriday.ToShortDateString();
        }
        return "";
    }

    public DataTable Months()
    {
        string sCon = WebConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString;
        SqlConnection con = new SqlConnection(sCon);

        SqlCommand cmd = new SqlCommand("SELECT convert(varchar(10), MonthBegin, 1) as MonthName, MonthBegin as MonthValue FROM Months ORDER BY MonthBegin", con);
        cmd.CommandType = CommandType.Text;

        try
        {
            con.Open();

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);

                return ds.Tables[0];
            }
        }
        catch (SqlException ex)
        {
            throw new ApplicationException(string.Format("Error ({0}): {1}", ex.Number, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            throw new ApplicationException(string.Format("Error: {0}", ex.Message));
        }
        catch (Exception ex)
        {
            // You might want to pass these errors
            // back out to the User.
            throw new ApplicationException(string.Format("Error: {0}", ex.Message));
        }
        finally
        {
            con.Close();
        }
    }

    /// <summary>
    /// Format AgentNo with leading zero if < 100, else as entire number.
    /// </summary>
    /// <param name="sAgentNo"></param>
    /// <returns></returns>
    public string AgentNo(string sAgentNo)
    {
        int i;
        sAgentNo = string.IsNullOrEmpty(sAgentNo) ? "" : sAgentNo;
        int.TryParse(sAgentNo, out i);

        if (i < 100)
        {
            sAgentNo = string.Format("{0:00}", i);
        }
        else
        {
            sAgentNo = string.Format("{0:000}", i);
        }

        return sAgentNo;
    }

    /// <summary>
    /// Strip any querystring off the end of a URL.
    /// </summary>
    /// <param name="sURL"></param>
    /// <returns></returns>
    public string BaseUrl(string sURL)
    {
        if (sURL.IndexOf("?") > 1)
        {
            return sURL.Substring(0, sURL.IndexOf("?"));
        }
        return sURL;
    }


    /// <summary>
    /// This set of utilities gets values from textboxes or drop downs.
    /// </summary>
    /// <param name="sText"></param>
    /// <returns></returns>
    public Boolean BooleanFromText(string sText)
    {
        if (sText == null || sText == "")
            return false;
        else
            return Convert.ToBoolean(sText);
    }
    public decimal DecimalFromText(string sText)
    {
        if (sText == null || sText == "")
            return 0;
        else
        {
            decimal d = 0;
            bool result = decimal.TryParse(sText, out d);
            if (result == true)
                return d;
            else
                return 0;
        }
    }
    public Int16 Int16FromText(string sText)
    {
        if (sText == null || sText == "")
            return 0;
        else
        {
            Int16 i = 0;
            bool result = Int16.TryParse(sText, out i);
            if (result == true)
                return i;
            else
                return 0;
        }
    }
    public Int32 Int32FromText(string sText)
    {
        if (sText == null || sText == "")
            return 0;
        else
        {
            Int32 i = 0;
            bool result = Int32.TryParse(sText, out i);
            if (result == true)
                return i;
            else
                return 0;
        }
    }
    public DateTime DateTimeFromText(string sText)
    {
        if (sText == null || sText == "")
            return Convert.ToDateTime("01/01/1900");
        else
        {
            DateTime dt;
            bool result = DateTime.TryParse(sText, out dt);
            if (result == true)
                return dt;
            else
                return Convert.ToDateTime("01/01/1900");
        }
    }
    public string StringFromText(string sText)
    {
        if (sText == null)
            return sText;
        return sText;
    }

    public string KVAEntry(string sPhase, string sText, bool bInternal)
    {
        if (sText == null)
            return sText;
        else
        {
            decimal d = 0;
            bool result = decimal.TryParse(sText, out d);
            if (result == true)
            {
                if (d == 0) return "";

                decimal dMin = 9;

                // Allow 7 if Internal, 9 otherwise.
                if (bInternal == true) dMin = 7;

                if (d < dMin)                     // Convert numbers < dMin to dMin.
                    return dMin.ToString();

                if (d > 1000)
                    return "1000";              // Convert numbers > 1000 to 1000.

                return d.ToString();
            }
            else
                return "";
        }
    }

    /// <summary>
    /// Returns Text (i.e. txtBox.Text) if available, else DropDown (i.e. ddlList.SelectedValue).
    /// </summary>
    /// <param name="sText"></param>
    /// <param name="sSelectedValue"></param>
    /// <returns></returns>
    public string TextOrDropDown(string sText, string sSelectedValue)
    {
        string sSelVal = (String.IsNullOrEmpty(sSelectedValue) == true) ? "" : sSelectedValue.Trim();
        string sTextVal = (String.IsNullOrEmpty(sText) == true) ? "" : sText.Trim();

        // Prefer text if both exist.
        string sReturn = (sTextVal == "" )? sSelVal : sTextVal;

        return sReturn;
    }

    public static string RemoveSpecialCharacters(string str)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Prepare string to be encapsulated by either quotes or apostrophes.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string RemoveQuotesAndApostrophes(string str)
    {
        string sRaw = String.IsNullOrEmpty(str) == true ? "": str;
        
        if (sRaw == "") return sRaw;

        string sNoQuotes = sRaw.Replace("\"", "");
        string sNoApostrophes = sNoQuotes.Replace("'", "");

        return sNoApostrophes;
    }

    /// <summary>
    /// Switch from Y to ZZ in Voltage when switching to HarmonicMitigating.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string VoltageReplace(string sVoltage, bool bHarmonic)
    {
        string sVoltageNew = String.IsNullOrEmpty(sVoltage) == true ? "" : sVoltage;
        int i = sVoltageNew.IndexOf("ZZ");

        if (sVoltageNew == "") return "";

        if (bHarmonic)
        {
            if (i <= 0)
            {
                sVoltageNew = sVoltageNew.Trim() + "ZZ";
            }
        }
        else
        {
            sVoltageNew = sVoltageNew.Replace("ZZ", "");
        }

        return sVoltageNew;
    }

    /// <summary>
    /// Convert a given Delta voltage to its Wye counterpart.
    /// Example:  208 D comes in.  208Y/120 goes out.
    /// </summary>
    /// <param name="sVoltage"></param>
    /// <returns></returns>
    public string VoltageConvertWye(string sVoltage)
    {
        string sVoltageNum = "";
        string sVoltageOut = "";

        sVoltageNum = Regex.Match(sVoltage, @"\d+").Value;
        Double dblVoltageIn = Double.Parse(sVoltageNum);
        Double dblVoltageOut = dblVoltageIn / 1.732;
        Int32 iVoltageIn = Convert.ToInt32(dblVoltageIn);
        Int32 iVoltageOut = Convert.ToInt32(dblVoltageOut);

        sVoltageOut = iVoltageIn.ToString() + "Y/" + iVoltageOut.ToString();

        return sVoltageOut;
    }

    /// <summary>
    /// Environment:  DEV, PROD, QA.
    /// </summary>
    /// <returns></returns>
    public string Environment()
    {
        string sPath = CurrentDir(true);

        string sDirDev = "https://MGMQuotation.MGMTransformer.com//MGMQuotation//MGMQuotationDev//";
        string sDirProd = "https://MGMQuotation.MGMTransformer.com//";
        string sDirQA = "https://MGMQuotation.MGMTransformer.com//MGMQuotation//MGMQuotationQA//";

        
        if (sPath == sDirDev) return "DEV";
        if (sPath == sDirProd) return "PROD";
        if (sPath == sDirQA) return "QA";

        return "";
    }

    // Returns the current path, expected to be either C:\\MGMQuotation\\ (production) or C:\\MGMQuotationQA\\ (QA).
    public string CurrentDir(bool bVirtual)
    {
        // This only works when run in IIS, not when run locally.
        //string sPath = Request.ApplicationPath;
        //string sPath = HttpRequest.ApplicationPath;

        string sPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        sPath = sPath.Replace("file:\\", "");
        sPath = sPath.Replace("bin", "");

        string sDirDev = "https://MGMQuotation.MGMTransformer.com//MGMQuotation//MGMQuotationDev//";
        string sDirProd = "https://MGMQuotation.MGMTransformer.com//";
        string sDirQA = "https://MGMQuotation.MGMTransformer.com//MGMQuotation//MGMQuotationQA//";
 
        if (bVirtual == true)
        {
            switch (sPath)
            {
                case "C:\\MGMQuotationDev\\":
                    return sDirDev;
                case "C:\\MGMQuotationQA\\":
                    return sDirQA;
                default:
                    return sDirProd;
            }
        }
        else
        {
            return sPath;
        }
    }

    /// <summary>
    /// For hand-entered Voltages (Admin only.)
    /// </summary>
    /// <param name="sVoltage"></param>
    /// <returns></returns>
    public string ValidVoltage(string sVoltage)
    {
        string sChar = "";
        string sErrorMsg = "";


        // RegEx to do this seemed complicated, and it's only used by Admin anyway...
        for (int i = 1; i < sVoltage.Length; i++)
        {
            sChar = sVoltage.Substring(i, 1);

            switch (sChar)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "C":
                case "D":
                case "T":
                case "X":
                case "Y":
                case "Z":
                case "/":
                case " ":
                    break;
                default:
                    sErrorMsg = "<br />Expect numbers, CT, D, X, Y, Z or / only.";
                    break;
            }

            int iVoltage = VoltageNum(sVoltage);

            if (iVoltage > 1200)
            {
                sErrorMsg = "<br />Contact MGM for voltages > 1200.";
            }
        }
 
        return sErrorMsg;
    }

    /// <summary>
    /// Return just the voltage.
    /// Example:  480Y/277 returns 480.
    /// </summary>
    /// <param name="sVoltage"></param>
    /// <returns></returns>
    public int VoltageNum(string sVoltage)
    {
        bool bEnd = false;
        int iLen = 0;
        int iVoltage = 0;
        string sChar = "";
        string sVoltageOut = "";

        sVoltage = string.IsNullOrEmpty(sVoltage) == true ? "" : sVoltage;
        iLen = sVoltage.Length;

        for (int i = 0; i < iLen; i++)
        {
            sChar = sVoltage.Substring(i, 1);
            switch (sChar)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    sVoltageOut = sVoltageOut + sChar;
                    break;
                default:
                    bEnd = true;
                    break;
            }
            // Stop the loop if we're not on a number.
            if (bEnd == true)
            {
                break;
            }
        }

        int.TryParse(sVoltageOut, out iVoltage);

        return iVoltage;
    }


    /// <summary>
    /// Safely adds an item to a DropDownList.
    /// </summary>
    /// <param name="objDropDownList"></param>
    /// <param name="sValue"></param>
    /// <returns></returns>
    public bool DropdownListAdd(DropDownList objDropDownList, string sValue)
    {
        if (objDropDownList.Items.Count > 0)
        {
            try
            {
                if (objDropDownList.Items.FindByValue(sValue) != null)
                {
                    objDropDownList.SelectedValue = sValue;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }
        }
        return false;
    }

    /// <summary>
    /// Safely adds an item to a RadioButtonList.
    /// </summary>
    /// <param name="objDropDownList"></param>
    /// <param name="sValue"></param>
    /// <returns></returns>
    public bool RadioButtonListAdd(RadioButtonList objRadioButtonList, string sValue)
    {
        if (objRadioButtonList.Items.Count > 0)
        {
            try
            {
                if (objRadioButtonList.Items.FindByValue(sValue) != null)
                {
                    objRadioButtonList.SelectedValue = sValue;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }
        }
        return false;
    }

    /// <summary>
    /// Returns True if the quote number has the format "WA....", since the "A" means "Admin" created it.
    /// "Admin" is anyone with Admin privileges.
    /// </summary>
    /// <param name="sQuoteNo"></param>
    /// <returns></returns>
    public bool IsAdminQuote(string sQuoteNo)
    {
        sQuoteNo = String.IsNullOrEmpty(sQuoteNo) == true ? "" : sQuoteNo.Trim();

        if (sQuoteNo.Length < 3)
            return false;

        if (sQuoteNo.Substring(1, 1) == "A")        // 0-based.
            return true;

        return false;
    }

    /// <summary>
    /// Return a properly formatted KVA string for text entry.
    /// </summary>
    /// <param name="sKVA"></param>
    /// <returns></returns>
    public string KVAFormat(string sKVA)
    {
        string sReturn = "";

        if (String.IsNullOrEmpty(sKVA) == false)
        {
            // Get just the numerical part of the KVA.  i.e. If 480 D was entered, get 480.
            decimal dKVA = 0;
            decimal.TryParse(sKVA, out dKVA);

            // Format to one fixed decimal place.
            sReturn = dKVA.ToString("F1");

            // Look at what is in that decimal place.
            string sDecimal = sReturn.Substring(sReturn.Length - 1, 1);

            // If it's not '5', truncate it.
            if (sDecimal != "5")
            {
                sReturn = sReturn.Substring(0, sReturn.Length - 2);
            }
         }
        return sReturn;
    }

    /// <summary>
    /// Returns true count in grid, given that the first row might be a dummy,
    /// just to show the headers.  If so, it won't be visible.
    /// </summary>
    /// <param name="gv"></param>
    /// <returns></returns>
    public int GridCount(GridView gv)
    {
        if (gv.Rows.Count == 1)
        {
            if (gv.Rows[0].Visible == false)
                return 0;
            else
                return 1;
        }
        else
        {
            return gv.Rows.Count;
        }
    }

    /// <summary>
    /// Returns a substitute table with a hidden dummy record
    /// if the existing table has no data.  Otherwise, returns regular data.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public void GridTable(GridView gv, DataTable dtGV)
    {
        if (dtGV == null)
        {
            gv.Visible = false;
        }
        else if (dtGV.Rows.Count == 0)
        {
            //DataTable dt = null;

            //dt = dtGV.Clone();
 
            //dt.Clear();
            //dt.Rows.Add(dt.NewRow()); 
      
            //gv.DataSource = dt;
            //gv.DataBind();

            //gv.Rows[0].Visible = false;
            //gv.Rows[0].Controls.Clear();

            gv.Visible = false;
        }
        else
        {
            gv.Visible = true;
            gv.DataSource = dtGV;
            gv.DataBind();

            if (gv.Rows.Count > 0)
                gv.Rows[0].Visible = true;
        }

    }
    /// <summary>
    /// Replace System Text, of the form >>xxxxx<<.
    /// OBSOLETE, but keeping as an example of how to do this.
    /// </summary>
    /// <param name="sExists"></param>
    /// <param name="sNew"></param>
    /// <returns></returns>
    public string TextReplaceSystem(string sExists, string sNew)
    {
        const string csBegin = ">>";
        const string csEnd = "<<";
        sExists = String.IsNullOrEmpty(sExists) ? "" : sExists;
        sNew = String.IsNullOrEmpty(sNew) ? "" : sNew;

        if (sExists.Contains(csBegin) && sExists.Contains(csEnd))
        {
            int iBegin = sExists.IndexOf(csBegin);
            int iEnd = sExists.IndexOf(csEnd);
            int len = sExists.Length;

            // Example:     a>>b<<c
            //              0123456
            // iBegin = 1
            // iEnd   = 4
            // len    = 6
            // sExistsBegin = starts at 0, for 1.  Returns:  a
            // sExistsEnd   = starts at 6, for 1.  Returns:  c
            string sExistsBegin = sExists.Substring(0, iBegin);
            string sExistsEnd = sExists.Substring(iBegin + 2, len - (iEnd + 1));
            string sReturn = sExistsBegin + sNew + sExistsEnd;
            return sReturn.Trim();
        }
        else
        {
            string sResult = sExists + " " + sNew;
            sResult.Trim();

            return sResult;
        }
    }

    /// <summary>
    /// Finds the best match for the PDF in the file system.
    /// </summary>
    /// <param name="sPDFUrl">This is the recorded PDFUrl, or PDFUrlNoPrice.</param>
    /// <param name="iQuoteID"></param>
    /// <param name="iQuoteVersion"></param>
    /// <param name="bNoPrice"></param>
    /// <returns></returns>
    public string PDFExists(string sPDFUrl, int iQuoteNo, int iQuoteVersion, bool bNoPrice, bool bSubmittal)
    {
        string sBestMatch = "";
        
        // Remove nulls, spaces from sPDFUrl.
        sPDFUrl = string.IsNullOrEmpty(sPDFUrl.Trim()) == true ? "" : sPDFUrl.Trim();

        // Creates dummy filename in the form WQ12345_XXX_v01_01.PDF or WQ12345_XXX_NoPrice_v01_01.PDF.
        if (sPDFUrl == "")
        {
            sPDFUrl = "WQ" + iQuoteNo.ToString() + "_XXX_";
            if (bSubmittal == true)
                sPDFUrl = sPDFUrl + "Submittal_";
            else if (bNoPrice == true)
                sPDFUrl = sPDFUrl + "NoPrice_";
            
            sPDFUrl = sPDFUrl + "v" + iQuoteVersion.ToString("00") + "_01.PDF";
        }
 
        // Matches any pattern beginning with W, having another character, then a Quote ID, and other things.
        // The pattern:  W?12345_XXXXXX...
        string sPattern = "W?" + iQuoteNo.ToString("D5") + "_*";

        // Use GETFILES to pattern match for the best fit for this file.
        string folderPath = "";
        if (Convert.ToBoolean(WebConfigurationManager.AppSettings["LocalMachine"]) == true)
            folderPath = WebConfigurationManager.AppSettings["LocalMachinePath"];
        else
            folderPath = @"\MGMQuotation\pdfs\";
         
        
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        FileInfo[] fileExact = dir.GetFiles(sPDFUrl, SearchOption.TopDirectoryOnly);
        FileInfo[] files = dir.GetFiles(sPattern, SearchOption.TopDirectoryOnly);

        int iInstance = 0;
        int iInstanceLatest = 0;
        int iVersion = 0;
        string sVersion = "";
        string s = "";

        foreach (var item in fileExact)
        {
            s = item.ToString();

            // Check best match.
            if (s == sPDFUrl)
            {
                sBestMatch = s;
                return sBestMatch;
            }
        }

        // If we don't have an exact match, then look for the best match.
        foreach (var item in files)
        {
            s = item.ToString();

            // Check best match.
            if (sBestMatch == "")
            {
                sBestMatch = s;
            }

            iInstance = 0;
            iVersion = 0;
            sVersion = "";
            if (item.Length > 10)
            {
                // _v02_03.PDF
                sVersion = s.Substring(s.Length - 9);
                // 02_03.PDF
            }

            int.TryParse(sVersion.Substring(0, 2), out iVersion);
            int.TryParse(sVersion.Substring(3, 2), out iInstance);

            if (iVersion == iQuoteVersion)
            {
                if (iInstance > iInstanceLatest)
                {
                    iInstanceLatest = iInstance;
                    sBestMatch = s;
                }
            }
         }

        return sBestMatch;
    }

     /// <summary>
    /// Update all URLs that have no PDFUrl with the best match available.
    /// </summary>
    public void UpdateAllURLs()
    {
        string sSQL = "SELECT QuoteID, QuoteNo, QuoteNoVer FROM Quote WHERE isnull(PDFUrl,'')='' ORDER BY QuoteID";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mgmdb"].ToString());
        SqlCommand cmd = new SqlCommand(sSQL, con);
        SqlCommand cmdUpd = new SqlCommand(sSQL, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();

        int iQuoteID = 0;
        int iQuoteNo = 0;
        int iQuoteNoVer = 0;
        string sPDFBest = "";
       
        using (con)
        {
            try
            {
                con.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    iQuoteID = Convert.ToInt32(dr["QuoteID"]);
                    iQuoteNo = Convert.ToInt32(dr["QuoteNo"]);
                    iQuoteNoVer = Convert.ToInt32(dr["QuoteNoVer"]);

                    System.Diagnostics.Debug.WriteLine(iQuoteID.ToString() + " " + iQuoteNo.ToString() + " " + iQuoteNoVer.ToString() + " before");

                    sPDFBest = PDFExists("", iQuoteNo, iQuoteNoVer, false, false);

                    System.Diagnostics.Debug.WriteLine(iQuoteID.ToString() + " " + iQuoteNo.ToString() + " " + iQuoteNoVer.ToString() + " " + sPDFBest);

                    if (sPDFBest != "")
                    {
                        cmdUpd.CommandText = "UPDATE Quote SET PDFUrl='" + sPDFBest + "' WHERE QuoteID=" + iQuoteID;
                        cmdUpd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error:" + ex);
                return;
            }
        }
    }

    /// <summary>
    /// Returns True if sVoltage1 = sVoltage2, except for conversion from three phase to single phase.
    /// </summary>
    /// <param name="sVoltage1"></param>
    /// <param name="sVoltage2"></param>
    /// <returns></returns>
    public bool VoltageCompare(string sVoltage1, string sVoltage2)
    {
        int iLen1 = sVoltage1.Length;
        int iLen2 = sVoltage2.Length;
        int iYPos = 0;
        string sVoltageTrim = "";

        if (iLen1 == 0 || iLen2 == 0)
            return false;

        // Example:  480, 480 D
        // Len1 = 3
        // Len2 = 5
        // If left('480 D', 3) = '480' then True.

        // Example: 480 D, 480Y/277, 480Y/277ZZ
        // If "Y" found, strip just the number out, so it's the same as the example above.
        iYPos = sVoltage1.IndexOf("Y");
        if (iYPos > 0)
        {
            sVoltage1 = sVoltage1.Substring(0, iYPos);
        }
        // If "ZZ" found, strip just the number out, so it's the same as the example above.
        if (iYPos == -1)
        {
            iYPos = sVoltage1.IndexOf("ZZ");
            if (iYPos > 0)
            {
                sVoltage1 = sVoltage1.Substring(0, iYPos);
            }
        }

        // Voltage 2.
        iYPos = sVoltage2.IndexOf("Y");
        if (iYPos > 0)
        {
            sVoltage2 = sVoltage2.Substring(0, iYPos);
        }
        // If "ZZ" found, strip just the number out, so it's the same as the example above.
        if (iYPos == -1)
        {
            iYPos = sVoltage2.IndexOf("ZZ");
            if (iYPos > 0)
            {
                sVoltage2 = sVoltage2.Substring(0, iYPos);
            }
        }

        iLen1 = sVoltage1.Length;
        iLen2 = sVoltage2.Length;

        if (iLen1 > iLen2)
        {
            // Trim 1 to be the same length as 2.
            sVoltageTrim = sVoltage1.Substring(0, iLen2);
            return sVoltageTrim == sVoltage2;
        }
        else
        {
            // Trim 2 to be the same length as 1.
            sVoltageTrim = sVoltage2.Substring(0, iLen1);
            return sVoltageTrim == sVoltage1;
        }
    }


    #endregion

}

