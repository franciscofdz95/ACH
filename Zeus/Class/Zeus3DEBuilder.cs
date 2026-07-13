using System;
using System.Collections.Generic;
using System.Linq;
using Paysafe.Zeus3DE.Model;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Configuration;
using PaymentXP.DataObjects;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;

namespace ZeusWeb
{
    public static class Zeus3DEBuilder
    {
        public static RequestData GetZeus3deRequestData(MerchantApp mApp, ZeusMultiLinkages multiLinkDecision, List<String> vendorSearches, string userRequested)
        {
            RequestData zeus3deRequest = new RequestData();

            Zeus3DEData zeus3deData = Getzeus3deData(Convert.ToInt32(mApp.ID));

            zeus3deRequest.merchantReferenceId = CommonUtility.Util.if_s(mApp.ID).EmptytoNull();

            zeus3deRequest.salesAgent = GetSalesAgentName(mApp.PrimaryContactUID);
            zeus3deRequest.salesSupportAgent = zeus3deRequest.salesAgent;
            zeus3deRequest.salesAgentId = CommonUtility.Util.if_s(mApp.AgentID).EmptytoNull();
            zeus3deRequest.salesAgentName = mApp.AgentDBA.EmptytoNull();
            zeus3deRequest.vendorSearches = vendorSearches;
            zeus3deRequest.multiLinkDecisionId = multiLinkDecision.MultiLinkDecisionID;
            zeus3deRequest.callbackUrl = ConfigurationManager.AppSettings["Zeus3deCallBackURL"];
            zeus3deRequest.userRequested = userRequested.EmptytoNull();
            zeus3deRequest.underwritingAgent = (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_CREDIT_UNDERWRITING) ? UserSessions.CurrentUser.FirstLastName.EmptytoNull() : string.Empty;
            zeus3deRequest.businessInformation = GetBusinessInfo(mApp, zeus3deData);
            zeus3deRequest.bankInformation = GetBankInfo(mApp);
            zeus3deRequest.tenantInformation = GetTenantInfo(mApp, zeus3deData, multiLinkDecision);
            zeus3deRequest.businessOwners = GetBusinessOwners(mApp);
           
            //code changes done for PXP-13771 by koshlendra start            
            zeus3deRequest.businessLocation = mApp.Office.ToString();
            zeus3deRequest.accountStatusCc = mApp.StatusName;            
            zeus3deRequest.gateway = mApp.Brand.ToString();
                //GetGateway(mApp);
            zeus3deRequest.businessRelationShips = GetBusinessRelationship(mApp);
            zeus3deRequest.contact = GetContacts(mApp);
            //code changes done for PXP-13771 by koshlendra end
            /***As part of the ticket PXP-2881, Chandra removed the multilinkReasoncodes from RequestData and added it to the context text***/

            //Getzeus3deWebsite(zeus3deRequest, mApp);
            
            return zeus3deRequest;
        }

        private static IList<MultilinkReasonCode> GetMultilinkReasonCodes(string zid)
        {

            string matchCase = "X";
            IList<MultilinkReasonCode> mrcList = new List<MultilinkReasonCode>();
            Hashtable prms = new Hashtable();

            //DataMerchantApp data = DataAccess.DataMerchantAppDao;
            //bool isMultiLink = data.CheckMultiLinkInfoV2(prms);

            prms.Add("@MerchantID", zid);
            prms.Add("@PageSize", 1000);
            prms.Add("@CurrentPage", 1);
            prms["@SortOrder"] = "Null";
            prms["@SortDirection"] = 0;

            DataTable dt = DataMerchantAppPaging.GetMultiLinkPaging(prms, 1000, 0);

            foreach (DataRow dr in dt.Rows)
            {
                List<string> fieldMachtes = new List<string>();
                MultilinkReasonCode zeus3DE_MRC = new MultilinkReasonCode();

                zeus3DE_MRC.zid = dr["ID"].ToString();
                if (dr["LegalName"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("MLE");

                if (dr["DBA"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("DBA");

                if (dr["ContactNo"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("ContactNo");

                if (dr["DBANo"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("DBANo");

                //Ani:Start:PXP-2760
                if (dr["BusinessFax"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("BusinessFax");
                //Ani:End:PXP-2760

                if (dr["MailingAddress"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("MailingAddress");

                if (dr["BusinessAddress"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("BusinessAddress");

                if (dr["WebSite"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("WebSite");

                if (dr["EmailAddress"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("EmailAddress");

                if (dr["TaxID"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("TaxID");

                if (dr["OwnerName"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("OwnerName");

                if (dr["OwnerSSN"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("OwnerSSN");

                if (dr["OwnerPhone"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("OwnerPhone");

                if (dr["Contact"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("ContactName");

                if (dr["BankAccount"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("BankAccountNumber");

                /**Chandra: Added OwnerAddress to the list of fieldsMatched for PXP-2758 **/
                if (dr["OwnerAddress"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("OwnerAddress");

                //Passing Customer service Phone number for Matched fileds to MOBS
                if (dr["CustPhone"].ToString().ToUpper().Equals(matchCase))
                    fieldMachtes.Add("CustPhone");

                /***As part of the ticket PXP-2881,Chandra renamed fieldMatched to fieldsMatched***/
                zeus3DE_MRC.fieldsMatched = fieldMachtes.ToArray();

                zeus3DE_MRC.ytdVolume = Convert.ToDecimal(dr["YTDVolume"]);

                if (!string.IsNullOrEmpty(dr["CreatedDate"].ToString()))
                    zeus3DE_MRC.createdDate = dr["CreatedDate"].ToString();
                else
                    zeus3DE_MRC.createdDate = "null";
                
                zeus3DE_MRC.status = dr["Status"].ToString();
                zeus3DE_MRC.closureCode = dr["ClosureCode"].ToString();
                
                if (!string.IsNullOrEmpty(dr["CancellationDate"].ToString()))
                    zeus3DE_MRC.cancellationDate = dr["CancellationDate"].ToString();
                else
                    zeus3DE_MRC.cancellationDate = "null";


                zeus3DE_MRC.ccVolume = CommonUtility.Util.if_dec(dr["CCVolume"],0M);
                zeus3DE_MRC.achVolume = CommonUtility.Util.if_dec(dr["ACHVolume"],0M);

                mrcList.Add(zeus3DE_MRC);
            }

            return mrcList;
        }

        private static string GetMerchantTransactionType(MerchantApp mApp)
        {
            string merchantTransactionType = string.Empty;
            DataMerchantApp dataMerchantApp = DataAccess.DataMerchantAppDao;
            merchantTransactionType = dataMerchantApp.GetMerchantTransactionType(mApp.ID);
            return merchantTransactionType;
        }

        private static string GetSalesAgentName(string userUID)
        {
            User user = new User();
            if (userUID != "-1" && !(string.IsNullOrEmpty(userUID)))
            {
                DataUser du = DataAccess.DataUserDao;
                user = du.GetUser(userUID);
            }
            return user.FirstLastName.EmptytoNull();
        }

        //private static RequestIn Getzeus3deWebsite(RequestIn zeus3deRequest, MerchantApp mApp)
        //{
        //    Website website = new Website();
        //    website.url = mApp.BusinessWebsite;

        //    zeus3deRequest.businessInformation.website = website;
        //    return zeus3deRequest;
        //}

        private static BusinessInformation GetBusinessInfo(MerchantApp mApp, Zeus3DEData zeus3deData)
        {
            BusinessInformation zeus3deBussInfo = new BusinessInformation();

            //code changes done for PXP-13771 by koshlendra start
            zeus3deBussInfo.fax = mApp.BusinessFax == null ? null : GetNumbersfromString(mApp.BusinessFax).EmptytoNull();
            zeus3deBussInfo.descriptors = GetDescriptorList(mApp);
            //code changes done for PXP-13771 by koshlendra end
            zeus3deBussInfo.name = mApp.BusinessDBAName.EmptytoNull();
            zeus3deBussInfo.monthlyVolume = CommonUtility.Util.if_s(mApp.TinfoAverageMonthlyVMCVolume).EmptytoNull();
            //code changes done for PXP-13771 by koshlendra start
            zeus3deBussInfo.fax = mApp.BusinessFax == null ? null : GetNumbersfromString(mApp.BusinessFax).EmptytoNull();
            zeus3deBussInfo.descriptors = GetDescriptorList(mApp);
            //code changes done for PXP-13771 by koshlendra end            zeus3deBussInfo.monthlyVolume = CommonUtility.Util.if_s(mApp.TinfoAverageMonthlyVMCVolume).EmptytoNull();
            zeus3deBussInfo.legalEntity = mApp.BusinessLegalName.EmptytoNull();
            zeus3deBussInfo.isCardPresent = (zeus3deData.AccountType == "CP") ? true : false;
            zeus3deBussInfo.phone = mApp.BusinessDBAPhone == null ? null : GetNumbersfromString(mApp.BusinessDBAPhone).EmptytoNull();
            zeus3deBussInfo.alternatePhone = mApp.ContactList == null ? null : GetNumbersfromString(mApp.ContactList.FirstOrDefault(x => x.IsPrimary).GetFirstPhone()).EmptytoNull();
            zeus3deBussInfo.customerServicePhone = GetNumbersfromString(mApp.CustomerServicePhone).EmptytoNull();
            zeus3deBussInfo.email = mApp.ContactList == null ? "Business@paysafe.com" : mApp.ContactList.FirstOrDefault(x => x.IsPrimary).EmailAddress.EmptytoNull();
            zeus3deBussInfo.nationalTaxId = CommonUtility.Util.GetNumbersFromString(mApp.BusinessTaxID).EmptytoNull();
            zeus3deBussInfo.countrySubDivisionTaxId = mApp.BusinessTaxID.EmptytoNull(); //TODO: Revist This.
            zeus3deBussInfo.products = GetzeusProducts(mApp);
            zeus3deBussInfo.currency = mApp.Currency == string.Empty ? "USD" : mApp.Currency;
            zeus3deBussInfo.address = Getzeus3deBussInfoAddress(mApp);
            zeus3deBussInfo.terminalTypes = mApp.Equipments.Select(x => x.Type).Distinct().ToList();
            zeus3deBussInfo.businessCategory = new BusinessCategory()
            {
                mcc = mApp.SicCode
            };
            switch (mApp.MerchantAppTypeUID.ToUpper())
            {
                //PXP-10046 Fady Massoud Add BBVA for 3DE Call ( only Match from db handled)
                case Constants.BANK_BBVACOMPASS:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.BBVA.AcquirerId"];// code changes for PXP-16948
                    break;
               case Constants.BANK_BMO_HARRIS:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.BMOHarris.AcquirerId"];
                    break;
                case Constants.BANK_WELLS_FARGO:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.WellsFargoBankICA.AcquirerId"];
                    break;
                case Constants.BANK_WOODFOREST:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.WoodforestNationalBankICA.AcquirerId"];
                    break;
                //Begin DM-7003, DM-7002
                case Constants.BANK_WOODFOREST_SB:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.WoodforestSB.AcquirerId"];
                    break;
                //End DM-7003, DM-7002 
                //Begin DM-1103 - Key created in Web.config as "Zeus3de.CitizensBank.AcquirerId"
                case Constants.BANK_CITIZENS:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.CitizensBank.AcquirerId"];
                    break;
                // End DM-1103
                // code added to implement PXP-8073 by koshlendra start
                case Constants.CORPORATE:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.Corporate.AcquirerId"];
                    break;
                case Constants.BANK_HEADQUARTER:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.Headquarter.AcquirerId"];
                    break;
                case Constants.BANK_ACH_ONLY:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.ACHOnly.AcquirerId"];
                    break;
                // Bank added to implement PXP-8073 by koshlendra end
                case Constants.BANK_CHESAPEAKE:
                    zeus3deBussInfo.acquirerId = ConfigurationManager.AppSettings["Zeus3de.Chesapeake.AcquirerId"];
                    break;
                default:
                    break;
            }

            if (mApp.BusinessStartDate > DateTime.MinValue)
            {
                zeus3deBussInfo.registerDate = new Date();
                //zeus3deBussInfo.registerDate.day = mApp.BusinessStartDate.Day.EmptytoNull();
                //zeus3deBussInfo.registerDate.month = mApp.BusinessStartDate.Month.EmptytoNull();
                //zeus3deBussInfo.registerDate.year = mApp.BusinessStartDate.Year.EmptytoNull();

                zeus3deBussInfo.businessStartDate = new Date();
                zeus3deBussInfo.businessStartDate.day = mApp.BusinessStartDate.Day.EmptytoNull();
                zeus3deBussInfo.businessStartDate.month = mApp.BusinessStartDate.Month.EmptytoNull();
                zeus3deBussInfo.businessStartDate.year = mApp.BusinessStartDate.Year.EmptytoNull();
            }

            zeus3deBussInfo.registerNo = CommonUtility.Util.if_s(mApp.BusinessLicense, null).EmptytoNull();
            zeus3deBussInfo.hasSoleProprietor = mApp.BusinessStructureUID.ToUpper().Equals("820184DE-0254-442C-A8DF-11CFC1C7D98D") ? true : false;
            zeus3deBussInfo.website = new Website();
            zeus3deBussInfo.website.url = mApp.BusinessWebsite;

            zeus3deBussInfo.type = GetMerchantTransactionType(mApp);
            return zeus3deBussInfo;
        }
        /// <summary>
        /// Function added for BusinessRelationship in 3De request for PXP-13771
        /// </summary>
        /// <param name="mApp"></param>
        /// <returns></returns>
        public static List<BusinessRelationShips> GetBusinessRelationship(MerchantApp mApp)
        {
            List<BusinessRelationShips> businessRelationShips = new List<BusinessRelationShips>();
            BusinessRelationShips busRelationships = null;
            DataMerchantServices objServices = new DataMerchantServices();
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", mApp.MerchantAppUID);
            prms.Add("@CategoryID", 6);
            prms.Add("@Checked", true);

            DataSet dsMSService = objServices.GetMerchantServicesDetails(prms);
            if (dsMSService != null && dsMSService.Tables.Count > 0 && dsMSService.Tables[0].Rows.Count > 0) { }
            foreach (DataRow drMSService in dsMSService.Tables[0].Rows)
            {
                busRelationships = new BusinessRelationShips();
                busRelationships.id = drMSService["RelationShipRecordID"].ToString();
                busRelationships.type = drMSService["Category"].ToString();
                busRelationships.description = drMSService["Description"].ToString();
                businessRelationShips.Add(busRelationships);
            }
            return businessRelationShips;
        }

        /// <summary>
        /// Function adde for Descriptor list in request for PXP-13771 by koshlendra
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        public static List<Descriptor> GetDescriptorList(MerchantApp ma)
        {

            List<Descriptor> busInfoDescriptors = new List<Descriptor>();
            Descriptor busInfoDescriptor = null;
            DataSet dsDescr = DataAccess.DataMerchantAppDao.GetMerchantDescriptors(ma.ID);

            if (dsDescr != null && dsDescr.Tables.Count > 0 && dsDescr.Tables[0].Rows.Count > 0) { }
            foreach (DataRow drDescr in dsDescr.Tables[0].Rows)
            {
                busInfoDescriptor = new Descriptor();
                busInfoDescriptor.id = drDescr["MerchantDescriptorID"].ToString();
                busInfoDescriptor.description = drDescr["Descriptor"].ToString();
                busInfoDescriptors.Add(busInfoDescriptor);
            }
            return busInfoDescriptors;
        }
         
        private static string GetNumbersfromString(string value)
        {
            if (value != null)
            {
                return CommonUtility.Util.GetNumbersFromString(value);
            }
            return null;
        }

        private static string GetNumbersfromString(Phone phone)
        {
            if (phone != null)
            {
                return GetNumbersfromString(phone.PhoneNumberStripped);
            }

            return null;
        }

        private static Address Getzeus3deBussInfoAddress(MerchantApp mApp)
        {
            Address zeus3deBussInfoAddress = new Address();

            zeus3deBussInfoAddress.street = mApp.BusinessAddress.EmptytoNull();
            zeus3deBussInfoAddress.street2 = mApp.BusinessAddressLine2.EmptytoNull();
            zeus3deBussInfoAddress.city = mApp.BusinessCity.EmptytoNull();
            zeus3deBussInfoAddress.state = mApp.BusinessState.EmptytoNull();
            zeus3deBussInfoAddress.country = mApp.BusinessCountry.EmptytoNull();
            zeus3deBussInfoAddress.zip = mApp.BusinessZip.EmptytoNull();

            return zeus3deBussInfoAddress;
        }

        public static List<string> GetzeusProducts(MerchantApp mApp)
        {
            List<string> zeusproducts = new List<string>();

            zeusproducts.Add(mApp.MerchantSells);

            //List<Subscription> subscriptionList = PaymentXP.DataObjects.DataProduct.GetMerchantCurrentProductSubscriptionList(int.Parse(mApp.ID));
            //if (subscriptionList != null)
            //{
            //    zeusproducts =  subscriptionList.Where(p => p.IsActive).Select(x => x.Product.Name).ToList();
            //}

            return zeusproducts;
        }

        private static BankInformation GetBankInfo(MerchantApp mApp)
        {
            BankInformation zeus3deBankInfo = new BankInformation();

            zeus3deBankInfo.routingNumber = mApp.RoutingNumber.EmptytoNull();
            zeus3deBankInfo.accountNumber = mApp.AccountNumber.EmptytoNull();

            //code adde for PXP-13371 by koshlendra start
            zeus3deBankInfo.accountNumberHashed = mApp.AccountNumberHashed.EmptytoNull();
            //code adde for PXP-13371 by koshlendra end
            //zeus3deRequest.bankInformation = zeus3deBankInfo;
            return zeus3deBankInfo;
        }

        private static TenantInformation GetTenantInfo(MerchantApp mApp, Zeus3DEData zeus3deData, ZeusMultiLinkages multiLinkDecision)
        {
            TenantInformation zeus3deTenantInfo = new TenantInformation();

            zeus3deTenantInfo.id = "mobus";
            zeus3deTenantInfo.context = GetTenantContext(zeus3deData, multiLinkDecision);

            //zeus3deRequest.tenantInformation = zeus3deTenantInfo;
            return zeus3deTenantInfo;
        }

        private static Context GetTenantContext(Zeus3DEData zeus3deData, ZeusMultiLinkages multiLinkDecision)
        {
            Context zeus3deContext = new Context();

            zeus3deContext.highTicket = zeus3deData.HighTicket.EmptytoNull();
            zeus3deContext.averageTicket = zeus3deData.AvgTicket.EmptytoNull();
            zeus3deContext.ndx = zeus3deData.NDXDay.EmptytoNull();
            zeus3deContext.totalApplicableVolume = zeus3deData.TotalApprovedVolume.EmptytoNull();

            zeus3deContext.multiLinkages = new MultiLinkages();
            zeus3deContext.multiLinkages.count = multiLinkDecision.MultiLinkAccountCount;
            zeus3deContext.multiLinkages.status = ((MultiLinkDecisionResponseCode)multiLinkDecision.MultiLinkDecisionStatusCode).ToString();

            /***As part of the ticket PXP-2881, Chandra added the multilinkReasoncodes to the context text and name changed to matches***/
            zeus3deContext.multiLinkages.matches = GetMultilinkReasonCodes(multiLinkDecision.ZID.ToString()).ToArray<MultilinkReasonCode>();

            return zeus3deContext;
        }

       
        /// <summary>
        /// Function added to add contact list in request for PXP-13771 by koshlendra
        /// </summary>
        /// <param name="mApp"></param>
        /// <returns> Contact list</returns>
        private static List<Contacts> GetContacts(MerchantApp mApp)
        {
            List<Contacts> zeus3deContacts = new List<Contacts>();
            Contacts zeus3deContact = null;
            foreach (Contact cn in mApp.ContactList)
            {
                if (cn.ContactID != 0)
                {

                    zeus3deContact = new Contacts();

                    zeus3deContact.firstName = cn.FirstName;
                    zeus3deContact.middleName = cn.MiddleName;
                    zeus3deContact.lastName = cn.LastName;
                    zeus3deContact.email = cn.EmailAddress;
                    zeus3deContact.phone = cn.PhoneList == null ? null : GetNumbersfromString(cn.GetFirstPhone()).EmptytoNull();
                    zeus3deContacts.Add(zeus3deContact);

                }
            }
            return zeus3deContacts;
        }

        private static List<BusinessOwner> GetBusinessOwners(MerchantApp mApp)
        {
            List<BusinessOwner> zeus3deBussOwners = new List<BusinessOwner>();
            BusinessOwner zeus3deBussOwner = null;

            foreach (Owner owner in mApp.Owners)
            {
                if (owner.OwnerCatagoryID == 1)
                {
                    owner.FirstName = owner.FullName;
                    owner.LastName = owner.FullName;
                }

                if (!String.IsNullOrEmpty(owner.FirstName) && !String.IsNullOrEmpty(owner.LastName))
                {
                    zeus3deBussOwner = new BusinessOwner();
                    
                    //Code added for PXP-13771 by koshlendra start
                    zeus3deBussOwner.id = owner.OwnerID.EmptytoNull();
                    zeus3deBussOwner.ssnHashed = owner.SSNHashed;
                    //Code added for PXP-13771 by koshlendra end
                    zeus3deBussOwner.firstName = owner.FirstName.EmptytoNull();
                    zeus3deBussOwner.lastName = owner.LastName.EmptytoNull();
                    zeus3deBussOwner.middleName = owner.MiddleName.EmptytoNull();
                    zeus3deBussOwner.email = "owner@paysafe.com";
                    zeus3deBussOwner.jobTitle = CommonUtility.Util.if_s(owner.Title, "Owner").EmptytoNull();
                    zeus3deBussOwner.phone = CommonUtility.Util.GetNumbersFromString(owner.HomePhone).EmptytoNull();
                    zeus3deBussOwner.nationality = string.IsNullOrEmpty(owner.IDNationality) ? "US" : owner.IDNationality;
                    zeus3deBussOwner.ssn = CommonUtility.Util.GetNumbersFromString(owner.SSN).EmptytoNull();
                    zeus3deBussOwner.percentageOwnership = Convert.ToInt32(owner.PercentOwnership);

                    zeus3deBussOwner.entity = new Entity();
                    zeus3deBussOwner.entity.type = owner.OwnerCatagoryID == 1 ? "CORPORATE" : "INDIVIDUAL";
                    if (owner.OwnerCatagoryID == 1 && owner.PercentOwnership > 0)
                    {
                        zeus3deBussOwner.entity.equity = true;
                    }

                    else
                    {
                        zeus3deBussOwner.entity.equity = owner.BeneficialOwner;
                    }
                    zeus3deBussOwner.entity.controller = owner.AuthorizedSignature;
                    zeus3deBussOwner.entity.cbrWaived = owner.CBRWaived;

                    if (owner.DOB > DateTime.MinValue)
                    {
                        zeus3deBussOwner.dateOfBirth = new Date();
                        zeus3deBussOwner.dateOfBirth.day = owner.DOB.Day.EmptytoNull();
                        zeus3deBussOwner.dateOfBirth.month = owner.DOB.Month.EmptytoNull();
                        zeus3deBussOwner.dateOfBirth.year = owner.DOB.Year.EmptytoNull();
                    }

                    zeus3deBussOwner.previousAddress = new Address();
                    zeus3deBussOwner.currentAddress = GetOwnerAddress(owner);
                    zeus3deBussOwner.drivingLicence = GetDrivingLicense(owner);
                    zeus3deBussOwner.internationalPassport = GetPassportInformation(owner);

                    zeus3deBussOwners.Add(zeus3deBussOwner);
                }
            }
            // zeus3deRequest.businessOwners = zeus3deBussOwners;
            return zeus3deBussOwners;
        }

        private static Address GetOwnerAddress(Owner owner)
        {
            Address zeus3deownerAddress = new Address();

            zeus3deownerAddress.street = owner.Address1.EmptytoNull();
            zeus3deownerAddress.street2 = owner.Address2.EmptytoNull();
            zeus3deownerAddress.city = owner.City.EmptytoNull();
            zeus3deownerAddress.state = owner.State.EmptytoNull();
            zeus3deownerAddress.country = string.IsNullOrEmpty(owner.Country) ? "US" : owner.Country;
            zeus3deownerAddress.zip = owner.Zip.Trim().EmptytoNull();

            return zeus3deownerAddress;
        }

        private static DrivingLicence GetDrivingLicense(Owner owner)
        {
            DrivingLicence zeus3deDrivingLicense = new DrivingLicence();

            if (!String.IsNullOrEmpty(owner.DriversLicense))
            {
                zeus3deDrivingLicense.number = owner.DriversLicense.EmptytoNull();
                zeus3deDrivingLicense.country = string.IsNullOrEmpty(owner.Country) ? "US" : owner.Country;
                zeus3deDrivingLicense.countrySubdivision = owner.DriversLicenseState.EmptytoNull();
                zeus3deDrivingLicense.issuingAuthority = "USDRIVINGAUTHORITY";

                if (owner.DriversLicenseExp > DateTime.MinValue)
                {
                    zeus3deDrivingLicense.expireDate = new Date();
                    zeus3deDrivingLicense.expireDate.day = owner.DriversLicenseExp.Day.EmptytoNull();
                    zeus3deDrivingLicense.expireDate.month = owner.DriversLicenseExp.Month.EmptytoNull();
                    zeus3deDrivingLicense.expireDate.year = owner.DriversLicenseExp.Year.EmptytoNull();
                }
            }

            return zeus3deDrivingLicense;

        }

        private static InternationalPassport GetPassportInformation(Owner owner)
        {
            InternationalPassport zeus3deownerPassport = new InternationalPassport();

            zeus3deownerPassport.number = owner.PassportNumber.EmptytoNull();
            zeus3deownerPassport.countryOfissue = string.IsNullOrEmpty(owner.IDNationality) ? "US" : owner.IDNationality;

            return zeus3deownerPassport;
        }

        private static Zeus3DEData Getzeus3deData(int zid)
        {
            return MerchantFacade.GetZeus3DEData(zid);
        }

        private static string EmptytoNull(this object Value)
        {
            if (Value != null)
            {
                return Value == "" ? null : Value.ToString().RemoveTab();
            }
            return null;
        }

        private static string RemoveTab(this string Value)
        {
            return Value.Replace("\t", "");
        }

        private static string RemoveSpecialChars(this string Value)
        {
            return Regex.Replace(Value, @"\t|\n|\r", "");
        }
    }
}