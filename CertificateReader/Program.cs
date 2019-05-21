using System;

namespace CertificateUtility
{
    public class Program
    {
        public static void Main()
        {
            MenuChoice optionSelected = Utilities.GetMenuChoice();           
            Tuple<bool, string> result = Utilities.ProcessMenuChoice(optionSelected);

            Utilities.ProcessResult(result);
            Utilities.ProcessAnotherCertificate();
        }
    }
}