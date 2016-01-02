using DarthNemesis;
using Devil_Survivor_Script_Editor.LZ77;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Devil_Survivor_Script_Editor
{
    class leerArchivo
    {
            static Dictionary<string,string> conversion = new Dictionary<string, string>()
            {
                {"91F0","91F0"},
                {"8360","8360"},
                {"8151","_"},
                {"815E","/"},
                {"8140"," "},
                {"8143",","},
                {"8144","."},
                {"8146",":"},
                {"8147",";"},
                {"8148","?"},
                {"8149","!"},
                {"8166","'"},
                {"8168","“"},
                {"817B","+"},
                {"817C","-"},
                {"8193","%"},
                {"8240","!?"},
                {"8260","A"},
                {"8261","B"},
                {"8262","C"},
                {"8263","D"},
                {"8264","E"},
                {"8265","F"},
                {"8266","G"},
                {"8267","H"},
                {"8268","I"},
                {"8269","J"},
                {"826A","K"},
                {"826B","L"},
                {"826C","M"},
                {"826D","N"},
                {"826E","O"},
                {"826F","P"},
                {"8270","Q"},
                {"8271","R"},
                {"8272","S"},
                {"8273","T"},
                {"8274","U"},
                {"8275","V"},
                {"8276","W"},
                {"8277","X"},
                {"8278","Y"},
                {"8279","Z"},
                {"8281","a"},
                {"8282","b"},
                {"8283","c"},
                {"8284","d"},
                {"8285","e"},
                {"8286","f"},
                {"8287","g"},
                {"8288","h"},
                {"8289","i"},
                {"828A","j"},
                {"828B","k"},
                {"828C","l"},
                {"828D","m"},
                {"828E","n"},
                {"828F","o"},
                {"8290","p"},
                {"8291","q"},
                {"8292","r"},
                {"8293","s"},
                {"8294","t"},
                {"8295","u"},
                {"8296","v"},
                {"8297","w"},
                {"8298","x"},
                {"8299","y"},
                {"829A","z"},
                {"81DF","¿"},
                {"81E0","Á"},
                {"81E1","É"},
                {"81E2","Í"},
                {"81E3","Ó"},
                {"81E4","Ú"},
                {"81E5","¡"},
                {"81E6","ñ"},
                {"81DE","á"},
                {"81E8","é"},
                {"81F0","í"},
                {"81F1","ó"},
                {"81F2","ú"},
                {"81F3","ü"},
                {"81F4","??"},
                {"81F5","¡¿"},
                {"81F6","¡¡"},
                {"8145","·"},
                {"8195","&"},
                {"8250","1"},
                {"8251","2"},
                {"8252","3"},
                {"8253","4"},
                {"8254","5"},
                {"8255","6"},
                {"8256","7"},
                {"8257","8"},
                {"8258","9"},
                {"824F","0"},
                {"8169","("},
                {"816A",")"},
                {"8196","*"},
                {"8241","!!"},
                {"00","[00]\n"},
                {"3C62723E","\n"},
                {"3C633D30353E","[c=05]"},
                {"3C636E3D303030443E","[cn=000D]"},
                {"3C636E3D303030303E","[cn=0000]"},
                {"3C636E3D303030433E","[cn=000C]"},
                {"3C703E","[new]"},
                {"3C633D30393E","[color1]"},
                {"3C633D46463E","[/color]"},
                {"3C633D30363E","[color2]"},
                {"3C6E643D30303E","[nd=00]"},
                {"3C6E643D30313E","[nd=01]"},

                //Enable Russian support.
                {"8440","А"},
                {"8441","Б"},
                {"8442","В"},
                {"8443","Г"},
                {"8444","Д"},
                {"8445","Е"},
                {"8446","Ё"},
                {"8447","Ж"},
                {"8448","З"},
                {"8449","И"},
                {"844A","Й"},
                {"844B","К"},
                {"844C","Л"},
                {"844D","М"},
                {"844E","Н"},
                {"844F","О"},
                {"8450","П"},
                {"8451","Р"},
                {"8452","С"},
                {"8453","Т"},
                {"8454","У"},
                {"8455","Ф"},
                {"8456","Х"},
                {"8457","Ц"},
                {"8458","Ч"},
                {"8459","Ш"},
                {"845A","Щ"},
                {"845B","Ъ"},
                {"845C","Ы"},
                {"845D","Ь"},
                {"845E","Э"},
                {"845F","Ю"},
                {"8460","Я"},
                {"8470","а"},
                {"8471","б"},
                {"8472","в"},
                {"8473","г"},
                {"8474","д"},
                {"8475","е"},
                {"8476","ё"},
                {"8477","ж"},
                {"8478","з"},
                {"8479","и"},
                {"847A","й"},
                {"847B","к"},
                {"847C","л"},
                {"847D","м"},
                {"847E","н"},
                {"8480","о"},
                {"8481","п"},
                {"8482","р"},
                {"8483","с"},
                {"8484","т"},
                {"8485","у"},
                {"8486","ф"},
                {"8487","х"},
                {"8488","ц"},
                {"8489","ч"},
                {"848A","ш"},
                {"848B","щ"},
                {"848C","ъ"},
                {"848D","ы"},
                {"848E","ь"},
                {"848F","э"},
                {"8490","ю"},
                {"8491","я"}

            };



        public static string leer(string archivoHex)
        {


            FileStream fs = new FileStream(archivoHex, FileMode.Open);
            int hexIn;
            string hex = "";
            StringBuilder hex2 = new StringBuilder();
            while ((hexIn = fs.ReadByte()) != -1)
            {
                hex2.Append(string.Format("{0:X2}", hexIn));
            }

            hex = hex2.ToString();
            fs.Dispose();

            byte[] compressedFile = strToByte.StringToByteArray(hex);
            byte[] decompressedFile = CompressionManager.Decompress(compressedFile);

            hex = strToByte.ByteArrayToString(decompressedFile);

            //Console.WriteLine(decompressedData);


            //string cabeceraDatos = (archivoHex.Contains("ds_event.cmp")) ? hex[212].ToString() + hex[213].ToString() + hex[210].ToString() + hex[211].ToString() + hex[208].ToString() + hex[209].ToString() : cabeceraDatos = hex[210].ToString() + hex[211].ToString() + hex[208].ToString() + hex[209].ToString();

            string cabeceraDatos = "";
            if (archivoHex.Contains("ds_event.cmp"))
            {
                cabeceraDatos = hex[212].ToString() + hex[213].ToString() + hex[210].ToString() + hex[211].ToString() + hex[208].ToString() + hex[209].ToString();
            }
            else if (archivoHex.Contains("ds_eventm"))
            {
                cabeceraDatos = "0";
            }
            else
            {
                cabeceraDatos = hex[210].ToString() + hex[211].ToString() + hex[208].ToString() + hex[209].ToString();
            }
            
            //Console.WriteLine(cabeceraDatos);

            int cabeceraDatosInt = Convert.ToInt32(cabeceraDatos,16);
            //Console.WriteLine(cabeceraDatosInt);
            string punterosyTexto = "";
            for (int i = cabeceraDatosInt*2; i < hex.Length; i++)
            {
                punterosyTexto += string.Format("{0:X2}", hex[i]);
            }

            //Console.WriteLine(cabeceraDatosInt);
            //Console.WriteLine(punterosyTexto);

            MatchCollection punterosMatch;

            if (archivoHex.Contains("ds_eventm"))
            {
                punterosMatch = Regex.Matches(punterosyTexto, "(?:....0000)", RegexOptions.RightToLeft);
            }
            else
            {
                punterosMatch = Regex.Matches(punterosyTexto, "....0{4}(0{2})(?!0*?$)", RegexOptions.RightToLeft);
            }

            string punteros = punterosyTexto.Substring(0, punterosMatch[0].Groups[0].Index);

            //Console.WriteLine(punteros);

            string texto = "";

            if(archivoHex.Contains("ds_eventm") == true)
            {
                texto = punterosyTexto.Substring(punterosMatch[0].Groups[0].Index+4);
            }
            else if (archivoHex.Contains("ds_event.cmp") == true)
            {
                texto = punterosyTexto.Substring(punterosMatch[0].Groups[0].Index+10);
            }
            else
            {
                texto = punterosyTexto.Substring(punterosMatch[0].Groups[1].Index + 2);
            }


            var conversionRegex = new Regex(string.Join("|", conversion.Keys));
            string textoConvertido = conversionRegex.Replace(texto, m => conversion[m.Value]);

            //Console.WriteLine(textoConvertido);

            return textoConvertido;
        }

    }
}
