using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ZeusWeb.Class
{
    public class Validate3DEVendorsData
    {
        #region Properties
        private static IList<string> validationStatus = new List<string>();
        #endregion Properties

        #region Constructor
        public Validate3DEVendorsData()
        {
            validationStatus.Clear();
        }

        #endregion Constructor

        #region Validation Methods
        internal IList<string> ValidateZeus3DEData(MerchantApp app)
        {
            int buzTaxID = -1;
            Zeus3DEData zeus3DEData = MerchantFacade.GetZeus3DEData(Convert.ToInt32(app.ID));

            string businessTaxID = CommonUtility.Util.GetNumbersFromString(app.BusinessTaxID);

            //Validate Business Information
            if ((!int.TryParse(businessTaxID, out buzTaxID)) || (!businessTaxID.Length.Equals(9)) || (!ValidateRegExPattern(businessTaxID, Constants.BusinessTaxIDPattern)))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_BusinessTaxID));
            }


            //Validation & Pattern added  Business information PXP-7424 by koshlendra start 

            if (string.IsNullOrWhiteSpace(app.BusinessAddress))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_BusinessAddress));
            }

            if (string.IsNullOrWhiteSpace(app.BusinessCity))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_BusinessCity));
            }

            if (string.IsNullOrWhiteSpace(app.BusinessState) || (!ValidateRegExPattern(app.BusinessState, Constants.OwnerAddressStatePattern)))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_BusinessState));
            }
            if (string.IsNullOrWhiteSpace(app.BusinessZip) || (!ValidateRegExPattern(app.BusinessZip, Constants.OwnerAddressZipPattern)))
            {
                validationStatus.Add("Please enter a valid 5 digit Business Zip Code.");
            }

            if (!string.IsNullOrWhiteSpace(app.BusinessDBAPhone) && CommonUtility.Util.GetNumbersFromString(app.BusinessDBAPhone).Length == 10 && (!ValidateRegExPattern(CommonUtility.Util.GetNumbersFromString(app.BusinessDBAPhone), Constants.OwnerHomePhonePattern)))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_BusinessDBAPhone));
            }
          //code updated for PXP-8073 by koshlendra start
            if (string.IsNullOrWhiteSpace(app.MerchantAppTypeUID))
            { 
                 validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_MerchantAppTypeUID));
            } 
            else if(!FormHandler.ISMerchantAppTypeUIDForAcquirerID(app.MerchantAppTypeUID))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_AcquirerID));                
            }
            //code updated for PXP-8073 by koshlendra end
            //Validation & Pattern added  Business information PXP-7424 by koshlendra end  

            //Validate NDX Days
            if (zeus3DEData.NDXDay <= 0)
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_NDXDays));
            }

            //Validate Monthly Volume
            //if (app.TinfoAverageMonthlyVMCVolume <= 0)
            //{
            //    validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_MonthlyVolume));
            //}

            //Validate Average Ticket
            //if (zeus3DEData.AvgTicket <= 0)
            //{
            //    validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_AverageTicket));
            //}

            //Validate High Ticket
            //if (zeus3DEData.HighTicket <= 0)
            //{
            //    validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_HighTicket));
            //}
            //Niranjan :PXP-4989 Zeus: Add fields in 'Transaction Information' section on Fees Page.
            //PXP-7152 abarua
            //PXP-18631: Code Change : Start
            Double totalSalesType = 0;  // Added code by anuj for PXP-10309 to BBVA bank
            if (UserSessions.CurrentMerchantApp != null)
            {
                if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
                {
                    totalSalesType = Convert.ToDouble(app.TinfoStoreFrontSwipedPercent) + Convert.ToDouble(app.TinfoInterntPercent) + Convert.ToDouble(app.TinfoMailOrderPercent) + Convert.ToDouble(app.TinfoOffPremisePercent) + Convert.ToDouble(app.TinfoTradeShowPercent) + Convert.ToDouble(app.TinfoOtherPercent);
                }
                else
                {
                    totalSalesType = Convert.ToDouble(app.TinfoStoreFrontSwipedPercent) + Convert.ToDouble(app.TinfoInterntPercent) + Convert.ToDouble(app.TinfoMailOrderPercent) + Convert.ToDouble(app.TinfoTelephoneOrderPercent);
                }
            }
            //PXP-18631: Code Change : End
            
            Double totalTransCompleted = Convert.ToDouble(app.TinfoElectronicDataCaptureSwipedPercent) + Convert.ToDouble(app.TinfoManualEntryWithImprintPercent) + Convert.ToDouble(app.TinfoManualEntryNoCardNoImprintPercent) + Convert.ToDouble(app.TinfoVoiceAuthPercent);

            //Validate totaled Transaction Type Percentage 
            if (!totalSalesType.Equals(100))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_TotalTransactionTypePercentage));
            }

            //Validate totaled Transaction Completed (CNP and CP) percentage
            if (!totalTransCompleted.Equals(100))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_TotalTransactionCompletedPercentage));
            }

            //Validate for Transaction Type "Internet" >= 1%
            if ((Convert.ToDouble(app.TinfoInterntPercent) >= 1) && (string.IsNullOrEmpty(app.BusinessWebsite)))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_WebSite));
            }

            // Validate Owners Information
            ValidateOwners(app);
            ValidateOwnerSequentialOrder(app);

            //Validate Bank Information
            ValidateBankInformation(app);

            return validationStatus;
        }

        

        internal void ValidateOwners(MerchantApp app)
        {
            int counter = 1;
            bool isAtLeastOneOwner = false; 
            if (app.Owners.Count > 0)
            {
                foreach (Owner owner in app.Owners)
                {
                    
                    if ((!string.IsNullOrWhiteSpace(owner.FirstName)) || (!string.IsNullOrWhiteSpace(owner.LastName)) || (!string.IsNullOrWhiteSpace(owner.HomePhone)))
                    {
                        isAtLeastOneOwner = true;
                        if (string.IsNullOrWhiteSpace(owner.FirstName))
                        {
                            validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_FirstName) + counter.ToString());
                        }
                        if (string.IsNullOrWhiteSpace(owner.LastName))
                        {
                            validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_LastName) + counter.ToString());
                        }
                        //Owner Phone Pattern added for PXP-7424 by koshlendra start
                        if (!string.IsNullOrWhiteSpace(owner.HomePhone) && (!ValidateRegExPattern(CommonUtility.Util.GetNumbersFromString(owner.HomePhone), Constants.OwnerHomePhonePattern)))
                        {
                            validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_BusPhone) + counter.ToString());
                        }
                        //Owner Phone Patern added for PXP-7424 by koshlendra end
                        if (string.IsNullOrWhiteSpace(owner.City))
                        {
                            validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_City) + counter.ToString());
                        }
                        //Address State Patern added for PXP-7424 by koshlendra start
                        if (!string.IsNullOrWhiteSpace(owner.State) && (!ValidateRegExPattern(owner.State, Constants.OwnerAddressStatePattern)))
                        {
                            validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_State) + counter.ToString());
                        }
                        //Address State Patern added for PXP-7424 by koshlendra end
                        //Address Zip Patern added for PXP-7424 by koshlendra start
                        if (string.IsNullOrWhiteSpace(owner.Zip) || (!ValidateRegExPattern(owner.Zip, Constants.OwnerAddressZipPattern)))
                        {
                            validationStatus.Add("Please enter a valid 5 digit Postal Code for owner:" + counter.ToString());
                        }
                        //Address Zip Patern added for PXP-7424 by koshlendra end

                        //Validation & Pattern added for PXP-7424 by koshlendra start 
                        if (string.IsNullOrWhiteSpace(owner.DOB.ToString()) || owner.DOB.ToShortDateString() == "1/1/0001" || (owner.DOB.Day.ToString().Length > 2) || (owner.DOB.Month.ToString().Length > 2) || (owner.DOB.Year.ToString().Length > 4))
                        {
                            validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_DOB) + counter.ToString());
                        }
                        if (string.IsNullOrWhiteSpace(owner.Address1))
                        {
                            validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_Address1) + counter.ToString());
                        }
                        if (!string.IsNullOrWhiteSpace(owner.DriversLicenseState) && (!ValidateRegExPattern(owner.DriversLicenseState, Constants.OwnerAddressStatePattern)))
                        {
                            validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_DrivingLicenseState) + counter.ToString());
                        }
                        //Validation & Pattern added for PXP-7424 by koshlendra start 
                    }
                    counter++;
                }
            }

            if ((app.Owners.Count <= 0) || (counter > 1 && !isAtLeastOneOwner))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Owner_Info));
            }
        }
 

        internal void ValidateBankInformation(MerchantApp app)
        {
            int bankRoutingNumber = -1;
            //Validate Bank Routing Number
            if ((!int.TryParse(app.RoutingNumber, out bankRoutingNumber)) || (!app.RoutingNumber.Length.Equals(9)) || (!ValidateRegExPattern(app.RoutingNumber, Constants.BankRoutingNumberPattern)))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_BankRoutingNumber));
            }

            //Validate Bank Account Number
            //https://stackoverflow.com/questions/1787285/regular-expression-for-bank-account-number
            //The US doesn't conform to IBAN standards for account numbers; AFAIK there is no definitive US standard for account numbers, just for routing numbers.
            if (string.IsNullOrEmpty(app.AccountNumber))
            {
                validationStatus.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_BankAccountNumber));
            }

        }


        /// <summary>
        /// This method validates if the owners are filled sequentially or not.
        /// </summary>
        /// <param name="app"></param>
        internal void ValidateOwnerSequentialOrder(MerchantApp app)
        {
            int prev_OwnerCount = 0;    //As first owner will always get validated so start validation for previous owners after the first owner

            if (app.Owners.Count > 0)
            {
                for (int ownerCount = 0; ownerCount < app.Owners.Count; ownerCount++)
                {
                    if ((!string.IsNullOrWhiteSpace(app.Owners[ownerCount].FirstName)) || (!string.IsNullOrWhiteSpace(app.Owners[ownerCount].LastName)))
                    {
                        for (int prevOwnerCount = prev_OwnerCount; prevOwnerCount < ownerCount; prevOwnerCount++)//Validate owners in sequence before this owner if they are not filled.
                        {
                            if ((!string.IsNullOrWhiteSpace(app.Owners[prevOwnerCount].FirstName)) || (!string.IsNullOrWhiteSpace(app.Owners[prevOwnerCount].LastName)))
                            {
                                ValidateOwnersData(app, ref validationStatus, prevOwnerCount);
                            }
                        }
                        prev_OwnerCount = ownerCount;
                    }
                }
            }
        }


        /// <summary> PXP-4113 RThakur 
        /// This method validates the Owners data.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="validationMsgs"></param>
        /// <param name="ownerCount"></param>
        internal static void ValidateOwnersData(MerchantApp app, ref IList<string> validationMsgs, int ownerCount)
        {
            string msg = string.Empty;
            if (string.IsNullOrWhiteSpace(app.Owners[ownerCount].FirstName) || string.IsNullOrWhiteSpace(app.Owners[ownerCount].LastName))
            {
                msg = Constants.GetDescription(Constants.ErrorCodes.Owner_SequentialOrder);
                if (!string.IsNullOrEmpty(msg) && !validationMsgs.Contains(msg))
                    validationMsgs.Add(msg);
            }
        }

        #endregion Validation Methods

        #region Helper Methods


        internal bool ValidateRegExPattern(string input, string regExPattern)
        {
            Regex regex = new Regex(regExPattern);
            return regex.IsMatch(input);
        }

        #endregion Helper Methods

    }
}