using System;

namespace GetADData.utilities
{
    public class ArgsValidations
    {
        public static void IsArgsValid(string[] args)
        {
            var useMode = @"Modo de uso: GetADData.exe <email@dominio> <arquivo_de_saida.json>";

            if (args.Length < 2 )
            {
                Console.WriteLine("Parametros ausentes! " + useMode);
                Environment.Exit(1);
            }

            if (!RegexUtilities.IsValidEmail(args[0]))
            {
                Console.WriteLine("Email invalido! " + useMode);
                Environment.Exit(1);
            }

        }
    }
}
