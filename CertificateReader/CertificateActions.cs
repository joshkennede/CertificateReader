using System;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace CertificateUtility
{
    public class CertificateActions
    {
        internal static Tuple<bool, string> GetRawCertificateData()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;

            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            byte[] results = certificate.GetRawCertData();

            if (results != null)
            {
                isSuccessful = true;
                messageResult = "The raw certificate was retrieved";
            }

            foreach (byte b in results)
            {
                Console.Write(b);
            }

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> GetRawCertificateString()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;

            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            string result = certificate.GetRawCertDataString();

            if (result != null)
            {
                messageResult = "The raw certificate string was retrieved";
                isSuccessful = true;
            }

            Console.WriteLine(result);

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> GetCertificateHash()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;

            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            byte[] results = certificate.GetCertHash();

            if (results != null)
            {
                isSuccessful = true;
                messageResult = "The raw certificate hash value was retrieved";
            }

            foreach (byte b in results)
            {
                Console.Write(b);
            }

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> GetCertificateHashString()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;

            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            string result = certificate.GetCertHashString();

            if (result != null)
            {
                messageResult = "The raw certificate hash string was retrieved";
                isSuccessful = true;
            }

            Console.WriteLine(result);

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> GetCertificateHashCode()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;

            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            int result = certificate.GetHashCode();

            if (result < 0 || result > 0)
            {
                isSuccessful = true;
                messageResult = "The raw certificate hash code was retrieved";
            }

            Console.WriteLine(result);

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> ExportToByteArray()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;

            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine("------BEGIN CERTIFICATE------");
            stringbuilder.AppendLine(Convert.ToBase64String(certificate.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            stringbuilder.AppendLine("------END   CERTIFICATE------");

            if (stringbuilder != null)
            {
                isSuccessful = true;
                messageResult = "The certificate was exported to a byte array";
            }

            Console.WriteLine(stringbuilder.ToString());

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> ConvertCertificateByteArrayToHexString()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;

            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            byte[] rawCertData = certificate.GetRawCertData();
            var hex = new StringBuilder(rawCertData.Length * 2);

            if (rawCertData != null)
            {
                isSuccessful = true;
                messageResult = "The certificate was converted from a byte array to a string";
            }

            foreach (byte b in rawCertData)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            Console.WriteLine(hex.ToString());

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> ConvertCertificateToHex()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;

            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            byte[] rawCertData = certificate.GetRawCertData();
            string hex = BitConverter.ToString(rawCertData);

            if (rawCertData != null)
            {
                isSuccessful = true;
                messageResult = "The raw certificate was converted to hexidecimal format";
            }

            Console.WriteLine(hex);

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> GetCertificateFromStore()
        {
            bool isSuccessful = false;
            string messageResult = string.Empty;
            bool isFound;
            var store = new X509Store(StoreLocation.CurrentUser);
            Console.WriteLine("Enter thumbprint: ");
            string thumbPrint = Console.ReadLine();
            Console.WriteLine("Enter certificate name (e.g. 'CN=DRS1WBVS1D.drs.wa.lcl'): ");
            string certName = Console.ReadLine();

            try
            {
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certificate2Collection = store.Certificates;
                X509Certificate2Collection currentCerts = certificate2Collection.Find(X509FindType.FindByThumbprint, thumbPrint, false);
                X509Certificate2Collection signingCerts = currentCerts.Find(X509FindType.FindBySubjectName, certName, false);

                if (signingCerts.Count == 0)
                {
                    isFound = false;
                    Console.WriteLine();
                    Console.WriteLine("Certificate was not found");
                }
                else
                {
                    isFound = true;
                    isSuccessful = isFound;
                    messageResult = isFound == true ? "The certificate was retrieved from local store" : "The certificate could not be retrieved from local store";
                    Console.WriteLine(signingCerts[0].GetRawCertDataString());
                }
            }
            finally
            {
                store.Close();
            }

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static Tuple<bool, string> InsertRawCertificateBinaryToDatabase()
        {
            bool isSuccessful = false;
            int result = 0;
            string certificatePath = Utilities.LoadCertificateFromFile();
            X509Certificate certificate = Utilities.CreateCertificateFromFile(certificatePath);
            
            string connectionString = ConfigurationManager.ConnectionStrings["IdentityServer"].ConnectionString;
            Console.WriteLine("Connection string:");
            Console.WriteLine(connectionString);
            Console.WriteLine("Do you want to use this connection string? - Y/N");
            string connectionConfirmation = Console.ReadLine();

            if (connectionConfirmation.ToUpper() == "Y")
            {
                Console.WriteLine("Enter id to update in database:");
                string idToInsert = Console.ReadLine();
                Console.WriteLine($"Id To Insert: {idToInsert}");
                string insertCommand = @"UPDATE [RelyingParties] SET [EncryptingCertificate] = @certificate WHERE Id = @Id";
                Console.WriteLine("SQL command that will be used:");
                Console.WriteLine(insertCommand);
                Console.WriteLine("Do you want to use this command? - Y/N");
                string commandConfirmation = Console.ReadLine();
                int Id = int.Parse(idToInsert);

                if (commandConfirmation.ToUpper() == "Y")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(insertCommand, connection);
                        command.Parameters.AddWithValue("@certificate", certificate.GetRawCertData());
                        command.Parameters.AddWithValue("@Id", Id);
                        connection.Open();
                        result = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Command was not used");
                }
            }
            else
            {
                Console.WriteLine("Command was not run");
            }

            if (result >= 1)
                isSuccessful = true;
            string messageResult = $"{result} certificate was inserted into database";

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }
    }
}