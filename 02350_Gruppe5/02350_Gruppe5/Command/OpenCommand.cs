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
    //
    // Class used to load a saved diagram. 
    //

    class OpenCommand
    {
        private ObservableCollection<ClassBox> classBoxs;
        private ObservableCollection<Edge> edges;
        private ToSave toSave;

        public OpenCommand(ObservableCollection<ClassBox> _classBoxs, ObservableCollection<Edge> _edges, string filename)
        {
            classBoxs = _classBoxs;
            edges = _edges;

            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(ToSave));

            /* If the XML document has been altered with unknown nodes or attributes,
            handle them with the UnknownNode and UnknownAttribute events.*/
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
            
            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(filename, FileMode.Open);
       
            /* Use the Deserialize method to restore the object's state with data from the XML document. */
            toSave = (ToSave)serializer.Deserialize(fs);
            deserializeClassBox();
            deserializeEdge();
        }
        
        //Method to deserialize all the classes. 
        private void deserializeClassBox()
        {
            foreach (ClassBoxSave cs in toSave.classes)
            {
                ClassBox newCb = new ClassBox(cs.number)
                {
                    X = cs.x,
                    Y = cs.y,
                    Width = cs.width,
                    Height = cs.height,
                    ClassName = cs.name
                };
                List<ClassBox.attOrMethodName> attNames, methodNames;
                attNames = new List<ClassBox.attOrMethodName>();
                methodNames = new List<ClassBox.attOrMethodName>();

                foreach (String att in cs.att)
                {
                    ClassBox.attOrMethodName toAdd = new ClassBox.attOrMethodName(att);
                    attNames.Add(toAdd);
                }
                foreach (String met in cs.method)
                {
                    ClassBox.attOrMethodName toAdd = new ClassBox.attOrMethodName(met);
                    methodNames.Add(toAdd);
                }
                newCb.AttNamesClass = attNames;
                newCb.MethodNamesClass = methodNames;

                classBoxs.Add(newCb);
            }
        }
        //Method to deserialize all the edges.
        private void deserializeEdge()
        {
            foreach (EdgeSave ed in toSave.edges)
            {
                ClassBox endA = null;
                ClassBox endB = null;
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
                Edge edge = new Edge(endA, endB);
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
