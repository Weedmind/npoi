// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.38967
//    <NameSpace>schemas</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>False</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net20</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>False</GenerateXMLAttributes><EnableEncoding>False</EnableEncoding><AutomaticProperties>False</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>False</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NPOI.OpenXmlFormats.Dml;
using System.Xml;
using System.IO;
using NPOI.OpenXml4Net.Util;


namespace NPOI.OpenXmlFormats.Spreadsheet
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    public enum ST_GradientType
    {
        NONE,
        linear,
        path,
    }
    public class ST_UnsignedshortHex
    {
        string stringValueField = null;
        public string StringValue
        {
            get { return stringValueField; }
            set { stringValueField = value; }
        }
        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }
    }

    public class CT_GradientStop
    {
        private int positionField;
        private CT_Color colorField;

        public static CT_GradientStop Parse(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            if (node == null)
                return null;
            CT_GradientStop ctObj = new CT_GradientStop();
            ctObj.positionField = XmlHelper.ReadInt(node.Attributes["position"]);
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.LocalName == "color")
                {
                    ctObj.colorField = CT_Color.Parse(childNode, namespaceManager);
                    break;
                }
            }
            return ctObj;
        }
        internal void Write(StreamWriter sw, string nodeName)
        {
            sw.Write(string.Format("<{0}", nodeName));
            XmlHelper.WriteAttribute(sw, "position", this.positionField, true);
            sw.Write(">");
            if (this.colorField != null)
            {
                this.colorField.Write(sw, "color");
            }
            sw.Write(string.Format("</{0}>", nodeName));
        }
    }

    [Serializable]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    public class CT_GradientFill
    {

        private List<CT_GradientStop> stopField = null; // 0..*

        private ST_GradientType typeField = ST_GradientType.linear;

        private double degreeField = 0.0;

        private double leftField = 0.0;

        private double rightField = 0.0;

        private double topField = 0.0;

        private double bottomField = 0.0;
        public static CT_GradientFill Parse(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            if (node == null)
                return null;
            CT_GradientFill ctObj = new CT_GradientFill();
            if (node.Attributes["type"] != null)
                ctObj.type = (ST_GradientType)Enum.Parse(typeof(ST_GradientType), node.Attributes["type"].Value);
            ctObj.degree = XmlHelper.ReadDouble(node.Attributes["degree"]);
            ctObj.left = XmlHelper.ReadDouble(node.Attributes["left"]);
            ctObj.right = XmlHelper.ReadDouble(node.Attributes["right"]);
            ctObj.top = XmlHelper.ReadDouble(node.Attributes["top"]);
            ctObj.bottom = XmlHelper.ReadDouble(node.Attributes["bottom"]);
            ctObj.stop = new List<CT_GradientStop>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.LocalName == "stop")
                    ctObj.stop.Add(CT_GradientStop.Parse(childNode, namespaceManager));
            }
            return ctObj;
        }



        internal void Write(StreamWriter sw, string nodeName)
        {
            sw.Write(string.Format("<{0}", nodeName));
            XmlHelper.WriteAttribute(sw, "type", this.type.ToString());
            XmlHelper.WriteAttribute(sw, "degree", this.degree);
            XmlHelper.WriteAttribute(sw, "left", this.left);
            XmlHelper.WriteAttribute(sw, "right", this.right);
            XmlHelper.WriteAttribute(sw, "top", this.top);
            XmlHelper.WriteAttribute(sw, "bottom", this.bottom);
            sw.Write(">");
            if (this.stop != null)
            {
                foreach (CT_GradientStop x in this.stop)
                {
                    x.Write(sw, "stop");
                }
            }
            sw.Write(string.Format("</{0}>", nodeName));
        }

        [XmlElement]
        public List<CT_GradientStop> stop
        {
            get { return this.stopField; }
            set { this.stopField = value; }
        }

        [XmlAttribute]
        [DefaultValue(ST_GradientType.linear)]
        public ST_GradientType type
        {
            get { return ST_GradientType.NONE == this.typeField ? ST_GradientType.linear : this.typeField; }
            set { this.typeField = value; }
        }

        [XmlAttribute]
        [DefaultValue(0D)]
        public double degree
        {
            get { return degreeField; }
            set { this.degreeField = value; }
        }
        [XmlAttribute]
        [DefaultValue(0D)]
        public double left
        {
            get { return this.leftField; }
            set { this.leftField = value; }
        }

        [XmlAttribute]
        [DefaultValue(0D)]
        public double right
        {
            get { return this.rightField; }
            set { this.rightField = value; }
        }

        [XmlAttribute]
        [DefaultValue(0D)]
        public double top
        {
            get { return this.topField; }
            set { this.topField = value; }
        }

        [XmlAttribute]
        [DefaultValue(0D)]
        public double bottom
        {
            get { return this.bottomField; }
            set { this.bottomField = value; }
        }
    }

    public class CT_XStringElement
    {

        private string vField;

        public string v
        {
            get
            {
                return this.vField;
            }
            set
            {
                this.vField = value;
            }
        }
    }
    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    public class CT_Extension
    {

        private string anyField;

        private string uriField = null;

        public static CT_Extension Parse(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            if (node == null)
                return null;
            CT_Extension ctObj = new CT_Extension();
            ctObj.uri = XmlHelper.ReadString(node.Attributes["uri"]);
            ctObj.Any = node.InnerXml;
            return ctObj;
        }

        public static CT_Extension Parse(XmlReader reader, XmlNamespaceManager namespaceManager)
        {
            if (reader == null)
                return null;
            CT_Extension ctObj = new CT_Extension();
            ctObj.uri = reader.GetAttribute("uri");
            ctObj.Any = reader.ReadInnerXml();
            return ctObj;
        }



        internal void Write(StreamWriter sw, string nodeName)
        {
            sw.Write(string.Format("<{0}", nodeName));
            XmlHelper.WriteAttribute(sw, "uri", this.uri);
            sw.Write(">");
            if (this.Any != null)
                sw.Write(this.Any);
            sw.Write(string.Format("</{0}>", nodeName));
        }


        [XmlText]
        public string Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }
        [XmlAttribute(DataType = "token")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }
        [XmlIgnore]
        public bool uriSpecified
        {
            get { return (null != uriField); }
        }
    }

    [Serializable]
    [System.Diagnostics.DebuggerStepThrough]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main")]
    public class CT_ExtensionList
    {
        private List<CT_Extension> extField = new List<CT_Extension>(); // 0..*

        public CT_ExtensionList Copy()
        {
            CT_ExtensionList obj = new CT_ExtensionList();
            obj.ext = new List<CT_Extension>(this.ext);
            return obj;
        }

        //[XmlArray(Order = 0)]
        [XmlElement]
        public List<CT_Extension> ext
        {
            get
            {
                return this.extField;
            }
            set
            {
                this.extField = value;
            }
        }

        public static CT_ExtensionList Parse(XmlNode node, XmlNamespaceManager namespaceManager)
        {
            if (node == null)
                return null;
            CT_ExtensionList ctObj = new CT_ExtensionList();
            ctObj.ext = new List<CT_Extension>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.LocalName == "ext")
                    ctObj.ext.Add(CT_Extension.Parse(childNode, namespaceManager));
            }
            return ctObj;
        }

        public static CT_ExtensionList Parse(XmlReader reader, XmlNamespaceManager namespaceManager)
        {
            if (reader == null)
                return null;
            CT_ExtensionList ctObj = new CT_ExtensionList();
            ctObj.ext = new List<CT_Extension>();
            while (reader.Read())
            {
                if (reader.Name == "ext")
                    ctObj.ext.Add(CT_Extension.Parse(reader, namespaceManager));
            }
            return ctObj;
        }


        internal void Write(StreamWriter sw, string nodeName)
        {
            sw.Write(string.Format("<{0}>", nodeName));
            if (this.ext != null)
            {
                foreach (CT_Extension x in this.ext)
                {
                    x.Write(sw, "ext");
                }
            }
            sw.Write(string.Format("</{0}>", nodeName));
        }

    }
}
