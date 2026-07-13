using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PaymentXP.BusinessObjects;

namespace BatchFileLoader
{
    public interface iBatchFile
    {
        string FileName
        {
            get;
            set;
        }

        long UploadID
        {
            get;
        }

        int MerchantID
        {
            get;
        }

        bool Encrypted
        {
            get;
        }

        MerchantApp Merchant
        {
            get;
            set;
        }

        string FileExtension
        {
            get;
        }

        bool ImportFile();

        bool PassDuplicateFile(FileInfo fi);

        long LogBatchFile(FileInfo fi);

        void ParseFile(string filePath);

        int TotalTransCount
        {
            get;
            set;
        }



    }
}
