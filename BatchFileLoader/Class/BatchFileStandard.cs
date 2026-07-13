using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nmc.Ach.Dal;
using CommonUtility;


namespace BatchFileLoader
{
    public class BatchFileStandard: BatchFile 
    {
        public BatchFileStandard(string filename)
            : base(filename)
        {
            this._FileExtension = ".ACH";
        }

        public BatchFileStandard(string fileName, bool encrypted, int zid)
            : base(fileName, encrypted, zid)
        {
            this._FileExtension = ".ACH";
        }

        public override bool ImportFile()
        {
            int cur = 1;

            try
            {
                if (this._Transactions.Count == 0)
                {
                    BatchLog.Batch.ErrorFormat("No transactions to process");
                    throw new BatchFileException("No transactions to process", BatchFileErrorCode.NoTransactions);
                }

                foreach (Transaction achTrans in this._Transactions)
                {
                    BatchLog.Batch.InfoFormat("Processing transaction {0} of {1}......", cur, this._Transactions.Count);

                    achTrans.UploadID = this.UploadID;

                    long transId = achTrans.SaveTransaction();

                    BatchLog.Batch.InfoFormat("Transaction {0} of {1} authorized [TransID={2}]", cur, this._Transactions.Count, transId);

                    if (transId > 0)
                    {
                        BatchLog.Batch.InfoFormat("Saving transaction [TransID={0}] details......", transId);

                        achTrans.SaveTransactionDetails();

                        BatchLog.Batch.InfoFormat("Transaction [TransID={0}] details saved", transId);
                    }

                    BatchLog.Batch.InfoFormat("Transaction {0} of {1} processed.", cur, this._Transactions.Count);

                    cur++;
                }

                BatchLog.Batch.InfoFormat("Standard batch transaction processing complete.");

                return true;
            }
            catch (BatchFileException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                BatchLog.Batch.ErrorFormat("A {0} has occurred while processing batch file '{1}': {2}", ex.GetType().Name, this.FileName, ex.Message);
                BatchLog.Batch.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                throw new BatchFileException("Transaction processing failed", ex.InnerException, BatchFileErrorCode.ProcessingFailed);
            }
        }
    }
}
