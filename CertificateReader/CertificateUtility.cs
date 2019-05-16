using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CertificateReader
{
    public class CertificateUtility
    {
        internal static X509Certificate CreateCertificateFromFile(string pathToCertificate)
        {
            try
            {
                var certificate = X509Certificate.CreateFromCertFile(pathToCertificate);
                return certificate;
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine($"There was an error proccessing the certificate path - {exception.ToString()}");
                throw;
            }
        }

        internal static string LoadCertificateFromFile()
        {
            Console.WriteLine(@"***** CERTIFICATE READER *****");
            Console.WriteLine();
            Console.WriteLine("Enter Path to Certificate: ");
            string pathToCertificate = @"" + Console.ReadLine();
            return pathToCertificate;
        }

        internal static void ProcessResult(Tuple<bool, string> result)
        {
            if (result.Item1 == true)
            {
                Console.WriteLine();
                Console.WriteLine("Certificate was processed successfully");
                Console.WriteLine(result.Item2);
                Console.WriteLine();
            }
            else if (result.Item1 == false && result.Item2 == string.Empty)
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Certificate couldn't be processed");
                Console.WriteLine(result.Item2);
                Console.WriteLine();
            }
        }

        internal static void ProcessAnotherCertificate()
        {
            Console.WriteLine("Would you like to process another certificate? - Y/N");
            var confirmation = Console.ReadLine();

            if (confirmation.ToUpper() == "Y")
            {
                string certificatePath = LoadCertificateFromFile();
                var certificate = CreateCertificateFromFile(certificatePath);
                var optionSelected = GetMenuChoice(certificate);
                var result = ProcessMenuChoice(optionSelected, certificate);
                ProcessResult(result);
            }
            else
            {
                Console.WriteLine("All Done!");
            }
        }

        internal static MenuChoice GetMenuChoice(X509Certificate certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(@"********************");
                Console.WriteLine(@"***** OPTIONS ******");
                Console.WriteLine(@"********************");
                Console.WriteLine();
                Console.WriteLine(@"0 - Quit");
                Console.WriteLine();
                Console.WriteLine(@"1 - Get Raw Cert Data");
                Console.WriteLine(@"2 - Get Raw Cert String");
                Console.WriteLine(@"3 - Get Cert Hash");
                Console.WriteLine(@"4 - Get Cert Hash String");
                Console.WriteLine(@"5 - Get Cert Hash Code");
                Console.WriteLine(@"6 - Export To Byte Array");
                Console.WriteLine(@"7 - Convert Cert To Hex");
                Console.WriteLine(@"8 - Convert Cert To Hex String");
                Console.WriteLine(@"9 - Get Cert from Store");
                Console.WriteLine(@"10 - Insert Certificate into SQL");
                Console.WriteLine();
                Console.WriteLine(@"Enter selection: ");
                string selectedOption = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(selectedOption, out int selection))
                {
                    selection = -1;
                }

                return (MenuChoice)selection;
            }
        }

        internal static Tuple<bool, string> ProcessMenuChoice(MenuChoice optionSelected, X509Certificate certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            bool isSuccessful = false;
            string messageResult = string.Empty;

            try
            {
                switch (optionSelected)
                {
                    case MenuChoice.Quit:
                        QuitMessage();
                        break;
                    case MenuChoice.RawCertData:
                        CertificateActions.GetRawCertificateData(certificate);
                        isSuccessful = true;
                        messageResult = "The raw certificate was retrieved";
                        break;
                    case MenuChoice.RawCertString:
                        CertificateActions.GetRawCertificateString(certificate);
                        messageResult = "The raw certificate string was retrieved";
                        isSuccessful = true;
                        break;
                    case MenuChoice.CertHash:
                        CertificateActions.GetCertificateHash(certificate);
                        isSuccessful = true;
                        messageResult = "The raw certificate hash value was retrieved";
                        break;
                    case MenuChoice.CertHashString:
                        CertificateActions.GetCertificateHashString(certificate);
                        messageResult = "The raw certificate hash string was retrieved";
                        isSuccessful = true;
                        break;
                    case MenuChoice.CertHashCode:
                        CertificateActions.GetCertificateHashCode(certificate);
                        isSuccessful = true;
                        messageResult = "The raw certificate hash code was retrieved";
                        break;
                    case MenuChoice.ExportToPEM:
                        CertificateActions.ExportToByteArray(certificate);
                        isSuccessful = true;
                        messageResult = "The certificate was exported to a byte array";
                        break;
                    case MenuChoice.ConvertToHex:
                        CertificateActions.ConvertCertificateByteArrayToHexString(certificate);
                        isSuccessful = true;
                        messageResult = "The certificate was converted from a byte array to a string";
                        break;
                    case MenuChoice.ConvertToHexString:
                        CertificateActions.ConvertCertificateToHex(certificate);
                        isSuccessful = true;
                        messageResult = "The raw certificate was converted to hexidecimal format";
                        break;
                    case MenuChoice.CertFromStore:
                        var isFound = CertificateActions.GetCertificateFromStore();
                        isSuccessful = isFound;
                        messageResult = isFound == true ? "The certificate was retrieved from local store": "The certificate could not be retrieved from local store";
                        break;
                    case MenuChoice.InsertCertIntoSQL:
                        var result = CertificateActions.InsertRawCertificateBinaryToDatabase(certificate);
                        if (result >= 1)
                            isSuccessful = true;
                        messageResult = $"{result} certificate was inserted into database";
                        break;
                    default:
                        UnknownOperation();
                        isSuccessful = false;
                        messageResult = "The certificate was not processed because of some issue or error";
                        break;
                }
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine($"There was an error processing your request - {exception.ToString()}");
            }

            return new Tuple<bool, string>(isSuccessful, messageResult);
        }

        internal static void QuitMessage()
        {
            Console.WriteLine("Thanks for using your friendly neighborhood certificate utility.");
        }

        internal static void UnknownOperation()
        {
            Console.WriteLine();
            Console.WriteLine("Thats not a valid menu choice!");
        }
    }

    public enum MenuChoice
    {
        Unknown = -1,
        Quit = 0,
        RawCertData = 1,
        RawCertString = 2,
        CertHash = 3,
        CertHashString = 4,
        CertHashCode = 5,
        ExportToPEM = 6,
        ConvertToHex = 7,
        ConvertToHexString = 8,
        CertFromStore = 9,
        InsertCertIntoSQL = 10
    }
}