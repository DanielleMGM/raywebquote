using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace MGM_Transformer
{
    public class Transformer
    {
        #region Fields

        private bool _bApprovalReqd = false;
        private bool _bInternal = false;
        private bool _bIndoorAllowed = false;     // Low KVA's only offer Indoor/Outdoor.  Starting with 225 KVA, Indoor unless add a rainhood, etc.
        private bool _bCustom = false;              // True = Custom.  False = Stock.
        private bool _bForExport = false;
        private bool _bMarineDuty = false;
        private bool _bNoFreeShipping = false;
        private bool _bStainless = false;
        private bool _bStepUp = false;
        private bool _bStockPossible = false;     // True = Custom could be saved as stock.
        private bool _bTENVPossible = false;
        private bool _bTotallyEnclosedNonVentilated = false;
        private bool _bZigZag = false;

        private decimal _decKVA = 0;
        private decimal _decKVAUsed = 0;
        private decimal _decPriceAdjustment = 0;
        private decimal _decPriceExpedite = 0;
        private decimal _decPriceExpedite_Calc = 0;
        private decimal _decPriceKit = 0;
        private decimal _decPriceKit_Calc = 0;
        private decimal _decPriceKit_Ext = 0;
        private decimal _decPriceKit_OSHPD = 0;
        private decimal _decPriceKit_OSHPD_Calc = 0;
        private decimal _decPriceKit_OSHPD_Ext = 0;
        private decimal _decPriceKit_RodentBird = 0;
        private decimal _decPriceKit_RodentBird_Calc = 0;
        private decimal _decPriceKit_RodentBird_Ext = 0;
        private decimal _decPriceKit_WallBracket = 0;
        private decimal _decPriceKit_WallBracket_Calc = 0;
        private decimal _decPriceKit_WallBracket_Ext = 0;
        private decimal _decPriceTotal_Calc = 0;
        private decimal _decPriceUnit = 0;
        private decimal _decPriceUnit_Calc = 0;

        private int _iID = 0;               // Custom or Stock ID.
        private int _iKitID = 0;
        private int _iKitID_OSHPD = 0;
        private int _iKitID_RodentBird = 0;
        private int _iKitID_WallBracket = 0;
        private int _iKitQty = 0;
        private int _iKitQty_OSHPD = 0;
        private int _iKitQty_RodentBird = 0;
        private int _iKitQty_WallBracket = 0;
        private int _iQuoteID = 0;
        private int _iQuoteDetailsID = 0;
        private int _iQty = 0;
        private int _iRepID = 0;
        private int _iRepIDPricing = 0;
        private int _iTempRise = 0;
        private int _iTempRiseUsed = 0;
        private int _iVoltagePrimary = 0;
        private int _iVoltageSecondary = 0;

        private string _sAdjustmentReason = "";
        private string _sApprovalReason = "";
        private string _sCatalogNo = "";
        private string _sCaseColor = "";
        private string _sCaseColorOther = "";
        private string _sCaseSize = "";
        private string _sCon = "";
        private string _sCustomerTag = "";
        private string _sEfficiency = "";
        private string _sEfficiencyExemptReason = "";
        private string _sElectrostaticShield = "";
        private string _sEnclosure = "";
        private string _sEnclosureMaterial = "";
        private string _sExpediteDays = "";
        private string _sFeatureKFactor20 = "";
        private string _sFeatureNone = "";
        private string _sFeatureNonStandardTaps = "";
        private string _sFeatureNotes = "";
        private string _sFeatureOther = "";
        private string _sFeatureSpecialImpedance = "";
        private string _sFeatureSpecificDimensions = "";
        private string _sFrequency = "";
        private string _sKFactor = "";
        private string _sKFactorUsed = "";
        private string _sKitName = "";
        private string _sIndoorOutdoor = "";
        private string _sMadeInUSA = "";
        private string _sNEMA = "";
        private string _sPhase = "";
        private string _sSoundLevel = "";
        private string _sTypeSpecial = "";
        private string _sUserCreated = "";
        private string _sUserLast = "";
        private string _sVoltagePrimary = "";
        private string _sVoltageSecondary = "";
        private string _sWindings = "";

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Cases class.
        /// </summary>
        public Transformer()
        {
            _sCon = WebConfigurationManager.ConnectionStrings["mgmdb"].ConnectionString;
        }
        #endregion

        #region Properties

        public decimal AdjustmentPrice
        {
            get { return _decPriceAdjustment; }
            set { _decPriceAdjustment = value; }
        }
        public string AdjustmentReason
        {
            get { return _sAdjustmentReason; }
            set { _sAdjustmentReason = value; }
        }

        public string ApprovalReason
        {
            get { return _sApprovalReason; }
            set { _sApprovalReason = value; }
        }

        public bool ApprovalRequired
        {
            get { return _bApprovalReqd; }
            set { _bApprovalReqd = value; }
        }

        public string CaseColor
        {
            get { return _sCaseColor; }
            set
            {
                switch (value)
                {
                    case "ANSI 61 (STD)":
                    case "ANSI 49":
                    case "Other":
                        _sCaseColor = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public string CaseColorOther
        {
            get { return _sCaseColorOther; }
            set { _sCaseColorOther = value; }
        }
        public string CaseSize
        {
            get { return _sCaseSize; }
            set
            {
                switch (value)
                {
                    case "A":
                    case "B":
                    case "B+":
                    case "C":
                    case "C+":
                    case "D":
                    case "E":
                    case "F":
                    case "FM":
                    case "US64":
                    case "US72":
                    case "US80":
                    case "US90":
                    case "N/A":
                        _sCaseSize = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public string CatalogNo
        {
            get { return _sCatalogNo; }
            set { _sCatalogNo = value; }
        }

        public bool Custom
        {
            get { return _bCustom; }
            set { _bCustom = value; }
        }

        public string CustomerTag
        {
            get { return _sCustomerTag; }
            set { _sCustomerTag = value; }
        }

        public string Efficiency
        {
            get { return _sEfficiency; }
            set
            {
                switch (value)
                {
                    case "TP-1":
                    case "EXEMPT":
                    case "DOE2016":
                        _sEfficiency = value;
                        break;
                    default:
                        break;
                }
            }
        }
        public string EfficiencyExemptReason
        {
            get { return _sEfficiencyExemptReason; }
        }

        public string ElectrostaticShield
        {
            get { return _sElectrostaticShield; }
            set
            {
                switch (value)
                {
                    case "None":
                    case "Shielded":
                    case "Two":
                        _sElectrostaticShield = value;
                        break;
                    default:
                        break;
                }
            }
        }
        public string Enclosure
        {
            get {
                // Calculate the correct Enclosure dropdown value, given the proper inputs to this point:
                // Enclosure Material, TENV (true/false), and Indoor/Outdoor.
                EnclosureCalc();

                return _sEnclosure; 
            }
            set
            {
                _bTotallyEnclosedNonVentilated = false;

                switch (value)
                {
                    case "HRPO NEMA 1 Indoor":
                        _sEnclosure = value;
                        _sEnclosureMaterial = "HRPO (STD)";
                        _sIndoorOutdoor = "Indoor";
                        _bStainless = false;
                        break;
                    case "HRPO NEMA 3R Indoor/Outdoor":
                        _sEnclosure = value;
                        _sEnclosureMaterial = "HRPO (STD)";
                        _sIndoorOutdoor = "Indoor/Outdoor";
                        _bStainless = false;
                        break;
                    case "HRPO NEMA 4 Indoor/Outdoor TENV":
                        _bTotallyEnclosedNonVentilated = true;
                        _sEnclosure = value;
                        _sEnclosureMaterial = "HRPO (STD)";
                        _sIndoorOutdoor = "Indoor/Outdoor";
                        _bStainless = false;
                        break;
                    case "No enclosure - Core and Coil Only":
                        _sEnclosure = value;
                        _sEnclosureMaterial = "Core and Coil Only";
                        _sIndoorOutdoor = "Indoor";
                        _bStainless = false;
                        break;
                    case "Stainless NEMA 1 Indoor":
                        _sEnclosure = value;
                        if (_sEnclosureMaterial != "316 Stainless Steel")
                            _sEnclosureMaterial = "304 Stainless Steel";
                        _sIndoorOutdoor = "Indoor";
                        _bStainless = true;
                        break;
                    case "Stainless NEMA 3RX Indoor/Outdoor":
                        _sEnclosure = value;
                        if (_sEnclosureMaterial != "316 Stainless Steel")
                            _sEnclosureMaterial = "304 Stainless Steel";
                        _sIndoorOutdoor = "Indoor/Outdoor";
                        _bStainless = true;
                        break;
                    case "Stainless NEMA 4X Indoor/Outdoor TENV":
                        _bTotallyEnclosedNonVentilated = true;
                        _sEnclosure = value;
                        if (_sEnclosureMaterial != "316 Stainless Steel")
                            _sEnclosureMaterial = "304 Stainless Steel";
                        _sIndoorOutdoor = "Indoor/Outdoor";
                        _bStainless = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public string EnclosureMaterial
        {
            get { return _sEnclosureMaterial; }
            set
            {
                switch (value)
                {
                    case "HRPO (STD)":
                        _sEnclosureMaterial = value;
                        _bStainless = false;
                        break;
                    case "304 Stainless Steel":
                        _sEnclosureMaterial = value;
                        _bStainless = true;
                        break;
                    case "316 Stainless Steel":
                        _sEnclosureMaterial = value;
                        _bStainless = true;
                        break;
                    case "Core and Coil Only":
                        _sEnclosureMaterial = value;
                        _bStainless = false;
                        break;
                    default:
                        break;
                }
            }
        }
        public string ExpediteDays
        {
            get { return _sExpediteDays; }
            set { _sExpediteDays = value; }
        }
        public decimal ExpeditePrice
        {
            get { return _decPriceExpedite; }
            set { _decPriceExpedite = value; }
        }
        public decimal ExpeditePriceCalc
        {
            get { return _decPriceExpedite_Calc; }
            set { _decPriceExpedite_Calc = value; }
        }

        public string FeatureNone
        {
            get { return _sFeatureNone; }
            set { _sFeatureNone = value; }
        }
        public string FeatureNonStandardTaps
        {
            get { return _sFeatureNonStandardTaps; }
            set { _sFeatureNonStandardTaps = value; }
        }
        public string FeatureSpecialImpedance
        {
            get { return _sFeatureSpecialImpedance; }
            set { _sFeatureSpecialImpedance = value; }
        }
        public string FeatureSpecificDimensions
        {
            get { return _sFeatureSpecificDimensions; }
            set { _sFeatureSpecificDimensions = value; }
        }
        public string FeatureKFactor20
        {
            get { return _sFeatureKFactor20; }
            set { _sFeatureKFactor20 = value; }
        }
        public string FeatureOther
        {
            get { return _sFeatureOther; }
            set { _sFeatureOther = value; }
        }
        public string FeatureNotes
        {
            get { return _sFeatureNotes; }
            set { _sFeatureNotes = value; }
        }

        public string Frequency
        {
            get { return _sFrequency; }
            set
            {
                switch (value)
                {
                    case "60 Hz (STD)":
                    case "50/60 Hz":
                    case "400 Hz":
                        _sFrequency = value;
                        break;
                    default:
                        break;
                }
            }
        }
        public bool ForExport
        {
            get { return _bForExport; }
            set { _bForExport = value; }
        }
        /// <summary>
        /// If Stock, StockID.  If Custom, CustomStockID.
        /// </summary>
        public int ID
        {
            get { return _iID; }
            set { _iID = value; }
        }

        public string IndoorOutdoor
        {
            get { return _sIndoorOutdoor; }
            set
            {
                switch (value)
                {
                    case "Indoor":
                    case "Indoor/Outdoor":
                    case "Outdoor":
                        _sIndoorOutdoor = value;
                        break;
                    default:
                        break;
                }

                EnclosureCalc();
            }
        }

        public bool Internal
        {
            get { return _bInternal; }
        }

        public string KFactor
        {
            get { return _sKFactor; }
            set
            {
                switch (value)
                {
                    case "K-1 (STD)":
                    case "K-4":
                    case "K-6":
                    case "K-9":
                    case "K-13":
                    case "K-20":
                        _sKFactor = value;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// KFactor used in price calculations.
        /// </summary>
        public string KFactorUsed
        {
            get { return _sKFactorUsed; }
            set
            {
                switch (value)
                {
                    case "K-1 (STD)":
                    case "K-4":
                    case "K-6":
                    case "K-9":
                    case "K-13":
                    case "K-20":
                        _sKFactorUsed = value;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Primary Kit ID, which includes Rain Hoods, Louvers.  These impact whether or not a transformer is Indoor or Outdoor,
        /// which determines the NEMA rating, often specified.
        /// </summary>
        public int KitID
        {
            get { return _iKitID; }
            set { _iKitID = value; }
        }
        public string KitName
        {
            get { return _sKitName; }
            set { _sKitName = value; }
        }
        public decimal KitPrice
        {
            get { return _decPriceKit; }
            set { _decPriceKit = value; }
        }
        public decimal KitPriceCalc
        {
            get { return _decPriceKit_Calc; }
            set { _decPriceKit_Calc = value; }
        }
        public decimal KitPriceExt
        {
            get { return _decPriceKit_Ext; }
        }
        public int KitQty
        {
            get { return _iKitQty; }
            set { 
                _iKitQty = value;
                
                // Refresh whether indoor or outdoor, based on whether or not a rain hood kit is included.
                IndoorOutCalc();

                // Refresh the current type of enclosure this transformer has.
                EnclosureCalc();
            }
        }

        public int KitID_OSHPD
        {
            get { return _iKitID_OSHPD; }
            set { _iKitID_OSHPD = value; }
        }
        public decimal KitPrice_OSHPD
        {
            get { return _decPriceKit_OSHPD; }
            set { _decPriceKit_OSHPD = value; }
        }
        public decimal KitPriceCalc_OSHPD
        {
            get { return _decPriceKit_OSHPD_Calc; }
            set { _decPriceKit_OSHPD_Calc = value; }
        }
        public decimal KitPriceExt_OSHPD
        {
            get { return _decPriceKit_OSHPD_Ext; }
        }
        public int KitQty_OSHPD
        {
            get { return _iKitQty_OSHPD; }
            set { _iKitQty_OSHPD = value; }
        }

        public int KitID_RodentBird
        {
            get { return _iKitID_RodentBird; }
            set { _iKitID_RodentBird = value; }
        }
        public decimal KitPrice_RodentBird
        {
            get { return _decPriceKit_RodentBird; }
            set { _decPriceKit_RodentBird = value; }
        }
        public decimal KitPriceCalc_RodentBird
        {
            get { return _decPriceKit_RodentBird_Calc; }
            set { _decPriceKit_RodentBird_Calc = value; }
        }
        public decimal KitPriceExt_RodentBird
        {
            get { return _decPriceKit_RodentBird_Ext; }
        }
        public int KitQty_RodentBird
        {
            get { return _iKitQty_RodentBird; }
            set { _iKitQty_RodentBird = value; }
        }

        public int KitID_WallBracket
        {
            get { return _iKitID_WallBracket; }
            set { _iKitID_WallBracket = value; }
        }
        public decimal KitPrice_WallBracket
        {
            get { return _decPriceKit_WallBracket; }
            set { _decPriceKit_WallBracket = value; }
        }
        public decimal KitPriceCalc_WallBracket
        {
            get { return _decPriceKit_WallBracket_Calc; }
            set { _decPriceKit_WallBracket_Calc = value; }
        }
        public decimal KitPriceExt_WallBracket
        {
            get { return _decPriceKit_WallBracket_Ext; }
        }
        public int KitQty_WallBracket
        {
            get { return _iKitQty_WallBracket; }
            set { _iKitQty_WallBracket = value; }
        }
        
        public decimal KVA
        {
            get { return _decKVA; }
            set { _decKVA = value; }
        }

        /// <summary>
        /// KVA used in price calculations.
        /// </summary>
        public decimal KVAUsed
        {
            get { return _decKVAUsed; }
            set { _decKVAUsed = value; }
        }
        
        public string MadeInUSA
        {
            get { return _sMadeInUSA; }
            set
            {
                switch (value)
                {
                    case "None":
                    case "Made in USA":
                    case "PA Steel":
                    case "Buy America":
                    case "Buy American":
                    case "ARRA":
                        _sMadeInUSA = value;
                        break;
                    default:
                        break;
                }
            }
        }
        public bool MarineDuty
        {
            get { return _bMarineDuty; }
            set { _bMarineDuty = value; }
        }

        public string NEMA
        {
            get { return _sNEMA; }
            set
            {
                switch (value)
                {
                    case "01":
                    case "3R":
                    case "3RX":
                    case "4":
                    case "4X":
                    case "CC":
                        _sNEMA = value;
                        break;
                    default:
                        break;
                }
                EnclosureCalcFromNema();
            }
        }

        /// <summary>
        /// If True, adding a Rain Hood will make this transformer Outdoor rated.
        /// Set in IndoorOutCalc().
        /// </summary>
        public bool IndoorAllowed
        {
            get {
                IndoorOutCalc();

                return _bIndoorAllowed; 
            }
        }
        public string Phase
        {
            get { return _sPhase; }
            set
            {
                switch (value)
                {
                    case "Single":
                    case "Three":
                        _sPhase = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public decimal Price
        {
            get { return _decPriceUnit; }
            set { _decPriceUnit = value; }
        }
        public decimal PriceExt
        {
            get { return _decPriceTotal_Calc; }
            set { _decPriceTotal_Calc = value; }
        }
        public decimal PriceCalc
        {
            get { return _decPriceUnit_Calc; }
            set { _decPriceUnit_Calc = value; }
        }

        public decimal PriceTotal
        {
            get { return _decPriceTotal_Calc; }
            set { _decPriceTotal_Calc = value; }
        }

        public int QuoteID
        {
            get { return _iQuoteID; }
            set { _iQuoteID = value; }
        }
        public int QuoteDetailsID
        {
            get { return _iQuoteDetailsID; }
            set { _iQuoteDetailsID = value; }
        }
        public int Qty
        {
            get { return _iQty; }
            set { _iQty = value; }
        }
        
        public int RepID
        {
            get { return _iRepID; }
            set { _iRepID = value; }
        }
        public int RepIDPricing
        {
            get { return _iRepIDPricing; }
            set { _iRepIDPricing = value; }
        }

        public string SoundLevel
        {
            get { return _sSoundLevel; }
            set
            {
                switch (value)
                {
                    case "-1 dB":
                    case "-2 dB":
                    case "-3 dB":
                    case "-4 dB":
                    case "-5 dB":
                    case "-6 dB":
                    case "-7 dB":
                    case "-8 dB":
                    case "-9 dB":
                    case "-10 dB":
                        _sSoundLevel = value;
                        break;
                    default:
                        break;
                }
            }
        }

         public bool ShippingNoFree
        {
            get { return _bNoFreeShipping; }
            set { _bNoFreeShipping = value; }
        }

        /// <summary>
        /// Set by assigning an enclosure.
        /// </summary>
        public bool Stainless
        {
            get { return _bStainless; }
        }

        
        /// <summary>
        /// Read only.  Set when entering voltages.
        /// </summary>
        public bool StepUp
        {
            get { return _bStepUp; }
        }

        /// <summary>
        /// If True, this Custom unit could be saved as Stock, possibly except for DOE2016 Efficiency ratings.
        /// </summary>
        public bool StockPossible
        {
            get { return _bStockPossible; }
            set { _bStockPossible = value; }
        }

      
        // Get logic for seeing if TENV is available.
        public int TempRise
        {
            get { return _iTempRise; }
            set 
            {
                switch (value)
                {
                    case 150:
                    case 115:
                    case 80:
                        _iTempRise = value; 
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Temp rise used in price calculations.
        /// </summary>
        public int TempRiseUsed
        {
            get { return _iTempRiseUsed; }
            set
            {
                switch (value)
                {
                    case 150:
                    case 115:
                    case 80:
                        _iTempRiseUsed = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public bool TotallyEnclosedNonVentilated
        {
            get { return _bTotallyEnclosedNonVentilated; }
            set { _bTotallyEnclosedNonVentilated = value; }
        }
        public string TypeSpecial
        {
            get { return _sTypeSpecial; }
            set
            {
                switch (value)
                {
                    case "None":
                    case "Auto Transformer":
                    case "Drive Isolation":
                    case "Harmonic Mitigating":
                    case "Scott-T":
                    case "Zig Zag":
                        _sTypeSpecial = value;
                        break;
                    default:
                        _sTypeSpecial = "None";
                        break;
                }

                // Used to display zig zag options.
                switch (value)
                {
                    case "Harmonic Mitigating":
                    case "Zig Zag":
                        _bZigZag = true;
                        break;
                    default:
                        _bZigZag = false;
                        break;
                }
            }
        }

        public string UserCreated
        {
            get { return _sUserCreated; }
            set {_sUserCreated = value; }
        }
        public string UserLast
        {
            get { return _sUserLast; }
            set { _sUserLast = value; }
        }

        public string VoltagePrimary
        {
            get { return _sVoltagePrimary; }
            set
            {
                _sVoltagePrimary = value;
                Voltage(true);      // Configure this voltage.
            }
        }
        public string VoltageSecondary
        {
            get { return _sVoltageSecondary; }
            set { 
                _sVoltageSecondary = value;
                Voltage(false);
            }
        }

        public string Windings
        {
            get { return _sWindings; }
            set 
            {
                switch (value)
                {
                    case "Aluminum":
                    case "Copper":
                        _sWindings = value; 
                        break;
                    default:
                        break;
                }
            }
        }
        public bool ZigZag
        {
            get { return _bZigZag; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determine if TENV is available for this configuration.
        /// </summary>
        /// <returns></returns>
        public bool TENVAvailable()
        {
 
            int iKFactorGroup = 1;
            int iPhase = 3;
    
            switch (_sKFactor)
            {
                case "K-1 (STD)":
                    iKFactorGroup = 1;
                    break;
                case "K-4":
                case "K-6":
                    iKFactorGroup = 2;
                    break;
                case "K-9":
                case "K-13":
                case "K-20":
                    iKFactorGroup = 3;
                    break;
                default:
                    iKFactorGroup = 1;
                    break;
            }

            switch (_sPhase)
            {
                case "Three":
                    iPhase = 3;
                    break;
                case "Single":
                    iPhase = 1;
                    break;
                default:
                    iPhase = 3;
                    break;
            }

            // True unless shown unavailable.
            _bTENVPossible = true;

            if (iPhase == 3)
            {
                switch(iKFactorGroup)
                {
                    case 1:
                        if(_decKVA > 300 || (_decKVA == 300 && _iTempRise < 150))
                            _bTENVPossible = false;
                        break;
                    case 2:
                        if(_decKVA > 250 || (_decKVA == 250 && _iTempRise < 115))
                            _bTENVPossible = false;
                        break;
                    case 3:
                        if(_decKVA > 225 || (_decKVA == 225 && _iTempRise < 115))
                            _bTENVPossible = false;
                        break;
                }
            }
            if (iPhase == 1)
            {
                switch(iKFactorGroup)
                {
                    case 1:
                        if(_decKVA > 250)
                            _bTENVPossible = false;
                        break;
                    case 2:
                    case 3:
                        if(_decKVA > 167)
                            _bTENVPossible = false;
                        break;
                }
            }

            return _bTENVPossible; 
        }


        /// <summary>
        /// Build a primary or secondary voltage, for determining step-up, case size, etc.
        /// </summary>
        /// <param name="bPrimary"></param>
        private void Voltage(bool bPrimary)
        {
            bool isNumeric = false;
            int iVolts1 = 0;
            int iVolts2 = 0;
            int j = 0;
            int n = 0;
            string sChar = "";
            string sVoltage = "";
            string sVolts1 = "";
            string sVolts2 = "";

            if (bPrimary == true)
            {
                sVoltage = _sVoltagePrimary;
            }
            else
            {
                sVoltage = _sVoltageSecondary;
            }

            for (int i = 1; i < sVoltage.Length; i++)
            {
                sChar = sVoltage.Substring(i, 1);
                isNumeric = int.TryParse(sChar, out n);
                if (isNumeric == true)
                {
                    if (j == 0)
                    {
                        sVolts1 = sVolts1 + sChar;
                    }
                    else
                    {
                        sVolts2 = sVolts2 + sChar;
                    }
                }
                else
                {
                    // Save the first voltage, and move on to another, if there is one.
                    if (sVolts1 != "")
                    {
                        j = 1;
                        isNumeric = int.TryParse(sVolts1, out iVolts1);
                    }
                    if (sVolts2 != "")
                    {
                        isNumeric = int.TryParse(sVolts1, out iVolts2);
                    }

                    // If we have a second voltage value > first, use it instead.
                    // We want to save the highest voltage value we come to.
                    if (iVolts2 > iVolts1)
                    {
                        sVolts1 = sVolts2;
                        iVolts1 = iVolts2;
                        sVolts2 = "";
                        iVolts2 = 0;
                    }
                }
            }
            // Cleanup after the loop.
            if (sVolts1 != "")
            {
                isNumeric = int.TryParse(sVolts1, out iVolts1);
            }
            if (sVolts2 != "")
            {
                isNumeric = int.TryParse(sVolts1, out iVolts2);
            }

            // If we have a second voltage value > first, use it instead.
            // We want to save the highest voltage value we come to.
            if (iVolts2 > iVolts1)
            {
                sVolts1 = sVolts2;
                iVolts1 = iVolts2;
            }

            if (bPrimary == true)
            {
                _iVoltagePrimary = iVolts1;
            }
            else
            {
                _iVoltageSecondary = iVolts1;
            }

            // Calculate Step-up, which only applies if we have both primary and secondary voltages already assigned.
            if (_iVoltageSecondary > _iVoltagePrimary && _iVoltagePrimary > 0)
            {
                _bStepUp = true;
            }
            else
            {
                _bStepUp = false;
            }
        }

        /// <summary>
        /// Assumes TENV and Case Size (looked up) have been input,
        /// with optional KitID.
        /// </summary>
        private void IndoorOutCalc()
        {
           if (_sCaseSize == "") 
           {
               _bIndoorAllowed = false;
               _sIndoorOutdoor = "";
               return;
           }

            // First, determine if indoors or outdoors based on the selection.

            // TENV is always Outdoors, since it is protected from the elements.
            if (_bTotallyEnclosedNonVentilated == true)
                _sIndoorOutdoor = "Indoor/Outdoor";
            else
            {
                switch (_sCaseSize)
                {
                    case "A":
                    case "B":
                    case "B+":
                    case "C":
                    case "C+":
                        _bIndoorAllowed = false;
                        _sIndoorOutdoor = "Indoor/Outdoor";
                        break;
                    case "D":
                       _bIndoorAllowed = true;
                        // Rain hood for Case Size D.  _iKitID == 3.
                        if (_iKitQty > 0)
                            _sIndoorOutdoor = "Outdoor";
                        else
                            _sIndoorOutdoor = "Indoor";

                        break;
                    case "E":
                       _bIndoorAllowed = true;
                        // Rain hood for Case Size E.  _iKitID == 4.
                        if (_iKitQty > 0)
                            _sIndoorOutdoor = "Outdoor";
                        else
                            _sIndoorOutdoor = "Indoor";

                        break;
                    case "F":
                       _bIndoorAllowed = true;
                        // Louvers for Case Size F.  _iKitID == 16.
                        if (_iKitQty > 0)
                            _sIndoorOutdoor = "Outdoor";
                        else
                            _sIndoorOutdoor = "Indoor";

                        break;
                    default:
                        _bIndoorAllowed = true;
                        _sIndoorOutdoor = "Indoor";
                        break;
                }
             }
        }

        private void EfficiencyCalc()
        {
            _sEfficiency = "DOE2016";

            if (_bCustom == false)
            {
                _sEfficiency = "TP-1";
            }
            else
            {
                // NOTE:  Previously, step-up transformers were EXEMPT.
                // ====   Also previously, single phase transformers were TP-1.

                switch(_sFrequency)
                {
                    case "50/60 Hz":
                    case "400 Hz":
                        _sEfficiency = "EXEMPT";
                        break;
                }
                switch(_sTypeSpecial)
                {
                    case "Auto Transformer":
                    case "Harmonic Mitigating":
                    case "Scott-T":
                    case "Zig Zag":
                        _sEfficiency = "EXEMPT";
                        break;
                }
                if (_bForExport == true || _bTotallyEnclosedNonVentilated == true)
                {
                    _sEfficiency = "EXEMPT";
                }
            }
        }


        /// <summary>
        /// Inserts a new record into Rep table.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public int Insert()
        {
            SqlConnection con = new SqlConnection(_sCon);

            SqlCommand cmd = new SqlCommand("usp_Codes_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

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
        /// Calculate the correct enclosure dropdown value, given these other inputs:
        ///  - Enclosure Material (HRPO, 304 SS, 316 SS, or Core & Coil Only).
        ///  - Indoor/Outdoor (Indoor, Indoor/Outdoor, Outdoor).
        ///  - Totally Enclosed Non-Ventilated (true/false).
        /// </summary>
        private void EnclosureCalc()
        {
            if (_sCaseSize == "") return;
            if (_sIndoorOutdoor == "") return;
            if (_sEnclosureMaterial == "") return;

            if (_sEnclosureMaterial == "HRPO (STD)")
            {
                if (_bTotallyEnclosedNonVentilated == true)
                {
                    _sEnclosure = "HRPO NEMA 4 Indoor/Outdoor TENV";
                }
                else if (_sIndoorOutdoor == "Indoor" && _iKitQty == 0)
                {
                    _sEnclosure = "HRPO NEMA 1 Indoor";
                }
                else if (_sIndoorOutdoor == "Indoor/Outdoor" || _sIndoorOutdoor == "Outdoor" || _iKitQty > 0)
                {
                    _sEnclosure = "HRPO NEMA 3R Indoor/Outdoor";
                }
            }
            else if (_sEnclosureMaterial == "304 Stainless Steel" || _sEnclosureMaterial == "316 Stainless Steel")
            {
                if (_bTotallyEnclosedNonVentilated == true)
                {
                    _sEnclosure = "Stainless NEMA 4X Indoor/Outdoor TENV";
                }
                else if (_sIndoorOutdoor == "Indoor" && _iKitQty == 0)
                {
                    _sEnclosure = "Stainless NEMA 1 Indoor";
                }
                else if (_sIndoorOutdoor == "Indoor/Outdoor" || _sIndoorOutdoor == "Indoor/Outdoor" || _iKitQty > 0)
                {
                    _sEnclosure = "Stainless NEMA 3RX Indoor/Outdoor";
                }
            }
            else if (_sEnclosureMaterial == "Core and Coil Only")
            {
                _sEnclosure = "No enclosure - Core and Coil Only";
            }
        }
       
        /// <summary>
        /// Called when selecting an item from the Catalog Number picker.
        /// </summary>
        private void EnclosureCalcFromNema()
        {

            if (_sNEMA == "") return;

            // The only key value that doesn't get set here is the Case Size.
            switch (_sNEMA)
            {
                case "01":
                    _bTotallyEnclosedNonVentilated = false;
                    _sEnclosure = "HRPO NEMA 1 Indoor";
                    _sEnclosureMaterial = "HRPO (STD)";
                    _sIndoorOutdoor = "Indoor";
                    break;
                case "3R":
                    _bTotallyEnclosedNonVentilated = false;
                    _sEnclosure = "HRPO NEMA 3R Indoor/Outdoor";
                    _sEnclosureMaterial = "HRPO (STD)";
                    _sIndoorOutdoor = "Indoor/Outdoor";
                    break;
                case "3RX":
                    _bTotallyEnclosedNonVentilated = false;
                    _sEnclosure = "Stainless NEMA 3RX Indoor/Outdoor";
                    if (_sEnclosureMaterial != "316 Stainless Steel")
                        _sEnclosureMaterial = "304 Stainless Steel";
                    _sIndoorOutdoor = "Indoor/Outdoor";
                    break;
                case "04":
                    _bTotallyEnclosedNonVentilated = false;
                    _sEnclosure = "HRPO NEMA 4 Indoor/Outdoor TENV";
                    _sEnclosureMaterial = "HRPO (STD)";
                    _sIndoorOutdoor = "Indoor/Outdoor";
                    break;
                case "4X":
                    _bTotallyEnclosedNonVentilated = false;
                    _sEnclosure = "Stainless NEMA 4X Indoor/Outdoor TENV";
                    if (_sEnclosureMaterial != "316 Stainless Steel")
                        _sEnclosureMaterial = "304 Stainless Steel";
                    _sIndoorOutdoor = "Indoor/Outdoor";
                    break;
                case "CC":
                    _bTotallyEnclosedNonVentilated = false;
                    _sEnclosure = "No enclosure - Core and Coil Only";
                    _sEnclosureMaterial = "Core and Coil Only";
                    _sIndoorOutdoor = "Indoor/Outdoor";
                    break;
            }

        }

        #endregion
    }
}