using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Devil_Survivor_Script_Editor
{
    class saving
    {
        public static XElement file = new XElement("File");
        public static string previous;

        public static void saveSegment(string archivo, int currentSegment, int maxSegment, List<string> originalText, string translatedText)
        {
            if (previous != archivo) { file.RemoveAll(); }
            previous = archivo;


            if (File.Exists("./DSSE-files/translation/" + archivo + ".xml"))
            {
                //Modifica un segmento
                XDocument document = XDocument.Load("./DSSE-files/translation/" + archivo + ".xml");
                XElement root = document.Root;


                    root.Elements("Segment").Where(e =>
                    e.Attribute("ID").Value.Equals(currentSegment.ToString())).Select(e =>
                    e.Element("Translated")).Single().SetValue(translatedText);

                    document.Save("./DSSE-files/translation/" + archivo + ".xml", SaveOptions.None);

            }
            else //Genera la plantilla del archivo vacío.
            {
                for (int i = 0; i <= maxSegment; i++)
                {

                    XAttribute segmentNumber = new XAttribute("ID", i);
                    XElement original = new XElement("Original", originalText[i]);
                    XElement translated = new XElement("Translated", "");
                    XElement segment = new XElement("Segment");

                    segment.Add(segmentNumber);
                    segment.Add(original);
                    segment.Add(translated);

                    file.Add(segment);
                }
                file.Save("./DSSE-files/translation/" + archivo + ".xml", SaveOptions.None);

                XDocument document = XDocument.Load("./DSSE-files/translation/" + archivo + ".xml");
                XElement root = document.Root;


                root.Elements("Segment").Where(e =>
                e.Attribute("ID").Value.Equals(currentSegment.ToString())).Select(e =>
                e.Element("Translated")).Single().SetValue(translatedText);

                document.Save("./DSSE-files/translation/" + archivo + ".xml", SaveOptions.None);

                file.RemoveAll();
            }           

        }
    }
}
