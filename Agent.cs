using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MGM_Transformer
{
    class Agent
    {
        private string _sSQL;
        private DataSet _ds;

        private string _sAgentCode = "";
        private string _sCompany = "";
        private string _sAddressCity = "";
        private string _sAddressState = "";
        private string _sAddressStreet = "";
        private string _sAddressZip = "";
        private string _sCompanyEMail = "";
        private string _sDefaultContactName = "";
        private string _sPhone = "";

        private static DataSet _ssds;
        private static string _ssSQL;

        private int _iRepNo;

        public Agent(int iRepNo)
        {

            if (iRepNo == -1)
            {
                _sCompany = "All Agents";
                _iRepNo = -1;
                _sAgentCode = "All Agents";
            }
            else
            {
                _iRepNo = iRepNo;
                _ds = DataLink.Select("select * from Agents where RepNo = " + iRepNo, DataLinkCon.bpss);

                if (_ds.Tables.Count > 0)
                {
                    IEnumerable<DataRow> rows = _ds.Tables[0].AsEnumerable();

                    _sAgentCode = rows.Select(r => r["AgentCode"].ToString()).First();
                    _sCompany = rows.Select(r => r["Company"].ToString()).First();
                    _sAddressCity = rows.Select(r => r["AddressCity"].ToString()).First();
                    _sAddressState = rows.Select(r => r["AddressState"].ToString()).First();
                    _sAddressStreet = rows.Select(r => r["AddressStreet"].ToString()).First();
                    _sAddressZip = rows.Select(r => r["AddressZip"].ToString()).First();
                    _sCompanyEMail = rows.Select(r => r["CompanyEmail"].ToString()).First();
                    _sDefaultContactName = rows.Select(r => r["DefaultContactName"].ToString()).First();
                    _sPhone = rows.Select(r => r["Phone"].ToString()).First();
                }
            }
        }


        public int RepNo
        {
            get { return _iRepNo; }
        }

        public string AgentCode
        {
            get { return _sAgentCode; }
        }

        public string Company
        {
            get { return _sCompany; }
        }

        public string AddressCity
        {
            get { return _sAddressCity; }
        }
        public string AddressState
        {
            get { return _sAddressState; }
        }

        public string AddressStreet
        {
            get { return _sAddressStreet; }
        }

        public string AddressZip
        {
            get { return _sAddressZip; }
        }

        public string CompanyEMail
        {
            get { return _sCompanyEMail; }
        }
        public string DefaultContactName
        {
            get { return _sDefaultContactName; }
        }

        public string Phone
        {
            get { return _sPhone; }
        }


        public static List<Agent> Items
        {
            get
            {
                List<Agent> lstAgents = new List<Agent>();
                _ssds = DataLink.Select("select RepNo from Agents", DataLinkCon.bpss);

                if (_ssds.Tables.Count > 0)
                {
                    _ssds.Tables[0].AsEnumerable().Select(r => Convert.ToInt32(r["RepNo"])).ToList().ForEach(b => lstAgents.Add(new Agent(b)));

                }

                lstAgents.Insert(0, new Agent(-1));

                return lstAgents;

            }
        }

        public static int GetMGMAgentNo(string sRepName)
        {
            int iRetValue = -1;

            DataSet ds = DataLink.Select("select MGMAgentNo from Rep where Display_Name = '" + sRepName + "'", DataLinkCon.mgmuser);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        iRetValue = ds.Tables[0].AsEnumerable().Select(r => Convert.ToInt32(r["MGMAgentNo"])).First();
                    }

                }
            }


            return iRetValue;
        }




        public static string GetAgentCode(int iMGMAgentNo, string sDisplayName)
        {
            int iRepID = -1;
            string sAgentCode = "";

            DataSet ds = DataLink.Select("select RepID from Rep where MGMAgentNo = " +
                                       iMGMAgentNo + " and Display_Name = '" +  sDisplayName + "'", DataLinkCon.mgmuser);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        iRepID = ds.Tables[0].AsEnumerable().Select(r => Convert.ToInt32(r["RepID"])).First();
                    }

                }
            }


            if(iRepID != -1)
            {
                ds = DataLink.Select("select AgentCode from Agents where RepIDWeb = " + iRepID, DataLinkCon.bpss);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            sAgentCode = ds.Tables[0].AsEnumerable().Select(r => r["AgentCode"].ToString()).First();
                        }

                    }
                }
            }

            return sAgentCode;


        }

    }
}