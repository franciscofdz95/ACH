using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeusWeb.Class
{
    public class WebUtilMerchantProcessingFee
    {
        //Please watch while you change the name of the proerty in this file. 
        //These names are tightly coupled to the IDs of the controls.
        //The nameing of the properties is done in a way that follow the  rule (MerchantProcessingFee.cs_MerchantCardFee.cs)

        //BatchTypeName
        public int VISACredit_BatchTypeID { get; set; }
        public int VISADebit_BatchTypeID { get; set; }
        public int VISAElectronDebit_BatchTypeID { get; set; }
        public int MasterCardDebit_BatchTypeID { get; set; }
        public int MasterCardCredit_BatchTypeID { get; set; }
        public int MaestroDebit_BatchTypeID { get; set; }
        public int AmexCredit_BatchTypeID { get; set; }
        public int AmexDebit_BatchTypeID { get; set; }
        public int AmexOPCredit_BatchTypeID { get; set; }
        public int AmexOPDebit_BatchTypeID { get; set; }
        public int AmexOBCredit_BatchTypeID { get; set; }
        public int AmexOBDebit_BatchTypeID { get; set; }
        public int DiscoverCredit_BatchTypeID { get; set; }
        public int DiscoverDebit_BatchTypeID { get; set; }
        public int DinersClubCredit_BatchTypeID { get; set; }
        public int JCBCredit_BatchTypeID { get; set; }

        //DiscountTypeName
        public int VISACredit_PricingTypeID { get; set; }
        public int VISADebit_PricingTypeID { get; set; }
        public int VISAElectronDebit_PricingTypeID { get; set; }
        public int MasterCardDebit_PricingTypeID { get; set; }
        public int MasterCardCredit_PricingTypeID { get; set; }
        public int MaestroDebit_PricingTypeID { get; set; }
        public int AmexCredit_PricingTypeID { get; set; }
        public int AmexDebit_PricingTypeID { get; set; }
        public int AmexOPCredit_PricingTypeID { get; set; }
        public int AmexOPDebit_PricingTypeID { get; set; }
        public int AmexOBCredit_PricingTypeID { get; set; }
        public int AmexOBDebit_PricingTypeID { get; set; }
        public int DiscoverCredit_PricingTypeID { get; set; }
        public int DiscoverDebit_PricingTypeID { get; set; }
        public int DinersClubCredit_PricingTypeID { get; set; }
        public int JCBCredit_PricingTypeID { get; set; }

        //TieredDiscount Q
        public decimal VISACredit_DiscountQual { get; set; }
        public decimal VISADebit_DiscountQual { get; set; }
        public decimal VISAElectronDebit_DiscountQual { get; set; }
        public decimal MasterCardDebit_DiscountQual { get; set; }
        public decimal MasterCardCredit_DiscountQual { get; set; }
        public decimal MaestroDebit_DiscountQual { get; set; }
        public decimal AmexCredit_DiscountQual { get; set; }
        public decimal AmexDebit_DiscountQual { get; set; }
        public decimal AmexOPCredit_DiscountQual { get; set; }
        public decimal AmexOPDebit_DiscountQual { get; set; }
        public decimal AmexOBCredit_DiscountQual { get; set; }
        public decimal AmexOBDebit_DiscountQual { get; set; }
        public decimal DiscoverCredit_DiscountQual { get; set; }
        public decimal DiscoverDebit_DiscountQual { get; set; }
        public decimal DinersClubCredit_DiscountQual { get; set; }
        public decimal JCBCredit_DiscountQual { get; set; }


        //TieredDiscount M
        public decimal VISACredit_DiscountMidQual { get; set; }
        public decimal VISADebit_DiscountMidQual { get; set; }
        public decimal VISAElectronDebit_DiscountMidQual { get; set; }
        public decimal MasterCardDebit_DiscountMidQual { get; set; }
        public decimal MasterCardCredit_DiscountMidQual { get; set; }
        public decimal MaestroDebit_DiscountMidQual { get; set; }
        public decimal AmexCredit_DiscountMidQual { get; set; }
        public decimal AmexDebit_DiscountMidQual { get; set; }
        public decimal AmexOPCredit_DiscountMidQual { get; set; }
        public decimal AmexOPDebit_DiscountMidQual { get; set; }
        public decimal AmexOBCredit_DiscountMidQual { get; set; }
        public decimal AmexOBDebit_DiscountMidQual { get; set; }
        public decimal DiscoverCredit_DiscountMidQual { get; set; }
        public decimal DiscoverDebit_DiscountMidQual { get; set; }
        public decimal DinersClubCredit_DiscountMidQual { get; set; }
        public decimal JCBCredit_DiscountMidQual { get; set; }

        //TieredDiscount N
        public decimal VISACredit_DiscountNonQual { get; set; }
        public decimal VISADebit_DiscountNonQual { get; set; }
        public decimal VISAElectronDebit_DiscountNonQual { get; set; }
        public decimal MasterCardDebit_DiscountNonQual { get; set; }
        public decimal MasterCardCredit_DiscountNonQual { get; set; }
        public decimal MaestroDebit_DiscountNonQual { get; set; }
        public decimal AmexCredit_DiscountNonQual { get; set; }
        public decimal AmexDebit_DiscountNonQual { get; set; }
        public decimal AmexOPCredit_DiscountNonQual { get; set; }
        public decimal AmexOPDebit_DiscountNonQual { get; set; }
        public decimal AmexOBCredit_DiscountNonQual { get; set; }
        public decimal AmexOBDebit_DiscountNonQual { get; set; }
        public decimal DiscoverCredit_DiscountNonQual { get; set; }
        public decimal DiscoverDebit_DiscountNonQual { get; set; }
        public decimal DinersClubCredit_DiscountNonQual { get; set; }
        public decimal JCBCredit_DiscountNonQual { get; set; }


        ////Blended Discount
        //public decimal VISACredit_BlendedDiscount { get; set; }
        //public decimal VISADebit_BlendedDiscount { get; set; }
        //public decimal VISAElectronDebit_BlendedDiscount { get; set; }
        //public decimal MasterCardDebit_BlendedDiscount { get; set; }
        //public decimal MasterCardCredit_BlendedDiscount { get; set; }
        //public decimal MaestroDebit_BlendedDiscount { get; set; }
        //public decimal AmexCredit_BlendedDiscount { get; set; }
        //public decimal AmexDebit_BlendedDiscount { get; set; }
        //public decimal DiscoverCredit_BlendedDiscount { get; set; }
        //public decimal DiscoverDebit_BlendedDiscount { get; set; }
        //public decimal DinersClubCredit_BlendedDiscount { get; set; }
        //public decimal JCBCredit_BlendedDiscount { get; set; }

        ////InterchangePlus Discount
        //public decimal VISACredit_InterchangePlusDiscount { get; set; }
        //public decimal VISADebit_InterchangePlusDiscount { get; set; }
        //public decimal VISAElectronDebit_InterchangePlusDiscount { get; set; }
        //public decimal MasterCardDebit_InterchangePlusDiscount { get; set; }
        //public decimal MasterCardCredit_InterchangePlusDiscount { get; set; }
        //public decimal MaestroDebit_InterchangePlusDiscount { get; set; }
        //public decimal AmexCredit_InterchangePlusDiscount { get; set; }
        //public decimal AmexDebit_InterchangePlusDiscount { get; set; }
        //public decimal DiscoverCredit_InterchangePlusDiscount { get; set; }
        //public decimal DiscoverDebit_InterchangePlusDiscount { get; set; }
        //public decimal DinersClubCredit_InterchangePlusDiscount { get; set; }
        //public decimal JCBCredit_InterchangePlusDiscount { get; set; }

        ////ERRDiscount Q
        //public decimal VISACredit_ERRDiscountQ { get; set; }
        //public decimal VISADebit_ERRDiscountQ { get; set; }
        //public decimal VISAElectronDebit_ERRDiscountQ { get; set; }
        //public decimal MasterCardDebit_ERRDiscountQ { get; set; }
        //public decimal MasterCardCredit_ERRDiscountQ { get; set; }
        //public decimal MaestroDebit_ERRDiscountQ { get; set; }
        //public decimal AmexCredit_ERRDiscountQ { get; set; }
        //public decimal AmexDebit_ERRDiscountQ { get; set; }
        //public decimal DiscoverCredit_ERRDiscountQ { get; set; }
        //public decimal DiscoverDebit_ERRDiscountQ { get; set; }
        //public decimal DinersClubCredit_ERRDiscountQ { get; set; }
        //public decimal JCBCredit_ERRDiscountQ { get; set; }

        ////ERRDiscount N
        //public decimal VISACredit_ERRDiscountN { get; set; }
        //public decimal VISADebit_ERRDiscountN { get; set; }
        //public decimal VISAElectronDebit_ERRDiscountN { get; set; }
        //public decimal MasterCardDebit_ERRDiscountN { get; set; }
        //public decimal MasterCardCredit_ERRDiscountN { get; set; }
        //public decimal MaestroDebit_ERRDiscountN { get; set; }
        //public decimal AmexCredit_ERRDiscountN { get; set; }
        //public decimal AmexDebit_ERRDiscountN { get; set; }
        //public decimal DiscoverCredit_ERRDiscountN { get; set; }
        //public decimal DiscoverDebit_ERRDiscountN { get; set; }
        //public decimal DinersClubCredit_ERRDiscountN { get; set; }
        //public decimal JCBCredit_ERRDiscountN { get; set; }

        //AuthApproved
        public decimal VISACredit_AuthApproved { get; set; }
        public decimal VISADebit_AuthApproved { get; set; }
        public decimal VISAElectronDebit_AuthApproved { get; set; }
        public decimal MasterCardDebit_AuthApproved { get; set; }
        public decimal MasterCardCredit_AuthApproved { get; set; }
        public decimal MaestroDebit_AuthApproved { get; set; }
        public decimal AmexCredit_AuthApproved { get; set; }
        public decimal AmexDebit_AuthApproved { get; set; }
        public decimal AmexOPCredit_AuthApproved { get; set; }
        public decimal AmexOPDebit_AuthApproved { get; set; }
        public decimal AmexOBCredit_AuthApproved { get; set; }
        public decimal AmexOBDebit_AuthApproved { get; set; }
        public decimal DiscoverCredit_AuthApproved { get; set; }
        public decimal DiscoverDebit_AuthApproved { get; set; }
        public decimal DinersClubCredit_AuthApproved { get; set; }
        public decimal JCBCredit_AuthApproved { get; set; }

        //AuthReversal
        public decimal VISACredit_AuthReversal { get; set; }
        public decimal VISADebit_AuthReversal { get; set; }
        public decimal VISAElectronDebit_AuthReversal { get; set; }
        public decimal MasterCardDebit_AuthReversal { get; set; }
        public decimal MasterCardCredit_AuthReversal { get; set; }
        public decimal MaestroDebit_AuthReversal { get; set; }
        public decimal AmexCredit_AuthReversal { get; set; }
        public decimal AmexDebit_AuthReversal { get; set; }
        public decimal AmexOPCredit_AuthReversal { get; set; }
        public decimal AmexOPDebit_AuthReversal { get; set; }
        public decimal AmexOBCredit_AuthReversal { get; set; }
        public decimal AmexOBDebit_AuthReversal { get; set; }
        public decimal DiscoverCredit_AuthReversal { get; set; }
        public decimal DiscoverDebit_AuthReversal { get; set; }
        public decimal DinersClubCredit_AuthReversal { get; set; }
        public decimal JCBCredit_AuthReversal { get; set; }


        //FailedRequest
        public decimal VISACredit_FailedRequests { get; set; }
        public decimal VISADebit_FailedRequests { get; set; }
        public decimal VISAElectronDebit_FailedRequests { get; set; }
        public decimal MasterCardDebit_FailedRequests { get; set; }
        public decimal MasterCardCredit_FailedRequests { get; set; }
        public decimal MaestroDebit_FailedRequests { get; set; }
        public decimal AmexCredit_FailedRequests { get; set; }
        public decimal AmexDebit_FailedRequests { get; set; }
        public decimal AmexOPCredit_FailedRequests { get; set; }
        public decimal AmexOPDebit_FailedRequests { get; set; }
        public decimal AmexOBCredit_FailedRequests { get; set; }
        public decimal AmexOBDebit_FailedRequests { get; set; }
        public decimal DiscoverCredit_FailedRequests { get; set; }
        public decimal DiscoverDebit_FailedRequests { get; set; }
        public decimal DinersClubCredit_FailedRequests { get; set; }
        public decimal JCBCredit_FailedRequests { get; set; }

        //TDSEnrollment
        public decimal VISACredit_TDSEnrollments { get; set; }
        public decimal VISADebit_TDSEnrollments { get; set; }
        public decimal VISAElectronDebit_TDSEnrollments { get; set; }
        public decimal MasterCardDebit_TDSEnrollments { get; set; }
        public decimal MasterCardCredit_TDSEnrollments { get; set; }
        public decimal MaestroDebit_TDSEnrollments { get; set; }
        public decimal AmexCredit_TDSEnrollments { get; set; }
        public decimal AmexDebit_TDSEnrollments { get; set; }
        public decimal AmexOPCredit_TDSEnrollments { get; set; }
        public decimal AmexOPDebit_TDSEnrollments { get; set; }
        public decimal AmexOBCredit_TDSEnrollments { get; set; }
        public decimal AmexOBDebit_TDSEnrollments { get; set; }
        public decimal DiscoverCredit_TDSEnrollments { get; set; }
        public decimal DiscoverDebit_TDSEnrollments { get; set; }
        public decimal DinersClubCredit_TDSEnrollments { get; set; }
        public decimal JCBCredit_TDSEnrollments { get; set; }

        //Settlement Completed Qualified
        public decimal VISACredit_SettlementCompletedQual{ get; set; }
        public decimal VISADebit_SettlementCompletedQual{ get; set; }
        public decimal VISAElectronDebit_SettlementCompletedQual{ get; set; }
        public decimal MasterCardDebit_SettlementCompletedQual{ get; set; }
        public decimal MasterCardCredit_SettlementCompletedQual{ get; set; }
        public decimal MaestroDebit_SettlementCompletedQual{ get; set; }
        public decimal AmexCredit_SettlementCompletedQual{ get; set; }
        public decimal AmexDebit_SettlementCompletedQual{ get; set; }
        public decimal AmexOPCredit_SettlementCompletedQual { get; set; }
        public decimal AmexOPDebit_SettlementCompletedQual { get; set; }
        public decimal AmexOBCredit_SettlementCompletedQual { get; set; }
        public decimal AmexOBDebit_SettlementCompletedQual { get; set; }
        public decimal DiscoverCredit_SettlementCompletedQual{ get; set; }
        public decimal DiscoverDebit_SettlementCompletedQual{ get; set; }
        public decimal DinersClubCredit_SettlementCompletedQual{ get; set; }
        public decimal JCBCredit_SettlementCompletedQual{ get; set; }

        //Settlement Completed Mid
        public decimal VISACredit_SettlementCompletedMidQual{ get; set; }
        public decimal VISADebit_SettlementCompletedMidQual{ get; set; }
        public decimal VISAElectronDebit_SettlementCompletedMidQual{ get; set; }
        public decimal MasterCardDebit_SettlementCompletedMidQual{ get; set; }
        public decimal MasterCardCredit_SettlementCompletedMidQual{ get; set; }
        public decimal MaestroDebit_SettlementCompletedMidQual{ get; set; }
        public decimal AmexCredit_SettlementCompletedMidQual{ get; set; }
        public decimal AmexDebit_SettlementCompletedMidQual{ get; set; }
        public decimal AmexOPCredit_SettlementCompletedMidQual { get; set; }
        public decimal AmexOPDebit_SettlementCompletedMidQual { get; set; }
        public decimal AmexOBCredit_SettlementCompletedMidQual { get; set; }
        public decimal AmexOBDebit_SettlementCompletedMidQual { get; set; }
        public decimal DiscoverCredit_SettlementCompletedMidQual{ get; set; }
        public decimal DiscoverDebit_SettlementCompletedMidQual{ get; set; }
        public decimal DinersClubCredit_SettlementCompletedMidQual{ get; set; }
        public decimal JCBCredit_SettlementCompletedMidQual{ get; set; }


        //Settlement Completed Non
        public decimal VISACredit_SettlementCompletedNonQual { get; set; }
        public decimal VISADebit_SettlementCompletedNonQual { get; set; }
        public decimal VISAElectronDebit_SettlementCompletedNonQual { get; set; }
        public decimal MasterCardDebit_SettlementCompletedNonQual { get; set; }
        public decimal MasterCardCredit_SettlementCompletedNonQual { get; set; }
        public decimal MaestroDebit_SettlementCompletedNonQual { get; set; }
        public decimal AmexCredit_SettlementCompletedNonQual { get; set; }
        public decimal AmexDebit_SettlementCompletedNonQual { get; set; }
        public decimal AmexOPCredit_SettlementCompletedNonQual { get; set; }
        public decimal AmexOPDebit_SettlementCompletedNonQual { get; set; }
        public decimal AmexOBCredit_SettlementCompletedNonQual { get; set; }
        public decimal AmexOBDebit_SettlementCompletedNonQual { get; set; }
        public decimal DiscoverCredit_SettlementCompletedNonQual { get; set; }
        public decimal DiscoverDebit_SettlementCompletedNonQual { get; set; }
        public decimal DinersClubCredit_SettlementCompletedNonQual { get; set; }
        public decimal JCBCredit_SettlementCompletedNonQual { get; set; }

        //Settlement Cancelled
        public decimal VISACredit_SettlementCancelled { get; set; }
        public decimal VISADebit_SettlementCancelled { get; set; }
        public decimal VISAElectronDebit_SettlementCancelled { get; set; }
        public decimal MasterCardDebit_SettlementCancelled { get; set; }
        public decimal MasterCardCredit_SettlementCancelled { get; set; }
        public decimal MaestroDebit_SettlementCancelled { get; set; }
        public decimal AmexCredit_SettlementCancelled { get; set; }
        public decimal AmexDebit_SettlementCancelled { get; set; }
        public decimal AmexOPCredit_SettlementCancelled { get; set; }
        public decimal AmexOPDebit_SettlementCancelled { get; set; }
        public decimal AmexOBCredit_SettlementCancelled { get; set; }
        public decimal AmexOBDebit_SettlementCancelled { get; set; }
        public decimal DiscoverCredit_SettlementCancelled { get; set; }
        public decimal DiscoverDebit_SettlementCancelled { get; set; }
        public decimal DinersClubCredit_SettlementCancelled { get; set; }
        public decimal JCBCredit_SettlementCancelled { get; set; }

        //Credit Completed
        public decimal VISACredit_CreditCompleted { get; set; }
        public decimal VISADebit_CreditCompleted { get; set; }
        public decimal VISAElectronDebit_CreditCompleted { get; set; }
        public decimal MasterCardDebit_CreditCompleted { get; set; }
        public decimal MasterCardCredit_CreditCompleted { get; set; }
        public decimal MaestroDebit_CreditCompleted { get; set; }
        public decimal AmexCredit_CreditCompleted { get; set; }
        public decimal AmexDebit_CreditCompleted { get; set; }
        public decimal AmexOPCredit_CreditCompleted { get; set; }
        public decimal AmexOPDebit_CreditCompleted { get; set; }
        public decimal AmexOBCredit_CreditCompleted { get; set; }
        public decimal AmexOBDebit_CreditCompleted { get; set; }
        public decimal DiscoverCredit_CreditCompleted { get; set; }
        public decimal DiscoverDebit_CreditCompleted { get; set; }
        public decimal DinersClubCredit_CreditCompleted { get; set; }
        public decimal JCBCredit_CreditCompleted { get; set; }

        //Credit Cancelled
        public decimal VISACredit_CreditCancelled { get; set; }
        public decimal VISADebit_CreditCancelled { get; set; }
        public decimal VISAElectronDebit_CreditCancelled { get; set; }
        public decimal MasterCardDebit_CreditCancelled { get; set; }
        public decimal MasterCardCredit_CreditCancelled { get; set; }
        public decimal MaestroDebit_CreditCancelled { get; set; }
        public decimal AmexCredit_CreditCancelled { get; set; }
        public decimal AmexDebit_CreditCancelled { get; set; }
        public decimal AmexOPCredit_CreditCancelled { get; set; }
        public decimal AmexOPDebit_CreditCancelled { get; set; }
        public decimal AmexOBCredit_CreditCancelled { get; set; }
        public decimal AmexOBDebit_CreditCancelled { get; set; }
        public decimal DiscoverCredit_CreditCancelled { get; set; }
        public decimal DiscoverDebit_CreditCancelled { get; set; }
        public decimal DinersClubCredit_CreditCancelled { get; set; }
        public decimal JCBCredit_CreditCancelled { get; set; }

        //Payment Completed
        public decimal VISACredit_PaymentCompleted { get; set; }
        public decimal VISADebit_PaymentCompleted { get; set; }
        public decimal VISAElectronDebit_PaymentCompleted { get; set; }
        public decimal MasterCardDebit_PaymentCompleted { get; set; }
        public decimal MasterCardCredit_PaymentCompleted { get; set; }
        public decimal MaestroDebit_PaymentCompleted { get; set; }
        public decimal AmexCredit_PaymentCompleted { get; set; }
        public decimal AmexDebit_PaymentCompleted { get; set; }
        public decimal AmexOPCredit_PaymentCompleted { get; set; }
        public decimal AmexOPDebit_PaymentCompleted { get; set; }
        public decimal AmexOBCredit_PaymentCompleted { get; set; }
        public decimal AmexOBDebit_PaymentCompleted { get; set; }
        public decimal DiscoverCredit_PaymentCompleted { get; set; }
        public decimal DiscoverDebit_PaymentCompleted { get; set; }
        public decimal DinersClubCredit_PaymentCompleted { get; set; }
        public decimal JCBCredit_PaymentCompleted { get; set; }


        //Payment Cancelled
        public decimal VISACredit_PaymentCancelled { get; set; }
        public decimal VISADebit_PaymentCancelled { get; set; }
        public decimal VISAElectronDebit_PaymentCancelled { get; set; }
        public decimal MasterCardDebit_PaymentCancelled { get; set; }
        public decimal MasterCardCredit_PaymentCancelled { get; set; }
        public decimal MaestroDebit_PaymentCancelled { get; set; }
        public decimal AmexCredit_PaymentCancelled { get; set; }
        public decimal AmexDebit_PaymentCancelled { get; set; }
        public decimal AmexOPCredit_PaymentCancelled { get; set; }
        public decimal AmexOPDebit_PaymentCancelled { get; set; }
        public decimal AmexOBCredit_PaymentCancelled { get; set; }
        public decimal AmexOBDebit_PaymentCancelled { get; set; }
        public decimal DiscoverCredit_PaymentCancelled { get; set; }
        public decimal DiscoverDebit_PaymentCancelled { get; set; }
        public decimal DinersClubCredit_PaymentCancelled { get; set; }
        public decimal JCBCredit_PaymentCancelled { get; set; }

        //Payment Declined
        public decimal VISACredit_PaymentDeclined { get; set; }
        public decimal VISADebit_PaymentDeclined { get; set; }
        public decimal VISAElectronDebit_PaymentDeclined { get; set; }
        public decimal MasterCardDebit_PaymentDeclined { get; set; }
        public decimal MasterCardCredit_PaymentDeclined { get; set; }
        public decimal MaestroDebit_PaymentDeclined { get; set; }
        public decimal AmexCredit_PaymentDeclined { get; set; }
        public decimal AmexDebit_PaymentDeclined { get; set; }
        public decimal AmexOPCredit_PaymentDeclined { get; set; }
        public decimal AmexOPDebit_PaymentDeclined { get; set; }
        public decimal AmexOBCredit_PaymentDeclined { get; set; }
        public decimal AmexOBDebit_PaymentDeclined { get; set; }
        public decimal DiscoverCredit_PaymentDeclined { get; set; }
        public decimal DiscoverDebit_PaymentDeclined { get; set; }
        public decimal DinersClubCredit_PaymentDeclined { get; set; }
        public decimal JCBCredit_PaymentDeclined { get; set; }


        //Chargebacks
        public decimal VISACredit_Chargebacks { get; set; }
        public decimal VISADebit_Chargebacks { get; set; }
        public decimal VISAElectronDebit_Chargebacks { get; set; }
        public decimal MasterCardDebit_Chargebacks { get; set; }
        public decimal MasterCardCredit_Chargebacks { get; set; }
        public decimal MaestroDebit_Chargebacks { get; set; }
        public decimal AmexCredit_Chargebacks { get; set; }
        public decimal AmexDebit_Chargebacks { get; set; }
        public decimal AmexOPCredit_Chargebacks { get; set; }
        public decimal AmexOPDebit_Chargebacks { get; set; }
        public decimal AmexOBCredit_Chargebacks { get; set; }
        public decimal AmexOBDebit_Chargebacks { get; set; }
        public decimal DiscoverCredit_Chargebacks { get; set; }
        public decimal DiscoverDebit_Chargebacks { get; set; }
        public decimal DinersClubCredit_Chargebacks { get; set; }
        public decimal JCBCredit_Chargebacks { get; set; }
        //PXP-4983
        //SettlementFee  
        public decimal VISACredit_SettlementFee { get; set; }
        public decimal VISADebit_SettlementFee { get; set; }
        public decimal VISAElectronDebit_SettlementFee { get; set; }
        public decimal MasterCardDebit_SettlementFee { get; set; }
        public decimal MasterCardCredit_SettlementFee { get; set; }
        public decimal MaestroDebit_SettlementFee { get; set; }
        public decimal AmexCredit_SettlementFee { get; set; }
        public decimal AmexDebit_SettlementFee { get; set; }
        public decimal AmexOPCredit_SettlementFee { get; set; }
        public decimal AmexOPDebit_SettlementFee { get; set; }
        public decimal AmexOBCredit_SettlementFee { get; set; }
        public decimal AmexOBDebit_SettlementFee { get; set; }
        public decimal DiscoverCredit_SettlementFee { get; set; }
        public decimal DiscoverDebit_SettlementFee { get; set; }
        public decimal DinersClubCredit_SettlementFee { get; set; }
        public decimal JCBCredit_SettlementFee { get; set; }


        //Reversal
        public decimal VISACredit_Reversal { get; set; }
        public decimal VISADebit_Reversal { get; set; }
        public decimal VISAElectronDebit_Reversal { get; set; }
        public decimal MasterCardDebit_Reversal { get; set; }
        public decimal MasterCardCredit_Reversal { get; set; }
        public decimal MaestroDebit_Reversal { get; set; }
        public decimal AmexCredit_Reversal { get; set; }
        public decimal AmexDebit_Reversal { get; set; }
        public decimal AmexOPCredit_Reversal { get; set; }
        public decimal AmexOPDebit_Reversal { get; set; }
        public decimal AmexOBCredit_Reversal { get; set; }
        public decimal AmexOBDebit_Reversal { get; set; }
        public decimal DiscoverCredit_Reversal { get; set; }
        public decimal DiscoverDebit_Reversal { get; set; }
        public decimal DinersClubCredit_Reversal { get; set; }
        public decimal JCBCredit_Reversal { get; set; }

        //Retrieval
        public decimal VISACredit_Retrieval { get; set; }
        public decimal VISADebit_Retrieval { get; set; }
        public decimal VISAElectronDebit_Retrieval { get; set; }
        public decimal MasterCardDebit_Retrieval { get; set; }
        public decimal MasterCardCredit_Retrieval { get; set; }
        public decimal MaestroDebit_Retrieval { get; set; }
        public decimal AmexCredit_Retrieval { get; set; }
        public decimal AmexDebit_Retrieval { get; set; }
        public decimal AmexOPCredit_Retrieval { get; set; }
        public decimal AmexOPDebit_Retrieval { get; set; }
        public decimal AmexOBCredit_Retrieval { get; set; }
        public decimal AmexOBDebit_Retrieval { get; set; }
        public decimal DiscoverCredit_Retrieval { get; set; }
        public decimal DiscoverDebit_Retrieval { get; set; }
        public decimal DinersClubCredit_Retrieval { get; set; }
        public decimal JCBCredit_Retrieval { get; set; }


        //Uncollectable Insu Fee
        public decimal VISACredit_UncollectedInsurance { get; set; }
        public decimal VISADebit_UncollectedInsurance { get; set; }
        public decimal VISAElectronDebit_UncollectedInsurance { get; set; }
        public decimal MasterCardDebit_UncollectedInsurance { get; set; }
        public decimal MasterCardCredit_UncollectedInsurance { get; set; }
        public decimal MaestroDebit_UncollectedInsurance { get; set; }
        public decimal AmexCredit_UncollectedInsurance { get; set; }
        public decimal AmexDebit_UncollectedInsurance { get; set; }
        public decimal AmexOPCredit_UncollectedInsurance { get; set; }
        public decimal AmexOPDebit_UncollectedInsurance { get; set; }
        public decimal AmexOBCredit_UncollectedInsurance { get; set; }
        public decimal AmexOBDebit_UncollectedInsurance { get; set; }
        public decimal DiscoverCredit_UncollectedInsurance { get; set; }
        public decimal DiscoverDebit_UncollectedInsurance { get; set; }
        public decimal DinersClubCredit_UncollectedInsurance { get; set; }
        public decimal JCBCredit_UncollectedInsurance { get; set; }

        //MisUse Auth Fee
        public decimal VISACredit_MisUseAuth { get; set; }
        public decimal VISADebit_MisUseAuth { get; set; }
        public decimal VISAElectronDebit_MisUseAuth { get; set; }
        public decimal MasterCardDebit_MisUseAuth { get; set; }
        public decimal MasterCardCredit_MisUseAuth { get; set; }
        public decimal MaestroDebit_MisUseAuth { get; set; }
        public decimal AmexCredit_MisUseAuth { get; set; }
        public decimal AmexDebit_MisUseAuth { get; set; }
        public decimal AmexOPCredit_MisUseAuth { get; set; }
        public decimal AmexOPDebit_MisUseAuth { get; set; }
        public decimal AmexOBCredit_MisUseAuth { get; set; }
        public decimal AmexOBDebit_MisUseAuth { get; set; }
        public decimal DiscoverCredit_MisUseAuth { get; set; }
        public decimal DiscoverDebit_MisUseAuth { get; set; }
        public decimal DinersClubCredit_MisUseAuth { get; set; }
        public decimal JCBCredit_MisUseAuth { get; set; }

        //DM-5078 FraudDisputeFee
        public decimal VISACredit_FraudDisputeFee { get; set; }
        public decimal VISADebit_FraudDisputeFee { get; set; }
        public decimal VISAElectronDebit_FraudDisputeFee { get; set; }
        public decimal MasterCardDebit_FraudDisputeFee { get; set; }
        public decimal MasterCardCredit_FraudDisputeFee { get; set; }
        public decimal MaestroDebit_FraudDisputeFee { get; set; }
        public decimal AmexCredit_FraudDisputeFee { get; set; }
        public decimal AmexDebit_FraudDisputeFee { get; set; }
        public decimal AmexOPCredit_FraudDisputeFee { get; set; }
        public decimal AmexOPDebit_FraudDisputeFee { get; set; }
        public decimal AmexOBCredit_FraudDisputeFee { get; set; }
        public decimal AmexOBDebit_FraudDisputeFee { get; set; }
        public decimal DiscoverCredit_FraudDisputeFee { get; set; }
        public decimal DiscoverDebit_FraudDisputeFee { get; set; }
        public decimal DinersClubCredit_FraudDisputeFee { get; set; }
        public decimal JCBCredit_FraudDisputeFee { get; set; }
        //DM-5078 end
    }
}