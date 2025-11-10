using System;
using System.Text;

namespace Fase02Procedural
{
    public class Codificacao
    {
        public static string CodificarMensagem(string texto, string modo)
        {
            return modo switch
            {
                "base64" => CodificarBase64(texto),
                "cesar" => CodificarCesar(texto, 3),
                "invertido" => InverterTexto(texto),
                _ => texto
            };
        }

        private static string CodificarBase64(string texto)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(texto);
            return Convert.ToBase64String(bytes);
        }

        private static string CodificarCesar(string texto, int deslocamento)
        {
            StringBuilder resultado = new StringBuilder();
            foreach (char c in texto)
            {
                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    resultado.Append((char)((c - baseChar + deslocamento) % 26 + baseChar));
                }
                else
                {
                    resultado.Append(c);
                }
            }
            return resultado.ToString();
        }

        private static string InverterTexto(string texto)
        {
            char[] array = texto.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}

