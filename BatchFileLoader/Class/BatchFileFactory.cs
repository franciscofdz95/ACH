using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BatchFileLoader
{
    public class BatchFileFactory
    {
        public BatchFile GetBatchFile(string filename)
        {
            BatchFile batch = null;
            string extension = filename.Substring(filename.Length - 3, 3).ToUpper();

            switch (extension)
            { 
                case "ACH":
                    batch = new BatchFileStandard(filename);
                    break;
                case "NAC":
                    batch = new BatchFileNacha(filename);
                    break;
                case "NMC":
                    batch = new BatchFileNmc (filename);
                    break;
                case "PGP":
                    string encExtension = filename.Substring(filename.Length - 7, 7).ToUpper();

                    switch(encExtension)
                    {
                        case "ACH.PGP":
                            batch = new BatchFileStandard(filename);
                            break;
                        case "NAC.PGP":
                            batch = new BatchFileNacha(filename);
                            break;
                        case "NMC.PGP":
                            batch = new BatchFileNmc(filename);
                            break;
                    }

                    break;
            }

            return batch;

        }

        public static iBatchFile GetBatchFile(string fileName, int zid)
        {
            iBatchFile batchFile = null;

            FileInfo info = new FileInfo(fileName);

            switch(info.Extension.TrimStart('.').ToUpper())
            {
                case "ACH":
                    batchFile = new BatchFileStandard(fileName, false, zid);
                    BatchLog.Root.InfoFormat("Batch file '{0}' is of type BatchFileStandard", fileName);
                    break;
                case "NAC":
                    batchFile = new BatchFileNacha(fileName, false, zid);
                    BatchLog.Root.InfoFormat("Batch file '{0}' is of type BatchFileNacha", fileName);
                    break;
                case "NMC":
                    batchFile = new BatchFileNmc(fileName, false, zid);
                    BatchLog.Root.InfoFormat("Batch file '{0}' is of type BatchFileNmc", fileName);
                    break;
                case "PGP":
                    //this implies the file was encrypted, we need to get the file extension of the batch file
                    //before it got encrypted to figure out the batch type and ultimately the correct class
                    //to instantiate for batch processing
                    if(info.FullName.Length < 7)
                    {
                        //we should have a file name of at least 7 characters to be
                        //able to figure out the type of batch format uploaded. if 
                        //we don't return null
                        BatchLog.Root.InfoFormat("Invalid Batch file '{0}': Invalid encrypted file extension", fileName);
                        return null;
                    }

                    string encExtension = fileName.Substring(fileName.Length - 7, 7).ToUpper();

                    switch (encExtension)
                    {
                        case "ACH.PGP":
                            batchFile = new BatchFileStandard(fileName, true, zid);
                            BatchLog.Root.InfoFormat("Batch file '{0}' is of type BatchFileStandard", fileName);
                            break;
                        case "NAC.PGP":
                            batchFile = new BatchFileNacha(fileName, true, zid);
                            BatchLog.Root.InfoFormat("Batch file '{0}' is of type BatchFileNacha", fileName);
                            break;
                        case "NMC.PGP":
                            batchFile = new BatchFileNmc(fileName, true, zid);
                            BatchLog.Root.InfoFormat("Batch file '{0}' is of type BatchFileNmc", fileName);
                            break;
                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }

            return batchFile;
        }
    }
}
