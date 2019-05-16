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
            Console.WriteLine();
            Console.WriteLine("Enter Path to Certificate: ");
            string pathToCertificate = @"" + Console.ReadLine();
            Console.WriteLine();

            return pathToCertificate;
        }

        internal static void ProcessResult(Tuple<bool, string> result)
        {
            if (result.Item1 == true)
            {
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
                var optionSelected = GetMenuChoice();                
                var result = ProcessMenuChoice(optionSelected);
                ProcessResult(result);
            }
            else
            {
                Console.WriteLine("All Done!");
            }
        }

        internal static MenuChoice GetMenuChoice()
        {
            Console.WriteLine(@"***** CERTIFICATE READER *****");
            Console.WriteLine();
            Console.WriteLine(@"********************");
            Console.WriteLine(@"***** OPTIONS ******");
            Console.WriteLine(@"********************");
            Console.WriteLine();
            Console.WriteLine(@" 0 - Quit");
            Console.WriteLine();
            Console.WriteLine(@" 1 - Get Raw Cert Data");
            Console.WriteLine(@" 2 - Get Raw Cert String");
            Console.WriteLine(@" 3 - Get Cert Hash");
            Console.WriteLine(@" 4 - Get Cert Hash String");
            Console.WriteLine(@" 5 - Get Cert Hash Code");
            Console.WriteLine(@" 6 - Export To Byte Array");
            Console.WriteLine(@" 7 - Convert Cert To Hex");
            Console.WriteLine(@" 8 - Convert Cert To Hex String");
            Console.WriteLine(@" 9 - Get Cert from Store");
            Console.WriteLine(@" 10 - Insert Certificate into SQL");
            Console.WriteLine();
            Console.WriteLine(@"Enter selection: ");
            string selectedOption = Console.ReadLine();

            if (!int.TryParse(selectedOption, out int selection))
            {
                selection = -1;
            }

            return (MenuChoice)selection;
        }

        internal static Tuple<bool, string> ProcessMenuChoice(MenuChoice optionSelected)
        {
            var results = new Tuple<bool, string>(false, string.Empty);

            try
            {
                switch (optionSelected)
                {
                    case MenuChoice.Quit:
                        QuitMessage();
                        break;
                    case MenuChoice.RawCertData:
                        results = CertificateActions.GetRawCertificateData();
                        break;
                    case MenuChoice.RawCertString:
                        results = CertificateActions.GetRawCertificateString();                        
                        break;
                    case MenuChoice.CertHash:
                        results = CertificateActions.GetCertificateHash();                        
                        break;
                    case MenuChoice.CertHashString:
                        results = CertificateActions.GetCertificateHashString();                        
                        break;
                    case MenuChoice.CertHashCode:
                        results = CertificateActions.GetCertificateHashCode();                        
                        break;
                    case MenuChoice.ExportToPEM:
                        results = CertificateActions.ExportToByteArray();                        
                        break;
                    case MenuChoice.ConvertToHex:
                        results = CertificateActions.ConvertCertificateByteArrayToHexString();                        
                        break;
                    case MenuChoice.ConvertToHexString:
                        results = CertificateActions.ConvertCertificateToHex();                        
                        break;
                    case MenuChoice.CertFromStore:
                        results = CertificateActions.GetCertificateFromStore();
                        break;
                    case MenuChoice.InsertCertIntoSQL:
                        results = CertificateActions.InsertRawCertificateBinaryToDatabase();                        
                        break;
                    default:
                        results = InvalidMenuSelection();
                        break;
                }

                Console.WriteLine();
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine($"There was an error processing your request - {exception.ToString()}");
            }

            return new Tuple<bool, string>(results.Item1, results.Item2);
        }

        internal static void QuitMessage()
        {
            Console.WriteLine("Thanks for using your friendly neighborhood certificate utility.");
        }

        internal static Tuple<bool, string> InvalidMenuSelection()
        {
            bool isSuccessful = false;
            string messageResult = "The certificate was not processed because of some issue or error";
            Console.WriteLine();
            Console.WriteLine("Thats not a valid menu choice!");

            return new Tuple<bool, string>(isSuccessful, messageResult);
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