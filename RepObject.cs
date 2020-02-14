using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Security;


public sealed class RepObject
{
    #region Fields

    private int _RepId;
    private string _Conn;
    private string _Email;
    private string _Full_Name;
    private int _Lead_Times;
    private DateTime _Last_activity;
    private int _MGMAgentNo;
    private string _Password;
    private string _Phone;
    private decimal _PriceMultiplier;
    private string _SecurityLevel;
    private string _UserName;

    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the Rep class.
    /// </summary>
    public RepObject()
    {
        _Conn = WebConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString;
    }
    #endregion

    #region Properties
    public int RepId
    {
        get { return _RepId; }
        set { _RepId = value; }
    }
    public string Email
    {
        get { return _Email; }
        set { _Email = value; }
    }
    public string Full_Name
    {
        get { return _Full_Name; }
        set { _Full_Name = value; }
    }
    public DateTime Last_activity
    {
        get { return _Last_activity; }
        set { _Last_activity = value; }
    }
    public int LeadTimeNoDays
    {
        get { return _Lead_Times; }
        set { _Lead_Times = value; }
    }
    public int MGMAgentNo
    {
        get { return _MGMAgentNo; }
        set { _MGMAgentNo = value; }
    }
    public string Password
    {
        get { return _Password; }
        set { _Password = value; }
    }
    public string Phone
    {
        get { return _Phone; }
        set { _Phone = value; }
    }
    public decimal PriceMultiplier
    {
        get { return _PriceMultiplier; }
        set { _PriceMultiplier = value; }
    }
    public string SecurityLevel
    {
        get { return _SecurityLevel; }
        set { _SecurityLevel = value; }
    }
    public string UserName
    {
        get { return _UserName; }
        set { _UserName = value; }
    }
    #endregion

    #region Methods

    /// <summary>
    /// Returns true if a RepID exists.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public bool RepExists(int RepID)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_Exists", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepID", SqlDbType.Int);

        cmd.Parameters["@RepID"].Value = RepID;

        try
        {
            con.Open();
            using (SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                DataSet _dataset = new DataSet();
                _sqlDataAdapter.Fill(_dataset);

                DataRow dr = _dataset.Tables[0].Rows[0];

                if (Convert.ToInt32(dr["RepExists"]) == 1)
                    return true;
                
                return false;
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

    /// Returns ID for a RepName.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public int RepNameExists(string Full_Name)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_SelectIDByName", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@Full_Name", SqlDbType.VarChar, 100);

        cmd.Parameters["@Full_Name"].Value = Full_Name;

        try
        {
            con.Open();
            using (SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                DataSet _dataset = new DataSet();
                _sqlDataAdapter.Fill(_dataset);

                DataRow dr = _dataset.Tables[0].Rows[0];

                return Convert.ToInt32(dr["RepID"]);
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
    /// Refreshes Product List for a given Rep. 
    /// </summary>
    public int RefreshProductList(int RepID, string SecurityLevel)
    {
        if (SecurityLevel == null || SecurityLevel == "")
            return 0;
        
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_UpdatePrices", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepID", SqlDbType.Int);
        cmd.Parameters.Add("@SecurityLevel", SqlDbType.VarChar, 20);

        cmd.Parameters["@RepID"].Value = RepID;
        cmd.Parameters["@SecurityLevel"].Value = SecurityLevel;

        try
        {
            con.Open();
            int _recordsAffected = cmd.ExecuteNonQuery();
            return _recordsAffected;
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
    /// Inserts a new record into Rep table.  Returns RepID.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public int Insert()
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_Insert", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@Full_Name", SqlDbType.VarChar, 100);
        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 200);
        cmd.Parameters.Add("@MGMAgentNo", SqlDbType.Int);
        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 21);
        cmd.Parameters.Add("@PriceMultiplier", SqlDbType.Money);
        cmd.Parameters.Add("@SecurityLevel", SqlDbType.VarChar, 20);
        cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 50);

        cmd.Parameters["@Full_Name"].Value = _Full_Name;
        cmd.Parameters["@Email"].Value = _Email;
        cmd.Parameters["@MGMAgentNo"].Value = _MGMAgentNo;
        cmd.Parameters["@Password"].Value = _Password;
        cmd.Parameters["@Phone"].Value = _Phone;
        cmd.Parameters["@PriceMultiplier"].Value = _PriceMultiplier;
        cmd.Parameters["@SecurityLevel"].Value = _SecurityLevel;
        cmd.Parameters["@Username"].Value = _UserName;

        try
        {
            con.Open();

            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                sda.Fill(ds);
                DataRow dr = ds.Tables[0].Rows[0];
                int RepID = Convert.ToInt32(dr["RepID"]);

                return RepID;
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
    /// Insert a new Login
    /// </summary>
    /// <returns></returns>
    public void InsertUpdateLogin(int iRepId, string sUserName, string sFullName, string sPassword,  
                                    string sEmail, string sPhone, string sPhoneType, string sVPN_IP)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Login_InsertUpdate", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@rep_id", SqlDbType.Int);
        cmd.Parameters.Add("@user_name", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@full_name", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@password", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@email", SqlDbType.VarChar, 100);
        cmd.Parameters.Add("@phone", SqlDbType.VarChar, 20);
        cmd.Parameters.Add("@phone_type", SqlDbType.Char, 10);
        cmd.Parameters.Add("@vpn_ip", SqlDbType.VarChar, 20);


        cmd.Parameters["@rep_id"].Value = iRepId;
        cmd.Parameters["@user_name"].Value = sUserName;
        cmd.Parameters["@full_name"].Value = sFullName;
        cmd.Parameters["@password"].Value = sPassword;
        cmd.Parameters["@email"].Value = sEmail;
        cmd.Parameters["@phone"].Value = sPhone;
        cmd.Parameters["@phone_type"].Value = sPhoneType;
        cmd.Parameters["@vpn_ip"].Value = sVPN_IP;

        try
        {
            con.Open();
            int iRecordsAffected = cmd.ExecuteNonQuery();
            return;
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
    /// Updates an existing record in Rep table.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public int Update()
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_Update", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepId", SqlDbType.Int);
        cmd.Parameters.Add("@Full_Name", SqlDbType.VarChar, 100);
        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100);
        cmd.Parameters.Add("@LeadTimeNoDays", SqlDbType.Int);
        cmd.Parameters.Add("@MGMAgentNo", SqlDbType.Int);
        cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@Phone", SqlDbType.VarChar, 21);
        cmd.Parameters.Add("@PriceMultiplier", SqlDbType.Money);
        cmd.Parameters.Add("@SecurityLevel", SqlDbType.VarChar, 20);

        cmd.Parameters["@RepId"].Value = _RepId;
        cmd.Parameters["@Full_Name"].Value = _Full_Name;
        cmd.Parameters["@Email"].Value = _Email;
        cmd.Parameters["@LeadTimeNoDays"].Value = _Lead_Times;
        cmd.Parameters["@MGMAgentNo"].Value = _MGMAgentNo;
        cmd.Parameters["@Password"].Value = _Password;
        cmd.Parameters["@Phone"].Value = _Phone;
        cmd.Parameters["@PriceMultiplier"].Value = _PriceMultiplier;
        cmd.Parameters["@SecurityLevel"].Value = _SecurityLevel;

        try
        {
            con.Open();
            int _recordsAffected = cmd.ExecuteNonQuery();
            return _recordsAffected;
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
    /// Updates an existing record in Logins table.
    /// Used in granting phone access.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public DataSet PhoneAccessData(string sUserName)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Login_PhoneData_New", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@user_name", sUserName);
 
        try
        {
            con.Open();
            using (SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                DataSet _dataset = new DataSet();
                _sqlDataAdapter.Fill(_dataset);

                return _dataset;
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
    /// Updates an existing record in Logins table.
    /// Used in requesting phone access.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public int PhoneAccessRequest(string sUserName, string sFullName, int iRepID, int iIsNew, string sPassword, 
                                    string sEmail, string sPhoneType, string sPhone)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Login_Update", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@user_name", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@full_name", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@rep_id", SqlDbType.Int);
        cmd.Parameters.Add("@is_new", SqlDbType.Bit);
        cmd.Parameters.Add("@password", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@email", SqlDbType.VarChar, 100);
        cmd.Parameters.Add("@phone_type", SqlDbType.VarChar, 10);
        cmd.Parameters.Add("@phone", SqlDbType.VarChar, 20);

        cmd.Parameters["@user_name"].Value = sUserName;
        cmd.Parameters["@full_name"].Value = sFullName;
        cmd.Parameters["@rep_id"].Value = iRepID;
        cmd.Parameters["@is_new"].Value = iIsNew;
        cmd.Parameters["@password"].Value = sPassword;
        cmd.Parameters["@email"].Value = sEmail;
        cmd.Parameters["@phone_type"].Value = sPhoneType;
        cmd.Parameters["@phone"].Value = sPhone;

        try
        {
            con.Open();
            int _recordsAffected = cmd.ExecuteNonQuery();
            return _recordsAffected;
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
    /// Updates an existing record in Logins table.
    /// Used in granting phone access.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public int PhoneAccessGrant(string sUserName, int iRepID)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Login_PhoneFinalize", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@user_name", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@rep_id", SqlDbType.Int);

        cmd.Parameters["@user_name"].Value = sUserName;
        cmd.Parameters["@rep_id"].Value = iRepID;

        try
        {
            con.Open();
            int _recordsAffected = cmd.ExecuteNonQuery();
            return _recordsAffected;
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
    /// Deletes an existing record from Rep table.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public int Delete(int RepID)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_Delete", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepId", SqlDbType.Int);
        cmd.Parameters["@RepId"].Value = RepID;

        try
        {
            con.Open();
            int _recordsAffected = cmd.ExecuteNonQuery();
            return _recordsAffected;
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
    /// Deletes an existing record from Login table.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public void DeleteLogin(string sUserName)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Login_Delete", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@user_name", SqlDbType.VarChar, 50);
        cmd.Parameters["@user_name"].Value = sUserName;

        try
        {
            con.Open();
            int iRecordsAffected = cmd.ExecuteNonQuery();
            return;
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
    /// Updates latest activity in both Login and Rep table.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public void LoginActivity(string sUserName)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Login_UpdateActivity", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@user_name", SqlDbType.VarChar, 50);
        cmd.Parameters["@user_name"].Value = sUserName;

        try
        {
            con.Open();
            int iRecordsAffected = cmd.ExecuteNonQuery();
            return;
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

    public DataSet Select(int RepId)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_Select", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepId", SqlDbType.Int);
        cmd.Parameters["@RepId"].Value = RepId;

        try
        {
            con.Open();

            using (SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                DataSet _dataset = new DataSet();
                _sqlDataAdapter.Fill(_dataset);

                return _dataset;
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


    public DataSet List()
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_List", con);
        cmd.CommandType = CommandType.StoredProcedure;

        try
        {
            con.Open();

            using (SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                DataSet _dataset = new DataSet();
                _sqlDataAdapter.Fill(_dataset);

                return _dataset;
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

    public DataTable GetName(int RepId)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Rep_Select", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepId", SqlDbType.Int);
        cmd.Parameters["@RepId"].Value = RepId;

        try
        {
            con.Open();

            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                sda.Fill(ds);

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
    /// Get the RepID for a givin Login UserName.
    /// </summary>
    /// <param name="sUserName"></param>
    /// <returns></returns>
    public DataTable GetRepID(string sUserName)
    {
        SqlConnection con = new SqlConnection(_Conn);

        string sCmd = "SELECT RepID FROM Logins WHERE UserName='" + sUserName + "'";
        SqlCommand cmd = new SqlCommand(sCmd, con);
        cmd.CommandType = CommandType.Text;
 
        try
        {
            con.Open();

            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                sda.Fill(ds);

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

    // Price Multipliers are +/- 20% of 1.00.
    public bool ValidPriceMultiplier(decimal? PriceMultiplier)
    {
        if (PriceMultiplier == null)
            return true;

        if ((float)PriceMultiplier < .8 || (float)PriceMultiplier > 1.2)
            return false;

        return true;
    }

    // Insert or Delete Rep IP Addresses.

    // Inserts new RepIP record, and possibly IP_Address record.
    public int RepIP_Insert(int RepID, string IPAddress, string IPDescription)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_RepIP_Insert", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepID", SqlDbType.Int);
        cmd.Parameters.Add("@IP_Address", SqlDbType.VarChar, 15);
        cmd.Parameters.Add("@IP_Description", SqlDbType.VarChar, 50);

        cmd.Parameters["@RepID"].Value = RepID;
        cmd.Parameters["@IP_Address"].Value = IPAddress;
        cmd.Parameters["@IP_Description"].Value = IPDescription;

        try
        {
            con.Open();
            int numrowsaffected = cmd.ExecuteNonQuery();
            return numrowsaffected;
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

    // Deletes RepIP record, and possibly IP_Address record.
    public int RepIP_Delete(int RepID, string IPAddress)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_RepIP_Delete", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepID", SqlDbType.Int);
        cmd.Parameters.Add("@IP_Address", SqlDbType.VarChar, 15);

        cmd.Parameters["@RepID"].Value = RepID;
        cmd.Parameters["@IP_Address"].Value = IPAddress;

        try
        {
            con.Open();
            int numrowsaffected = cmd.ExecuteNonQuery();
            return numrowsaffected;
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
    /// Presents a table of Rep Customers
    /// </summary>
    /// <param name="RepID"></param>
    /// <returns></returns>
    public DataTable Customers(int RepID, string sCustomerPart)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Customers_New", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@RepID", SqlDbType.Int);
        cmd.Parameters.Add("@customer_part", SqlDbType.VarChar, 50);

        cmd.Parameters["@RepID"].Value = RepID;
        cmd.Parameters["@customer_part"].Value = sCustomerPart;

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
        {
            con.Open();
            da.Fill(ds);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error in selecting customers:" + ex);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds.Tables[0];
    }

    /// <summary>
    /// Data source for Expedite Fee List.
    /// </summary>
    /// <returns></returns>
    public DataTable ExpediteFeesList(int iShipDays, bool bExact)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_ExpediteFees_List_20180906", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ship_days", iShipDays);
        cmd.Parameters.AddWithValue("@is_exact", bExact);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
        {
            con.Open();
            da.Fill(ds);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error in generating expedite fee list:" + ex);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds.Tables[0];
    }


    /// <summary>
    /// Presents a list of Rep Companies.
    /// </summary>
    /// <param name="RepID"></param>
    /// <returns></returns>
    public DataTable Companies(int RepID, string sUserName)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Company_List_New", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@rep_id", SqlDbType.Int);
        cmd.Parameters["@rep_id"].Value = RepID;
        cmd.Parameters.Add("@user_name", SqlDbType.VarChar);
        cmd.Parameters["@user_name"].Value = sUserName;

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
        {
            con.Open();
            da.Fill(ds);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error in selecting Companies:" + ex);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds.Tables[0];
    }

    /// <summary>
    /// Presents a list of Rep Distributors.
    /// If zero passed in as RepID, selects ALL Distributors.
    /// This option is used for the Admin function to display all logins.
    /// </summary>
    /// <param name="RepID"></param>
    /// <returns></returns>
    public DataTable Distributors(int RepID)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Distributor_List_20180611", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@rep_id", SqlDbType.Int);
        cmd.Parameters["@rep_id"].Value = RepID;

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
        {
            con.Open();
            da.Fill(ds);
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error in selecting Distributors:" + ex);
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
        return ds.Tables[0];
    }


    /// <summary>
    /// Return Initials for given UserName.
    /// </summary>
    /// <param name="sUserName"></param>
    /// <returns></returns>
    public string Initials(string sUserName)
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("SELECT Initials FROM Logins WHERE UserName=@user_name", con);
        cmd.CommandType = CommandType.Text;

        cmd.Parameters.Add("@user_name", SqlDbType.VarChar, 50);
        cmd.Parameters["@user_name"].Value = sUserName;

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
        {
            con.Open();
            da.Fill(ds);
            return ds.Tables[0].Rows[0]["Initials"].ToString();
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
    /// Return Origin Code for internal reps.
    /// </summary>
    /// <param name="sUserName"></param>
    /// <returns></returns>
    public string OriginCode(int iRepID, string sUserName)
    {
        string sOriginCode = "Q";
       
        SqlConnection con = new SqlConnection(_Conn);
        SqlCommand cmd = new SqlCommand("usp_OriginCode", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@rep_id", iRepID);
        cmd.Parameters.AddWithValue("@user_name", sUserName);

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        try
        {
            con.Open();
            da.Fill(ds);
            DataRow r;

            if (ds.Tables[0].Rows.Count > 0)
            {
                r = ds.Tables[0].Rows[0];
                sOriginCode = r["OriginCode"].ToString();
            }

            return sOriginCode;
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


    
    
    #endregion
}

