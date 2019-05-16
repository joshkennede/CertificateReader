using System;
using System.Security.Cryptography.X509Certificates;

namespace CertificateReader
{
    public class Program
    {
        public static void Main()
        {
            MenuChoice optionSelected = CertificateUtility.GetMenuChoice();           
            Tuple<bool, string> result = CertificateUtility.ProcessMenuChoice(optionSelected);

            CertificateUtility.ProcessResult(result);
            CertificateUtility.ProcessAnotherCertificate();
        }
    }
}