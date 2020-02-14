using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Security;


public sealed class Codes
{
    #region Fields

    private string _CodeType;
    private string _CodeValue;
    private string _Conn;
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the Cases class.
    /// </summary>
    public Codes(string CodeType)
    {
        _CodeType = CodeType;
        _Conn = WebConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString;
    }
    #endregion

    #region Properties
    public string CodeType
    {
        get { return _CodeType; }
        set { _CodeType = value; }
    }
    public string CodeValue
    {
        get { return _CodeValue; }
        set { _CodeValue = value; }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Inserts a new record into Rep table.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public int Insert()
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Codes_Insert", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@CodeType", SqlDbType.VarChar, 20);
        cmd.Parameters.Add("@CodeValue", SqlDbType.VarChar, 20);

        cmd.Parameters["@CodeType"].Value = _CodeType;
        cmd.Parameters["@CodeValue"].Value = _CodeValue;

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
    /// Updates an existing record in Rep table.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public int Update()
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Codes_Update", con);
        cmd.CommandType = CommandType.StoredProcedure;

         cmd.Parameters.Add("@CodeType", SqlDbType.VarChar, 20);
        cmd.Parameters.Add("@CodeValue", SqlDbType.VarChar, 20);

        cmd.Parameters["@CodeType"].Value = _CodeType;
        cmd.Parameters["@CodeValue"].Value = _CodeValue;

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
    public int Delete()
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("usp_Codes_Delete", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@CodeType", SqlDbType.VarChar, 20);
        cmd.Parameters.Add("@CodeValue", SqlDbType.VarChar, 20);

        cmd.Parameters["@CodeType"].Value = _CodeType;
        cmd.Parameters["@CodeValue"].Value = _CodeValue;

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


    public DataSet Select()
    {
        SqlConnection con = new SqlConnection(_Conn);

        SqlCommand cmd = new SqlCommand("SELECT * FROM SecurityLevels ORDER BY SecurityLevel", con);
        cmd.CommandType = CommandType.Text;

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
    /// AppStateID 1    Normal
    /// AppStateID 2    Warning - System will be down briefly today at 3:45 PM.
    /// AppStateID 3    Down - System is down for maintenance.
    /// </summary>
    /// <returns></returns>
    public int AppState()
    {
        SqlConnection con = new SqlConnection(_Conn);
        SqlCommand cmd = new SqlCommand("SELECT AppStateID FROM AppControl", con);
        cmd.CommandType = CommandType.Text;
        int iAppState = 0;

        try
        {
            con.Open();

            using (SqlDataAdapter _sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                DataSet _dataset = new DataSet();
                _sqlDataAdapter.Fill(_dataset);

                iAppState = Convert.ToInt32(_dataset.Tables[0].Rows[0][0]);
                return iAppState;
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

    
    
    #endregion
}

