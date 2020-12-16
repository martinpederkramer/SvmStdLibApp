using System;
using System.Xml;

namespace ConsoleTester
{
    public static class XmlTest
    {
        static void Test(string[] args)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\child.xml");

            //string[] s = { "Document", "SW.Blocks.FB", "AttributeList" };
            //XmlNode blockProperties = FindChildNode(xmlDoc, s);
            //s = new string[] { "Interface", "Sections" };
            //XmlNode blockInterface = FindChildNode(blockProperties, s);
            //s = new string[] { "Document", "SW.Blocks.FB", "ObjectList", "SW.Blocks.CompileUnit", "AttributeList", "NetworkSource", "StructuredText" };
            //XmlNode compileUnit = FindChildNode(xmlDoc, s);

            foreach (XmlNode xmlNode in xmlDoc)
            {
                GetNodes(xmlNode);
            }

            Console.WriteLine();
            Console.WriteLine($"Machine: {Environment.MachineName}");
        }
        public static XmlNode FindChildNode(XmlNode xmlNode, string[] nodenames)
        {
            XmlNode output = xmlNode;
            foreach (string node in nodenames)
            {
                if (output != null)
                    output = FindChildNode(output, node);
            }
            return output;
        }
        public static XmlNode FindChildNode(XmlNode xmlNode, string nodename)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (nodename == node.Name)
                {
                    return node;
                }
            }
            return null;
        }
        public static void GetNodes(XmlNode node)
        {
            if (node == null)
            {
                return;
            }
            Console.WriteLine($"{node.Name} : {node.Value}");
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    Console.WriteLine($"\t{attribute.Name} : {attribute.Value}");
                }
            }
            foreach (XmlNode n in node.ChildNodes)
            {
                GetNodes(n);
            }
        }

    }

}
