using System;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace CertificateReader
{
    public class CertificateActions
    {
        internal static void GetRawCertificateData(X509Certificate certificate)
        {
            byte[] results = certificate.GetRawCertData();

            foreach (byte b in results)
            {
                Console.Write(b);
            }
        }

        internal static void GetRawCertificateString(X509Certificate certificate)
        {
            string result = certificate.GetRawCertDataString();
            Console.WriteLine(result);
        }

        internal static void GetCertificateHash(X509Certificate certificate)
        {
            byte[] results = certificate.GetCertHash();

            foreach (byte b in results)
            {
                Console.Write(b);
            }
        }

        internal static void GetCertificateHashString(X509Certificate certificate)
        {
            string result = certificate.GetCertHashString();
            Console.WriteLine(result);
        }

        internal static void GetCertificateHashCode(X509Certificate certificate)
        {
            int results = certificate.GetHashCode();
            Console.WriteLine(results);
        }

        internal static void ExportToByteArray(X509Certificate certificate)
        {
            var stringbuilder = new StringBuilder();
            stringbuilder.AppendLine("------BEGIN CERTIFICATE------");
            stringbuilder.AppendLine(Convert.ToBase64String(certificate.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            stringbuilder.AppendLine("------END   CERTIFICATE------");

            Console.WriteLine(stringbuilder.ToString());
        }

        internal static void ConvertCertificateByteArrayToHexString(X509Certificate certificate)
        {
            byte[] rawCertData = certificate.GetRawCertData();
            var hex = new StringBuilder(rawCertData.Length * 2);

            foreach (byte b in rawCertData)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            Console.WriteLine(hex.ToString());
        }

        internal static void ConvertCertificateToHex(X509Certificate certificate)
        {
            byte[] rawCertData = certificate.GetRawCertData();
            string hex = BitConverter.ToString(rawCertData);

            Console.WriteLine(hex);
        }

        internal static bool GetCertificateFromStore()
        {
            bool isFound;
            var store = new X509Store(StoreLocation.CurrentUser);
            Console.WriteLine("Enter thumbprint: ");
            string thumbPrint = Console.ReadLine();
            Console.WriteLine("Enter certificate name (e.g. 'CN=localhost'): ");
            string certName = Console.ReadLine();

            try
            {
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certificate2Collection = store.Certificates;
                X509Certificate2Collection currentCerts = certificate2Collection.Find(X509FindType.FindByThumbprint, thumbPrint, false);
                X509Certificate2Collection signingCerts = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);

                if (signingCerts.Count == 0)
                {
                    isFound = false;
                    Console.WriteLine();
                    Console.WriteLine("Certificate was not found");
                }
                else
                {
                    isFound = true;
                    Console.WriteLine(signingCerts[0].GetRawCertDataString());
                }

                return isFound;
            }
            finally
            {
                store.Close();
            }
        }

        internal static int InsertRawCertificateBinaryToDatabase(X509Certificate certificate)
        {
            int result = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            Console.WriteLine("Connection string:");
            Console.WriteLine(connectionString);
            Console.WriteLine("Do you want to use this connection string? - Y/N");
            string connectionConfirmation = Console.ReadLine();

            if (connectionConfirmation.ToUpper() == "Y")
            {
                string insertCommand = @"UPDATE [Table] SET [Column] = @certificate WHERE Id = 1";
                Console.WriteLine("SQL command that will be used:");
                Console.WriteLine(insertCommand);
                Console.WriteLine("Do you want to use this command? - Y/N");
                string commandConfirmation = Console.ReadLine();

                if (commandConfirmation.ToUpper() == "Y")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(insertCommand, connection);
                        command.Parameters.AddWithValue("@certificate", certificate.GetRawCertData());
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

            return result;
        }
    }
}