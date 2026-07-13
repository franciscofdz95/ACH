using System;
using System.Security.Cryptography;
using System.Text;

namespace Nmc.Common
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Crypto
	{		

		private SymmetricAlgorithm mobjCryptoService;
		private string desKey = "2r98Jy3x";

		public Crypto()
		{
			mobjCryptoService = new DESCryptoServiceProvider();
		}

		/// <remarks>
		/// Constructor for using a customized SymmetricAlgorithm class.
		/// </remarks>
		public Crypto(SymmetricAlgorithm ServiceProvider)
		{
			mobjCryptoService = ServiceProvider;
		}

		public Crypto(SymmetricAlgorithm ServiceProvider, string Key)
		{
			mobjCryptoService = ServiceProvider;
			desKey = Key;
		}		

		public string Decrypt(string Source)
		{

			// convert from Base64 to binary
			byte[] bytIn = System.Convert.FromBase64String(Source);
			// create a MemoryStream with the input
			System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);

			byte[] bytKey = ASCIIEncoding.ASCII.GetBytes(desKey);

			// set the private key
			mobjCryptoService.Key = bytKey;
			mobjCryptoService.IV = bytKey;

			// create a Decryptor from the Provider Service instance
			ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
 
			// create Crypto Stream that transforms a stream using the decryption
			CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

			// read out the result from the Crypto Stream
			System.IO.StreamReader sr = new System.IO.StreamReader( cs );
			return sr.ReadToEnd();

		}	


		public string Encrypt(string Source)
		{
			byte[] bytIn = System.Text.ASCIIEncoding.ASCII.GetBytes(Source);
			// create a MemoryStream so that the process can be done without I/O files
			System.IO.MemoryStream ms = new System.IO.MemoryStream();

			byte[] bytKey = ASCIIEncoding.ASCII.GetBytes(desKey);

			// set the private key
			mobjCryptoService.Key = bytKey;
			mobjCryptoService.IV = bytKey;

			// create an Encryptor from the Provider Service instance
			ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
			
			// create Crypto Stream that transforms a stream using the encryption
			CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

			// write out encrypted content into MemoryStream			
			cs.Write(bytIn, 0, bytIn.Length);
			cs.FlushFinalBlock();
            
			byte[] bytOut = ms.GetBuffer();						
			/// 
			/// Instead of looking for the first null char,
			/// we will search for the first non-null char from the end
			/// 

			// trim the '\0' bytes						
			//			int i = 0;
			//			for (i = 0; i < bytOut.Length; i++)
			//				if (bytOut[i] == 0)
			//					break;

			int i = 0;
			for ( i = bytOut.Length; i > -1; i-- )
				if ( bytOut[i-1] != 0 )
					break;                
			/// 
			///
			///
			///

			/// This is a bug fix
			/// Data to save needs to be in complete groups of 8 bytes            
			if ((i % 8) != 0)
				i += 1;

			// convert into Base64 so that the result can be used in xml			
			return System.Convert.ToBase64String(bytOut, 0, i);
		}

	}
}
