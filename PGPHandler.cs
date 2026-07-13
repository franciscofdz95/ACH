using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;

namespace FileMoveNechaToBrown
{
	class PGPHandler
	{

		static string m_DecryptedFile = "";

		public static bool Decrypt(string Src, string Dest, ref string ErrorMessage)
		{
			SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("Ao89LoZas0gAn0ujyMIe1ELVdB7lxUdyr6+XgLbaCNRSqd14Zwt7Xu6yBNVJRISCdDPSKnCt2zyXIyRX+NJlfirkeVqB/lUlpWXlSicavW0QS2sn31mjod7UAldfCldLj3B8JoFToFgzGZfOfIq5GdC8EZN69X4gsHh+6DvSYXF2twEotsEOFwWtChVEq2lvBoZHqBhTfDzDt8sLU695u3v7tXWFCBjeJsYT6psxu/SIoPzSJItonqS+dz+AWb9ZfhOrpHviwnaGocGSKkoihqIurpLzH7faPqnF9V9hy5BvdryXSsKaKLX9HFUNtHWTili+REfWGulYrb/TFTUG2w=="));
			string message = string.Empty;

			m_DecryptedFile = Dest;
			if (!File.Exists(Src))
				throw new Exception("PGPDecrypt -> Source File Does Not Exist");
			SBPGPKeys.TElPGPKeyring keyring = new SBPGPKeys.TElPGPKeyring();
			SBPGP.TElPGPReader pgpReader = new SBPGP.TElPGPReader();
			string publicKey = ConfigurationManager.AppSettings ["PublicKey"];
			string secretKey = ConfigurationManager.AppSettings ["SecretKey"];

			keyring.Load(publicKey, secretKey, true);

			pgpReader.DecryptingKeys = keyring;
			pgpReader.VerifyingKeys = keyring;

			pgpReader.OnCreateOutputStream += new SBPGP.TSBPGPCreateOutputStreamEvent(OnCreateOutputStream);
			pgpReader.OnKeyPassphrase += new SBPGPStreams.TSBPGPKeyPassphraseEvent(OnKeyPassphrase);

			System.IO.FileStream inF = null;
			inF = new System.IO.FileStream(Src, FileMode.Open);
			try
			{
				pgpReader.DecryptAndVerify(inF, 0);
				return true;
			}
			catch (Exception ex)
			{
				ErrorMessage = "ErrorMessage: " + ex.Message + " : " + ex.StackTrace;
				return false;
			}
			finally
			{
				inF.Close();
				//File.Delete(Src);
			}
		}

        public static bool Encrypt(string Src, string Dest, ref string ErrorMessage)
        {
            SBUtils.Unit.SetLicenseKey(SBUtils.Unit.BytesOfString("Ao89LoZas0gAn0ujyMIe1ELVdB7lxUdyr6+XgLbaCNRSqd14Zwt7Xu6yBNVJRISCdDPSKnCt2zyXIyRX+NJlfirkeVqB/lUlpWXlSicavW0QS2sn31mjod7UAldfCldLj3B8JoFToFgzGZfOfIq5GdC8EZN69X4gsHh+6DvSYXF2twEotsEOFwWtChVEq2lvBoZHqBhTfDzDt8sLU695u3v7tXWFCBjeJsYT6psxu/SIoPzSJItonqS+dz+AWb9ZfhOrpHviwnaGocGSKkoihqIurpLzH7faPqnF9V9hy5BvdryXSsKaKLX9HFUNtHWTili+REfWGulYrb/TFTUG2w=="));
            string message = string.Empty;

            m_DecryptedFile = Dest;
            if (!File.Exists(Src))
                throw new Exception("Encrypt -> Source File Does Not Exist");
            
            System.IO.FileStream inF, outF;
            System.IO.FileInfo info;

            SBPGPKeys.TElPGPKeyring keyring = new SBPGPKeys.TElPGPKeyring();
            SBPGP.TElPGPWriter pgpWriter = new SBPGP.TElPGPWriter();
            string publicKey = ConfigurationManager.AppSettings ["PublicKey"];
            string secretKey = ConfigurationManager.AppSettings ["SecretKey"];

            keyring.Load(publicKey, secretKey, true);

            pgpWriter.Compress = false;
            pgpWriter.EncryptingKeys = keyring;
            pgpWriter.SigningKeys = keyring;
            pgpWriter.EncryptionType = SBPGP.TSBPGPEncryptionType.etPublicKey;

            info = new System.IO.FileInfo(Src);
            pgpWriter.Filename = info.Name;
            pgpWriter.InputIsText = false;
            pgpWriter.Passphrases.Clear();
            pgpWriter.Passphrases.Add("");
            pgpWriter.Protection = SBPGPConstants.TSBPGPProtectionType.ptNormal;
            pgpWriter.SignBufferingMethod = SBPGP.TSBPGPSignBufferingMethod.sbmRewind;
            pgpWriter.SymmetricKeyAlgorithm = SBPGPConstants.Unit.SB_PGP_ALGORITHM_SK_CAST5;

            pgpWriter.Timestamp = DateTime.Now;
            pgpWriter.UseNewFeatures = true;
            pgpWriter.UseOldPackets = false;

            inF = new FileStream(Src, FileMode.Open);
            outF = new FileStream(Dest, FileMode.Create);
            try
            {
                pgpWriter.Encrypt(inF, outF, 0);
                return true;
            }
            catch (Exception exc)
            {
                ErrorMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace;
                return false;
            }
            finally
            {
                outF.Close();
                inF.Close();
            }

        }


		private static void OnCreateOutputStream(object Sender, string Filename, System.DateTime TimeStamp, ref System.IO.Stream Stream, ref bool FreeOnExit)
		{
			Stream = new System.IO.FileStream(m_DecryptedFile, FileMode.Create);
			FreeOnExit = true;
		}


		private static void OnKeyPassphrase(object Sender, SBPGPKeys.TElPGPCustomSecretKey Key, ref string Passphrase, ref bool Cancel)
		{
			string pass = "620 newport";
			if (pass == "" || pass.Length <= 0)
				throw new Exception("OnKeyPassphrase -> Password Required for Decrypt");
			Passphrase = pass;
		}
	}
}
