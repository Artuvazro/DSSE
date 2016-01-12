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
            static Dictionary<string,string> conversionES = new Dictionary<string, string>()
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
                {"3C6E643D30313E","[nd=01]"}
            };

        //Enable Russian support.
        static Dictionary<string, string> conversionRU = new Dictionary<string, string>()
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
                {"8240","!?"},
                {"81F4","??"},
                {"8148","?"},
                {"8149","!"},
                {"8168","“"},
                {"817B","+"},
                {"817C","-"},
                {"8193","%"},
                {"8260","А"},
                {"8261","Б"},
                {"8262","В"},
                {"8263","Г"},
                {"8264","Д"},
                {"8265","Е"},
                {"8266","Ё"},
                {"8267","Ж"},
                {"8268","З"},
                {"8269","И"},
                {"826A","Й"},
                {"826B","К"},
                {"826C","Л"},
                {"826D","М"},
                {"826E","Н"},
                {"826F","О"},
                {"8270","П"},
                {"8271","Р"},
                {"8272","С"},
                {"8273","Т"},
                {"8274","У"},
                {"8275","Ф"},
                {"8276","Х"},
                {"8277","Ц"},
                {"8278","Ч"},
                {"8279","Ш"},
                {"8281","Щ"},
                {"8282","Ъ"},
                {"8283","Ы"},
                {"8284","Ь"},
                {"8285","Э"},
                {"8286","Ю"},
                {"8287","Я"},
                {"8288","а"},
                {"8289","б"},
                {"828A","в"},
                {"828B","г"},
                {"828C","д"},
                {"828D","е"},
                {"828E","ё"},
                {"828F","ж"},
                {"8290","з"},
                {"8291","и"},
                {"8292","й"},
                {"8293","к"},
                {"8294","л"},
                {"8295","м"},
                {"8296","н"},
                {"8297","о"},
                {"8298","п"},
                {"8299","р"},
                {"829A","с"},
                {"81DD","т"},
                {"81DE","у"},
                {"81DF","ф"},
                {"81E0","х"},
                {"81E1","ц"},
                {"81E2","ч"},
                {"81E3","ш"},
                {"81E4","щ"},
                {"81E5","ъ"},
                {"81E6","ы"},
                {"81E7","ь"},
                {"81E8","э"},
                {"81F0","ю"},
                {"81F1","я"},
                {"8141","A"},
                {"8142","B"},
                {"817D","C"},
                {"8145","D"},
                {"814B","E"},
                {"81F2","F"},
                {"814C","G"},
                {"814D","H"},
                {"814E","I"},
                {"814F","J"},
                {"8152","K"},
                {"81F3","L"},
                {"8153","M"},
                {"81F5","N"},
                {"8154","O"},
                {"8155","P"},
                {"8156","Q"},
                {"81F6","R"},
                {"81F7","S"},
                {"8157","T"},
                {"8158","U"},
                {"8159","V"},
                {"81FC","W"},
                {"815A","X"},
                {"81BE","Y"},
                {"81BF","Z"},
                {"815B","a"},
                {"8165","b"},
                {"8166","c"},
                {"8161","d"},
                {"8162","e"},
                {"8163","f"},
                {"8164","g"},
                {"8173","h"},
                {"8174","i"},
                {"8175","j"},
                {"8176","k"},
                {"8177","l"},
                {"8178","m"},
                {"8179","n"},
                {"817A","o"},
                {"8185","p"},
                {"8186","q"},
                {"8188","r"},
                {"818B","s"},
                {"818C","t"},
                {"818D","u"},
                {"81CD","v"},
                {"81CE","w"},
                {"81DA","x"},
                {"81DB","y"},
                {"81DC","z"},
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
                {"3C6E643D30313E","[nd=01]"}




        };

        public static Dictionary<string, string> conversion;

        public static string leer(string archivoHex)
        {

            switch (Properties.Settings.Default.selectedLang)
            {
                case 0:
                    conversion = conversionES;
                    break;
                case 1:
                    conversion = conversionRU;
                    break;
            }


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
