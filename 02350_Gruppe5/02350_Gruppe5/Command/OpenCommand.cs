using _02350_Gruppe5.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _02350_Gruppe5.Command
{
    class OpenCommand
    {
        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<Edge> edges;
        private ToSave toSave;

        public OpenCommand(ObservableCollection<ClassBox> _classBoxs, ObservableCollection<Edge> _edges)
        {
            classBoxs = _classBoxs;
            edges = _edges;
            
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "XML documents (.xml)|*.xml"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                classBoxs.Clear();
                edges.Clear();
                
                // Open document 
                string filename = dlg.FileName;


                // Create an instance of the XmlSerializer class;
                // specify the type of object to be deserialized.
                XmlSerializer serializer = new XmlSerializer(typeof(ToSave));
                /* If the XML document has been altered with unknown nodes or attributes,
                   handle them with the UnknownNode and UnknownAttribute events.*/
                serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                // A FileStream is needed to read the XML document.
                FileStream fs = new FileStream(filename, FileMode.Open);
                // Declare an object variable of the type to be deserialized.
                
                /* Use the Deserialize method to restore the object's state with data from the XML document. */
                toSave = (ToSave)serializer.Deserialize(fs);
                deserializeClassBox();
                deserializeEdge();
            }
        }
        private void deserializeClassBox()
        {
            foreach (ClassBoxSave cs in toSave.classes)
            {
                ClassBox newCb = new ClassBox(cs.number) { X = cs.x, Y = cs.y, Width = cs.width, Height = cs.height, 
                    ClassName  = cs.name};
                List<string> attNames, methodNames;
                attNames = new List<string>();
                methodNames = new List<string>();

                foreach (String att in cs.att)
                {
                    attNames.Add(att);
                }
                foreach (String met in cs.method)
                {
                    methodNames.Add(met);
                }
                newCb.AttNames = attNames;
                newCb.MethodNames = methodNames;

                classBoxs.Add(newCb);
            }
        }
        private void deserializeEdge()
        {
            foreach (EdgeSave ed in toSave.edges)
            {
                ClassBox endA=null;
                ClassBox endB=null;
                foreach (ClassBox cl in classBoxs)
                {
                    if (cl.Number == ed.a)
                    {
                        endA = cl;
                    }
                    else if (cl.Number == ed.b)
                    {
                        endB = cl;
                    }
                    
                }
                Edge edge = new Edge(endA,endB);
                edges.Add(edge);
            }
        }
        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
       
    }
}
