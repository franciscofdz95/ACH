using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Nmc.Ach.Dal;
using CommonUtility;


namespace BatchFileLoader
{
    public class BatchFileNmc : BatchFile
    {
        public BatchFileNmc(string filename)
            : base(filename)
        {
            this._FileExtension = ".NMC";
        }

        public BatchFileNmc(string fileName, bool encrypted, int zid)
            : base(fileName, encrypted, zid)
        {
            this._FileExtension = ".NMC";
        }

        public override void ParseFile(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            string strLine = string.Empty;
            int line = 1;

            try
            {
                //Pass the file path and file name to the StreamReader constructor
                using (StreamReader sr = new StreamReader(this.FileName))
                {
                    //Read the first line of text
                    strLine = sr.ReadLine();

                    if (strLine == null)
                    {
                        BatchLog.Batch.ErrorFormat("Empty NMC file detected.");
                        throw new BatchFileException("Empty NMC file detected.", BatchFileErrorCode.EmptyFile);
                    }

                    //Continue to read until you reach end of file
                    while (strLine != null)
                    {
                        try
                        {
                            BatchLog.Batch.InfoFormat("Line {0} - Parsing line......", line);

                            string[] fields = strLine.Split(new string[] { '"' + "," + '"' }, StringSplitOptions.None);

                            if (fields.Length != 10)
                            {
                                sb.AppendFormat("Line [{0}]: File format is invalid (Column Count Error)<br/>", line);
                            }
                            else
                            {
                                Transaction tran = new Transaction();

                                tran.MerchantID = this.MerchantID;
                                tran.Description = fields[0].Replace("\"", "");
                                tran.TransType = fields[1].Replace("\"", "");
                                tran.TransRoute = fields[2].Replace("\"", "");
                                tran.AccountNo = fields[3].Replace("\"", "");
                                tran.NameOnAccount = fields[4].Replace("\"", "");
                                tran.RefID = fields[5].Replace("\"", "");
                                tran.Amount = Math.Round(decimal.Parse(fields[6].Replace("\"", "")), 2);
                                tran.Secc = fields[7].Replace("\"", "");
                                tran.CompanyName = fields[8].Replace("\"", "");

                                //Flag transaction if it is a resubmitted trans
                                if (Convert.ToInt32(fields[9].Replace("\"", "")) > 1)
                                    tran.IsResubmittedTrans = true;

                                this.FileTotal += tran.Amount;
                                this.TotalTransCount++;

                                this._Transactions.Add(tran);

                                BatchLog.Batch.InfoFormat("Line {0} - ACH transaction parsed.", line);
                            }

                            //Read the next line
                            strLine = sr.ReadLine();
                            line++;
                        }
                        catch (Exception ex)
                        {
                            BatchLog.Batch.ErrorFormat("A {0} has occurred while parsing line '{1}': {2}", ex.GetType().Name, line, ex.Message);
                            BatchLog.Batch.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                            sb.AppendFormat("Line [{0}]: {1}<br>", line, ex.Message);
                        }
                    }

                    sr.Close();
                }

                if (sb.Length > 0)
                {
                    //parsing had errors
                    throw new BatchFileException(sb.ToString(), BatchFileErrorCode.ParseErrors);
                }

                BatchLog.Batch.InfoFormat("Total transactions: {0}", this.TotalTransCount);
                BatchLog.Batch.InfoFormat("Total amount: {0}", this.FileTotal);
            }
            catch (BatchFileException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                BatchLog.Batch.ErrorFormat("A {0} has occurred while parsing batch file '{1}': {2}", ex.GetType().Name, this.FileName, ex.Message);
                BatchLog.Batch.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                throw new BatchFileException("Failed to parse batch file", ex.InnerException, BatchFileErrorCode.ParseFailed);
            }
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

                foreach (Transaction tran in this._Transactions)
                {
                    BatchLog.Batch.InfoFormat("Processing transaction {0} of {1}......", cur, this._Transactions.Count);

                    tran.UploadID = this.UploadID;
                    long transId = tran.SaveTransaction();

                    BatchLog.Batch.InfoFormat("Transaction {0} of {1} authorized [TransID={2}]", cur, this._Transactions.Count, transId);
                    BatchLog.Batch.InfoFormat("Transaction {0} of {1} processed.", cur, this._Transactions.Count);

                    cur++;
                }

                BatchLog.Batch.InfoFormat("NMC batch transaction processing complete.");

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
