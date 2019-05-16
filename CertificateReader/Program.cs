using System;
using System.Security.Cryptography.X509Certificates;

namespace CertificateReader
{
    public class Program
    {
        public static void Main()
        {
            string certificatePath = CertificateUtility.LoadCertificateFromFile();
            X509Certificate certificate = CertificateUtility.CreateCertificateFromFile(certificatePath);

            MenuChoice optionSelected = CertificateUtility.GetMenuChoice(certificate);
            Tuple<bool, string> result = CertificateUtility.ProcessMenuChoice(optionSelected, certificate);

            CertificateUtility.ProcessResult(result);
            CertificateUtility.ProcessAnotherCertificate();
        }
    }
}