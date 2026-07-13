//using System;
//using System.Data;
//using System.Configuration;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;

///// <summary>
///// Summary description for Constants
///// </summary>
//public class Constants
//{
//    public const string QueueAP = "Application Processing";
//    public const string QueueCU = "Credit Underwriting";
//    public const string QueueAB = "Application Boarding";
//    public const string QueueRK = "Risk";
//    public const string QueueDP = "Deployment";
//    public const string QueueCS = "Merchant Support";


//    public const string QueueStatus_AP_RECEIVED = "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409";
//    public const string QueueStatus_AP_PENDING = "F5FAF4FE-A132-45F6-A854-8CCFB07AA8D9";
//    public const string QueueStatus_AP_SENT_UNDERWRITING = "EFCB31E9-C314-4A59-9CC9-4A471EF524D5";
//    public const string QueueStatus_CU_RECEIVED = "87F2DFAE-B0EC-4208-83FC-C9488393AA61";
//    public const string QueueStatus_CU_PENDING = "4358B3A7-9936-448B-BEE5-FC8DB48FB9FF";
//    public const string QueueStatus_CU_APPROVED = "2FDDA5E4-E80A-4155-8CB2-D5200992FA81";
//    public const string QueueStatus_CU_DECLINED = "330B9533-CFCF-439E-A135-CE10C633611C";
//    public const string QueueStatus_CU_SENT_TO_APPLICATION_BOARDING = "06AD1C86-D5FC-4919-8B87-89C8931FB640";
//    public const string QueueStatus_AB_RECEIVED = "73FC4B27-98D4-40EA-B9FC-1370C564CB12";
//    public const string QueueStatus_AB_FILE_BUILT = "EB1DFF71-2A3F-489E-AE7B-C1A12F54CBAF";
//    public const string QueueStatus_AB_SENT_TO_DEPLOYMENT = "35C0E149-D8E2-4B59-A223-EEE410E80821";
//    public const string QueueStatus_RK_RECEIVED = "B3C5096E-36D0-4DB8-82C7-8C4EC401FBE0";
//    public const string QueueStatus_RK_IN_REVIEW = "85CC2592-74E0-461D-B089-DC96F59F7589";
//    public const string QueueStatus_RK_SENT_TO_DEPLOYMENT = "04EB3C35-2274-48A9-8752-C471A41ABB39";
//    public const string QueueStatus_DP_RECEIVED = "158F32CA-4447-48CE-9CD4-ED45514E24D8";
//    public const string QueueStatus_DP_BUILD_TERMINAL_FILE_GATEWAY = "D0554A82-EE05-44A4-9D2B-F874C5D1EA10";
//    public const string QueueStatus_DP_SCHEDULE_DOWNLOAD_TRAINING = "501FA519-AFB4-4935-8149-1929D35B8593";
//    public const string QueueStatus_DP_MERCHANT_ACTIVE = "0B38A68C-D1E9-45D9-8AE0-E098BF621BC1";
//    public const string QueueStatus_DP_SENT_TO_CLIENT_SERVICES = "72917208-AE07-42BE-A47C-C0C75A9F86A2";
//    public const string QueueStatus_CS_RECEIVED = "A4CA9897-5719-4938-95F7-FA3D7A4BFF5B";
//    public const string QueueStatus_CS_SCHEDULED_WELCOME_CALL = "F5F18138-D977-40BA-AB8C-415ABB4A0760";
//    public const string QueueStatus_CS_CONDUCTED_WELCOME_CALL = "7C4F0BB4-3E7F-4644-8734-263256BE5AC1";
//    public const string QueueStatus_CS_CANCELLATION = "1DB379A0-8F4C-4B9D-ACFC-092E6711DB88";
//    public const string QueueStatus_CS_INACTIVE = "FC2F32C1-4359-4547-A3A9-528ADFFF6928";
//    public const string QueueStatus_CS_ACTIVE = "36DB0F43-47CF-4F9B-BE79-B56A691DD8B8";




//    //public const string QueueStatus_AP_RECEIVED = "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409";
//    //public const string QueueStatus_AP_PENDING = "F5FAF4FE-A132-45F6-A854-8CCFB07AA8D9";
//    //public const string QueueStatus_AP_SENT_UNDERWRITING = "EFCB31E9-C314-4A59-9CC9-4A471EF524D5";

//    //public const string QueueStatus_CU_RECEIVED = "87F2DFAE-B0EC-4208-83FC-C9488393AA61";
//    //public const string QueueStatus_CU_PENDING = "4358B3A7-9936-448B-BEE5-FC8DB48FB9FF";
//    //public const string QueueStatus_CU_APPROVED = "2FDDA5E4-E80A-4155-8CB2-D5200992FA81";
//    //public const string QueueStatus_CU_DECLINED = "330B9533-CFCF-439E-A135-CE10C633611C";
//    //public const string QueueStatus_CU_SENT_TO_APPLICATION_BOARDING = "06AD1C86-D5FC-4919-8B87-89C8931FB640";

//    //public const string QueueStatus_DE_RECEIVED = "73FC4B27-98D4-40EA-B9FC-1370C564CB12";
//    //public const string QueueStatus_DE_FILE_BUILT = "EB1DFF71-2A3F-489E-AE7B-C1A12F54CBAF";
//    //public const string QueueStatus_DE_SENT_TO_RISK = "35C0E149-D8E2-4B59-A223-EEE410E80821";

//    //public const string QueueStatus_RK_RECEIVED = "B3C5096E-36D0-4DB8-82C7-8C4EC401FBE0";
//    //public const string QueueStatus_RK_IN_REVIEW = "85CC2592-74E0-461D-B089-DC96F59F7589";
//    //public const string QueueStatus_RK_SENT_TO_DEPLOYMENT = "04EB3C35-2274-48A9-8752-C471A41ABB39";

//    //public const string QueueStatus_DP_RECEIVED = "158F32CA-4447-48CE-9CD4-ED45514E24D8";
//    //public const string QueueStatus_DP_BUILD_TERMINAL_FILE_GATEWAY = "D0554A82-EE05-44A4-9D2B-F874C5D1EA10";
//    //public const string QueueStatus_DP_SCHEDULE_DOWNLOAD_TRAINING = "501FA519-AFB4-4935-8149-1929D35B8593";
//    //public const string QueueStatus_DP_MERCHANT_ACTIVE = "0B38A68C-D1E9-45D9-8AE0-E098BF621BC1";
//    //public const string QueueStatus_DP_SENT_TO_CLIENT_SERVICES = "72917208-AE07-42BE-A47C-C0C75A9F86A2";


//    //public const string QueueStatus_CS_RECEIVED = "A4CA9897-5719-4938-95F7-FA3D7A4BFF5B";
//    //public const string QueueStatus_CS_SCHEDULED_WELCOME_CALL = "F5F18138-D977-40BA-AB8C-415ABB4A0760";
//    //public const string QueueStatus_CS_CONDUCTED_WELCOME_CALL = "7C4F0BB4-3E7F-4644-8734-263256BE5AC1";
//    //public const string QueueStatus_CS_CANCELLATION = "1DB379A0-8F4C-4B9D-ACFC-092E6711DB88";
//    //public const string QueueStatus_CS_INACTIVE = "FC2F32C1-4359-4547-A3A9-528ADFFF6928";
//    //public const string QueueStatus_CS_ACTIVE = "36DB0F43-47CF-4F9B-BE79-B56A691DD8B8";



//    //public const string QueueStatus_AR_Received = "2223E9E3-970D-435F-A2E2-C5247F43AA7A";
//    //public const string QueueStatus_AR_Pending_Internal = "226D8CA8-6F11-4323-BD45-6EBA9A65CA4E";
//    //public const string QueueStatus_AR_Sent_to_Data_Entry = "A9204C55-4802-4D5C-85B7-24AEF07E818A";










    

//    public Constants()
//    {
//        //
//        // TODO: Add constructor logic here
//        //
//    }
//}
