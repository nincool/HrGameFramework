using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
//using FGL.Mem;

namespace StaticData
{
    public class EncodingAttribute : Attribute
    {
        public override object TypeId
        {
            get
            {
                return "EncodingAttribute";
            }
        }
    }

    [EncodingAttribute]
    public class EncodingSetting
    {
        private static EncodingSetting m_Instance = null;
        public static EncodingSetting Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new EncodingSetting();
                }
                return m_Instance;
            }
        }

        public void Load()
        {
            try
            {
                XElement xEle = XElement.Load(fileName);
                Load(xEle);
            }
            catch (Exception)
            {
            }
        }

        public void Load(XElement xEle)
        {
            String strCurrent = xEle.Element("Current").Attribute("encoding").Value;
            Current = Encoding.GetEncoding(strCurrent);

            foreach (XElement xSub in xEle.Element("Encodings").Elements())
            {
                try
                {
	                String strEncoding = xSub.Attribute("Type").Value;
	                Encoding encoding = Encoding.GetEncoding(strEncoding);
	                EncodingList.Add(encoding);
                }
                catch (Exception)
                {
                }
            }
        }

        public void Save(String fileName)
        {
            XElement xEle = new XElement("root",
                                new XElement("Current", new XAttribute("encoding", Current.BodyName),
                                                new XAttribute("Name",Current.EncodingName)));
            XElement xList = new XElement("Encodings");
            foreach (Encoding sub in EncodingList)
            {
                XElement xSub = new XElement("Encoding", new XAttribute("Type", sub.BodyName), 
                                                new XAttribute("Name",sub.EncodingName));
                xList.Add(xSub);
            }
            xEle.Add(xList);
            xEle.Save(fileName);
        }

        private Encoding m_Current = Encoding.UTF8;

        public Encoding Current
        {
            get
            {
                return m_Current;
            }
            set
            {
                m_Current = value;
            }
        }

        private List<Encoding> m_EncodingList = null;
        public List<Encoding> EncodingList
        {
            get
            {
                if (m_EncodingList == null)
                {
                    m_EncodingList = new List<Encoding>();
                }
                return m_EncodingList;
            }
        }

        private String fileName = Environment.CurrentDirectory + "\\encoding.xml";

        public bool SetCurrentEncoding(int encodingPage)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodingPage);
                if (encoding != null)
                {
                    Current = encoding;
                    if (!EncodingList.Contains(encoding))
                    {
                        EncodingList.Add(encoding);
                    }
                    Save(fileName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetCurrentEncoding(string encodingName)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodingName);
                if (encoding != null)
                {
                    Current = encoding;
                    if (!EncodingList.Contains(encoding))
                    {
                        EncodingList.Add(encoding);
                    }
                    Save(fileName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Encoding GetEncoding()
        {
            return Instance.Current;
        }

    }
}