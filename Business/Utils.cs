using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Utils
    {
        public static List<string> getSplittedStrings(string cadena) //a partir de una cadena, nos devuelve en una lista las palabras o frases según las reglas del negocio del challenge
        {
            List<string> lista = new List<string>();
            StringBuilder nuevaCadena = new StringBuilder();
            Boolean isNormal = true;

            for (int i = 0; i < cadena.Length; i++)
            {
                if (!Char.IsWhiteSpace(cadena[i]) && cadena[i] != '\"')
                {
                    nuevaCadena.Append(cadena[i]);
                }
                else if (Char.IsWhiteSpace(cadena[i]))
                {
                    if (isNormal)
                    {
                        nuevaCadena.Append("\r\n");
                    }
                    else
                    {
                        nuevaCadena.Append(" ");
                    }
                }
                else if (cadena[i] == '\"')
                {
                    isNormal = !isNormal;
                }
            }

            foreach (var token in nuevaCadena.ToString().Split('\n'))
            {
                lista.Add(token.Replace("\r",""));
            }
                        
            return (from l in lista where l.Length > 0 select l).ToList(); //prevenimos excepciones por items blank
        }

        public static void PrintToUser(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
