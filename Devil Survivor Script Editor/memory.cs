using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Devil_Survivor_Script_Editor
{
    class memory
    {
        public static XElement file = new XElement("tmx");

        public static XElement body = new XElement("body");

        public static void saveToMemory(string originalText, string translatedText)
        {

            if (File.Exists("./DSSE-files/translation-memory.tmx"))
            {

                //Modifica un segmento
                XDocument document = XDocument.Load("./DSSE-files/translation-memory.tmx");
                XElement root = document.Root;

                var appendHere = root.Element("body");

                var lookup = from e in root.Descendants("tuv")
                             where (string)e.Attribute("lang") == "en"
                             where (string)e.Element("seg") == originalText
                             select e;

                //Console.WriteLine(originalText);

                string lookUpValue = "";

                foreach (XElement e in lookup)
                {
                   //Console.WriteLine("This: " + (string)e.Element("seg").Value);
                    lookUpValue = (string)e.Element("seg").Value;
                }



                if (lookUpValue != originalText)
                {
                    //Console.WriteLine("Nuevo");
                    //Agrega nueva entrada

                    XElement tu = new XElement("tu");
                    XElement tuv1 = new XElement("tuv");
                    XAttribute xmllang1 = new XAttribute("lang", "en");
                    XElement seg1 = new XElement("seg", originalText);
                    tuv1.Add(xmllang1);
                    tuv1.Add(seg1);
                    tu.Add(tuv1);
                    XElement tuv2 = new XElement("tuv");
                    XAttribute xmllang2 = new XAttribute("lang", "es");
                    XElement seg2 = new XElement("seg", translatedText);
                    tuv2.Add(xmllang2);
                    tuv2.Add(seg2);
                    tu.Add(tuv2);
                    appendHere.Add(tu);

                    document.Save("./DSSE-files/translation-memory.tmx", SaveOptions.None);
                    file.RemoveAll();
                }
                else
                {
                    //Editar el valor existente.
                    //Console.WriteLine("Repetido");

                    var edit = from e in root.Descendants("tuv")
                             where (string)e.Attribute("lang") == "en"
                             where (string)e.Element("seg") == originalText
                             select e.NextNode;


                    foreach (XElement e in edit)
                    {
                        //Console.WriteLine("This: " + (string)e.Element("seg").Value);
                        e.Element("seg").SetValue(translatedText);
                    }

                    document.Save("./DSSE-files/translation-memory.tmx", SaveOptions.None);
                }

            }
            else //Genera la plantilla del archivo vacío.
            {
                XAttribute tmxversion = new XAttribute("version", "1.4");
                XElement header = new XElement("header");
                XAttribute headerContent = new XAttribute("creationtool", "DSSE");
                XAttribute headerContent2 = new XAttribute("creationtoolversion", "ALPHA");
                XAttribute headerContent3 = new XAttribute("datatype", "PlainText");
                XAttribute headerContent4 = new XAttribute("segtype", "sentence");
                XAttribute headerContent5 = new XAttribute("adminlang", "en-US");
                XAttribute headerContent6 = new XAttribute("srclang", "en-US");

                file.Add(tmxversion);
                file.Add(header);
                header.Add(headerContent);
                header.Add(headerContent2);
                header.Add(headerContent3);
                header.Add(headerContent4);
                header.Add(headerContent5);
                header.Add(headerContent6);

                XElement tu = new XElement("tu");
                XElement tuv1 = new XElement("tuv");
                XAttribute xmllang1 = new XAttribute("lang", "en");
                XElement seg1 = new XElement("seg", originalText);
                tuv1.Add(xmllang1);
                tuv1.Add(seg1);
                tu.Add(tuv1);
                XElement tuv2 = new XElement("tuv");
                XAttribute xmllang2 = new XAttribute("lang", "es");
                XElement seg2 = new XElement("seg", translatedText);
                tuv2.Add(xmllang2);
                tuv2.Add(seg2);
                tu.Add(tuv2);
                body.Add(tu);
                file.Add(body);


                file.Save("./DSSE-files/translation-memory.tmx", SaveOptions.None);

                file.RemoveAll();
            }

        }
    }
}
