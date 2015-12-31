using DarthNemesis;
using Devil_Survivor_Script_Editor.LZ77;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Devil_Survivor_Script_Editor
{
    class generateOutput
    {

        static Dictionary<string, string> conversion = new Dictionary<string, string>()
            {
                {@"91F0","91F0"},
                {@"8360","8360"},
                {@"_","8151"},
                {@"/","815E"},
                {@" ","8140"},
                {@",","8143"},
                {@".","8144"},
                {@":","8146"},
                {@";","8147"},
                {@"!?","8240"},
                {@"??","81F4"},
                {@"¡¿","81F5"},
                {@"¡¡","81F6"},
                {@"?","8148"},
                {@"!","8149"},
                {@"'","8166"},
                {@"“","8168"},
                {@"+","817B"},
                {@"-","817C"},
                {@"%","8193"},
                {@"A","8260"},
                {@"B","8261"},
                {@"C","8262"},
                {@"D","8263"},
                {@"E","8264"},
                {@"F","8265"},
                {@"G","8266"},
                {@"H","8267"},
                {@"I","8268"},
                {@"J","8269"},
                {@"K","826A"},
                {@"L","826B"},
                {@"M","826C"},
                {@"N","826D"},
                {@"O","826E"},
                {@"P","826F"},
                {@"Q","8270"},
                {@"R","8271"},
                {@"S","8272"},
                {@"T","8273"},
                {@"U","8274"},
                {@"V","8275"},
                {@"W","8276"},
                {@"X","8277"},
                {@"Y","8278"},
                {@"Z","8279"},
                {@"a","8281"},
                {@"b","8282"},
                {@"c","8283"},
                {@"d","8284"},
                {@"e","8285"},
                {@"f","8286"},
                {@"g","8287"},
                {@"h","8288"},
                {@"i","8289"},
                {@"j","828A"},
                {@"k","828B"},
                {@"l","828C"},
                {@"m","828D"},
                {@"n","828E"},
                {@"o","828F"},
                {@"p","8290"},
                {@"q","8291"},
                {@"r","8292"},
                {@"s","8293"},
                {@"t","8294"},
                {@"u","8295"},
                {@"v","8296"},
                {@"w","8297"},
                {@"x","8298"},
                {@"y","8299"},
                {@"z","829A"},
                {@"¿","81DF"},
                {@"Á","81E0"},
                {@"É","81E1"},
                {@"Í","81E2"},
                {@"Ó","81E3"},
                {@"Ú","81E4"},
                {@"¡","81E5"},
                {@"ñ","81E6"},
                {@"á","81DE"},
                {@"é","81E8"},
                {@"í","81F0"},
                {@"ó","81F1"},
                {@"ú","81F2"},
                {@"ü","81F3"},
                {@"·","8145"},
                {@"&","8195"},
                {@"1","8250"},
                {@"2","8251"},
                {@"3","8252"},
                {@"4","8253"},
                {@"5","8254"},
                {@"6","8255"},
                {@"7","8256"},
                {@"8","8257"},
                {@"9","8258"},
                {@"0","824F"},
                {@"(","8169"},
                {@")","816A"},
                {@"*","8196"},
                {@"!!","8241"},
                {@"[00]","00"},
                {@"[br]","3C62723E"},
                {@"[c=05]","3C633D30353E"},
                {@"[cn=000D]","3C636E3D303030443E"},
                {@"[cn=0000]","3C636E3D303030303E"},
                {@"[cn=000C]","3C636E3D303030433E"},
                {@"[new]","3C703E"},
                {@"[color1]","3C633D30393E"},
                {@"[/color]","3C633D46463E"},
                {@"[color2]","3C633D30363E"},
                {@"[nd=00]","3C6E643D30303E"},
                {@"[nd=01]","3C6E643D30313E"},


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

        public static int fileNumber;

        public static void generate(string path)
        {
            
            List<string> filesTranslated = new List<string>();
            filesTranslated.AddRange(Directory.GetFiles("./DSSE-files/translation/", "*.xml"));
            fileNumber = filesTranslated.Count();
            List<string> filesTranslatedNames = new List<string>();

            for (int i = 0; i < fileNumber; i++)
            {
                filesTranslatedNames.Add(Path.GetFileNameWithoutExtension(filesTranslated[i]));
            }

            //Console.WriteLine(filesTranslatedNames[0]);



            for (int i = 0; i < fileNumber; i++)
            {


                FileStream fs = new FileStream(path + filesTranslatedNames[i] + ".cmp", FileMode.Open);
                int hexIn;
                string hex = "";
                StringBuilder hex2 = new StringBuilder();
                for (int j = 0; (hexIn = fs.ReadByte()) != -1; j++)
                {
                    hex2.Append(string.Format("{0:X2}", hexIn));
                }
                hex = hex2.ToString();
                fs.Dispose();

                byte[] compressedFile = strToByte.StringToByteArray(hex);
                byte[] decompressedFile = CompressionManager.Decompress(compressedFile);

                hex = strToByte.ByteArrayToString(decompressedFile);

                string cabeceraDatos = "";
                if (filesTranslatedNames[i].Equals("ds_event"))
                {
                    cabeceraDatos = hex[212].ToString() + hex[213].ToString() + hex[210].ToString() + hex[211].ToString() + hex[208].ToString() + hex[209].ToString();
                }
                else if (filesTranslatedNames[i].Contains("ds_eventm"))
                {
                    cabeceraDatos = "0";
                }
                else
                {
                    cabeceraDatos = hex[210].ToString() + hex[211].ToString() + hex[208].ToString() + hex[209].ToString();
                }

                int cabeceraDatosInt = Convert.ToInt32(cabeceraDatos, 16);
                
                string binaryData = "";
                StringBuilder binaryData2 = new StringBuilder();
                int k = 0;
                while (k < cabeceraDatosInt*2)
                {
                    binaryData2.Append(string.Format("{0:X2}", hex[k]));
                    k++;
                }
                binaryData = binaryData2.ToString();

                //Console.WriteLine("Datos binarios:" + binaryData);

                XDocument document = XDocument.Load("./DSSE-files/translation/" + filesTranslatedNames[i] + ".xml");
                XElement root = document.Root;

                List<string> lines = new List<string>();

                lines.AddRange(from el in root.Elements("Segment")
                        where el.Element("Translated") != null
                        select el.Element("Translated").Value);

                int numberofTextSegments = 0;
                for (int m = 0; m < lines.Count; m++)
                {
                    lines[m] = lines[m].Replace("\n", "[br]");
                    if (!lines[m].Contains("[new]")) {
                        lines[m] = "[00]" + lines[m];
                        numberofTextSegments++;

                    }
                }

                string allLines = string.Join("", lines);
                allLines += "[00]"; //añadir último 00

                //Console.WriteLine(allLines);

                var conversionRegex = new Regex(string.Join("|", conversion.Keys.Select(key => Regex.Escape(key))));
                var textoConvertido = conversionRegex.Replace(allLines, n => conversion[n.Value]);

                //Console.WriteLine(textoConvertido);

                string punterosyTexto = "";
                StringBuilder punterosyTexto2 = new StringBuilder();
                for (int m = cabeceraDatosInt * 2; m < hex.Length; m++)
                {
                    //punterosyTexto += string.Format("{0:X2}", hex[m]);
                    punterosyTexto2.Append(string.Format("{0:X2}", hex[m]));
                }

                punterosyTexto = punterosyTexto2.ToString();


                MatchCollection punterosMatch;

                if (filesTranslatedNames[i].Contains("ds_eventm"))
                {
                    punterosMatch = Regex.Matches(punterosyTexto, "(?:....0000)", RegexOptions.RightToLeft);
                }
                else
                {
                    punterosMatch = Regex.Matches(punterosyTexto, "....0{4}(0{2})(?!0*?$)", RegexOptions.RightToLeft);
                }

                //Console.WriteLine(4 + punterosMatch[0].Groups[0].Value);

                string punteros = "";
                StringBuilder punteros2 = new StringBuilder();
                for (int p = 0; p < 4+punterosMatch[0].Groups[0].Index; p++)
                {
                    punteros2.Append(string.Format("{0:X2}", punterosyTexto[p]));
                }

                punteros = punteros2.ToString();

                //Console.WriteLine(punteros);

                MatchCollection punterosDiv = Regex.Matches(punteros, "(....)0000");

                //Console.WriteLine(punterosDiv[0].Groups[1] + "\n\r");
                //Console.WriteLine(punterosDiv[1].Groups[1]);

                //El primer valor que hay en punteros es la longitud de punteros + texto. [!] Hay que adaptarlo.
                //El segundo valor no sirve, solo resta -1 respecto al siguiente.
                //El tercer valor (2) es el primer puntero base.

                //En lines[] tengo las líneas convertidas en HEX ya para saber la longitud.

                var pointBase = "";

                if (filesTranslatedNames[i].Contains("ds_eventm")) {
                    pointBase = punterosDiv[1].Groups[1].Value;
                }
                else pointBase = punterosDiv[2].Groups[1].Value;

                //Console.WriteLine("Pointbase: "+pointBase);

                string pointers = "";

                List<string> lineasHex = Regex.Split(textoConvertido.Substring(2), "00(?!0)").ToList();
                //Console.WriteLine(lineasHex[0]);

                StringBuilder pointers2 = new StringBuilder();
                for (int q = 0; q < lineasHex.Count()-2; q++) //-2 porque el último texto no va punteado
                {
                    
                    var _pointBaseLE = pointBase.ToCharArray();
                    var _pointBaseBE = _pointBaseLE[2].ToString() + _pointBaseLE[3].ToString() + _pointBaseLE[0].ToString() + _pointBaseLE[1].ToString();
                    var _pointBaseBEDecimal = Convert.ToInt32(_pointBaseBE, 16);
                    int lineSize = ((lineasHex[q].Length/2)+1);
                    //Console.WriteLine(lineSize + "\n\r");
                    //Console.WriteLine("Point base: "+pointBase.ToString() + "\r\n");
                    var newPointerBE = Convert.ToInt16((_pointBaseBEDecimal + lineSize)).ToString("x").PadLeft(4, '0').ToCharArray();
                    string newPointerLE = newPointerBE[2].ToString() + newPointerBE[3].ToString() + newPointerBE[0].ToString() + newPointerBE[1].ToString();
                    //pointers += newPointerLE.ToString().PadRight(8, '0');
                    pointers2.Append(newPointerLE.ToString().PadRight(8, '0'));
                    pointBase = newPointerLE;
                }

                if (filesTranslatedNames[i].Contains("ds_eventm"))
                {
                    numberofTextSegments = Convert.ToInt16(numberofTextSegments);
                    string numberofTextSegmentsS = numberofTextSegments.ToString("x").PadRight(8, '0');
                    //Console.WriteLine(numberofTextSegmentsS);
                    pointers = numberofTextSegmentsS + punterosDiv[1].Groups[1].Value + "0000" + pointers2.ToString();
                }
                else pointers = punterosDiv[0].Groups[1].Value + "0000" + punterosDiv[1].Groups[1].Value + "0000" + punterosDiv[2].Groups[1].Value + "0000" + pointers2.ToString();

                //Console.WriteLine(pointers);


                //string archivoFinal = pointers + "00" + textoConvertido;

                string archivoFinal = "";

                if ((filesTranslatedNames[i].Equals("ds_event") == false) && (filesTranslatedNames[i].Contains("ds_eventm") == false))
                {
                    archivoFinal = pointers + textoConvertido;
                    
                }
                else if (filesTranslatedNames[i].Contains("ds_eventm") == true)
                {
                    archivoFinal = pointers + textoConvertido.Substring(2); //Elimina los 00 añadidos antes al principio, en este tipo de ficheros sobra.
                }
                else
                {
                    //Este fixedData es para el archivo "ds_event.cmp", tiene esos datos invariables. Para ahorrar su captura los pongo aquí directamente.
                    string fixedData = "7E0000000000000062010000BD0100005E020000DB02000046030000B2030000020400005C040000FC04000077050000D50500004B06000082060000D706000045070000A5070000FC0700003A0800008F080000D80800003F090000E9090000520A00008E0A0000D80A0000370B0000930B0000E80B0000440C0000E50C00001B0D00007B0D0000E30D00004C0E0000A60E0000F70E0000420F0000840F0000F10F000086100000BB1000001F110000E01100003E12000093120000DA1200005F130000C613000079140000F314000071150000BE1500000F1600004D16000099160000EE1600006D170000CD1700001D18000082180000CB1800000919000058190000A5190000E31900004E1A0000D41A0000641B0000DA1B00001D1C0000691C0000AE1C00002A1D0000861D0000F01D00004C1E0000921E0000D21E0000141F0000551F0000F61F00003A2000007D200000CA2000003521000093210000EB21000046220000B8220000362300007F230000D3230000412400008F240000BE2400000425000039250000BC2500001A26000080260000D926000028270000772700001628000075280000B82800000D29000065290000D6290000382A0000AF2A00000D2B0000662B0000C92B00000D2C00005B2C0000B12C0000042D00003B2D00007F2D0000E72D0000232E00006E2E0000C82E00000E2F0000";
                    archivoFinal = pointers + fixedData + textoConvertido;
                }

                var datosLengthArray = Convert.ToInt16((archivoFinal.Length / 2)).ToString("x").PadLeft(4, '0').ToCharArray();
                var datosLength = datosLengthArray[2].ToString() + datosLengthArray[3].ToString() + datosLengthArray[0].ToString() + datosLengthArray[1].ToString();

                

                if(filesTranslatedNames[i].Contains("ds_eventm") == false)
                {
                    archivoFinal = archivoFinal.Remove(0, 4);
                    archivoFinal = archivoFinal.Insert(0, datosLength.ToString());
                    archivoFinal = binaryData + archivoFinal;
                }

                byte[] archivoFinalBin = strToByte.StringToByteArray(archivoFinal);

                //File.WriteAllBytes("./DSSE-files/output/" + filesTranslatedNames[i] + ".decmp", archivoFinalBin);

                byte[] compressedFileOutput = CompressionManager.Compress(archivoFinalBin);


                File.WriteAllBytes("./DSSE-files/output/" + filesTranslatedNames[i] + ".cmp", compressedFileOutput);

                Stream myStreamF = File.Open("./DSSE-files/output/" + filesTranslatedNames[i] + ".cmp", FileMode.Open);
                    myStreamF.Seek(myStreamF.Length, SeekOrigin.Begin);

                    while ((myStreamF.Length) % 4 != 0)
                    {
                        myStreamF.WriteByte(0x00);
                    }
                

                myStreamF.Dispose();

                //Console.WriteLine(archivoFinal);

                //Console.WriteLine("Progreso: " + (i + 1) + " de " + fileNumber);

                MainWindow.generateOutputBGW.ReportProgress(i+1);

            }
        }

    }

}
