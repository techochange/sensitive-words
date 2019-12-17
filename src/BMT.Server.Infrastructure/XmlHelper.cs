using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BMT.Server.Infrastructure
{
    /// <summary>
    /// XML操作帮助类
    /// </summary>
    public static class XmlHelper
    {
        #region XML操作

        /// <summary>
        /// 获取子节点的text值
        /// </summary>
        /// <example>
        /// <code lang="xml">
        /// <![CDATA[
        ///     <root>
        ///         <ID>1</ID>
        ///         <Name>test</Name>
        ///     </root>
        /// ]]>
        /// </code>
        /// <code lang="c#">
        /// <![CDATA[
        ///     XmlNode rootNode=xmlDoc.DocumentElement.SelectSingleNode("//root");
        ///     int id=rootNode.GetChildNodeInnerText<int>("ID");
        ///     string name=rootNode.GetChildNodeInnerText<string>("Name");
        /// ]]>
        /// </code>
        /// 
        /// </example>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="xmlNode">当前节点</param>
        /// <param name="childNodeName">子节点名</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns>返回值</returns>
        public static T GetChildNodeInnerText<T>(this XmlNode xmlNode, string childNodeName, T defaultVal = default(T))
        {
            if (xmlNode == null)
            {
                return defaultVal;
            }
            XmlNode child = xmlNode.SelectSingleNode(childNodeName);
            if (child == null)
            {
                return defaultVal;
            }
            return child.InnerText.Trim().ToType<T>();
        }


        /// <summary>
        /// 获取XmlNode节点的值，返回指定类型
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="xmlNode">xml节点</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>返回值</returns>
        public static T GetXmlNodeValue<T>(this XmlNode xmlNode, T defaultValue)
        {
            if (xmlNode == null)
            {
                return defaultValue;
            }
            return xmlNode.Value.ToType<T>(defaultValue);
        }

        /// <summary>
        /// 获取XmlNode节点的属性值，返回指定类型
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="xmlNode">xml节点</param>
        /// <param name="attrName">属性名</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <returns>返回值</returns>
        public static T GetXmlNodeAttrValue<T>(this XmlNode xmlNode, string attrName, T defaultValue)
        {
            if (xmlNode == null || string.IsNullOrWhiteSpace(attrName))
            {
                return defaultValue;
            }

            XmlAttribute attr = xmlNode.Attributes[attrName];
            if (attr == null || string.IsNullOrWhiteSpace(attr.Value))
            {
                return defaultValue;
            }
            return attr.Value.ToType<T>(defaultValue);
        }


        #region XML文档节点查询和读取

        /// <summary>
        /// 选择匹配XPath表达式的第一个节点XmlNode.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNode</returns>
        public static XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.
#if NETSTANDARD1_6
                LoadFromFile(xmlFileName);
#else
                Load(xmlFileName); //加载XML文档
#endif
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                return xmlNode;
            }
            catch/* (Exception ex)*/
            {
                return null;
            }
        }

        /// <summary>
        /// 选择匹配XPath表达式的节点列表XmlNodeList.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNodeList</returns>
        public static XmlNodeList GetXmlNodeListByXpath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadFileEx(xmlFileName);
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                return xmlNodeList;
            }
            catch /*(Exception ex)*/
            {
                return null;
            }
        }

        /// <summary>
        /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <returns>返回xmlAttributeName</returns>
        public static XmlAttribute GetXmlAttribute(string xmlFileName, string xpath, string xmlAttributeName)
        {
            string content = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            XmlAttribute xmlAttribute = null;
            try
            {
                xmlDoc.LoadFileEx(xmlFileName);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    if (xmlNode.Attributes.Count > 0)
                    {
                        xmlAttribute = xmlNode.Attributes[xmlAttributeName];
                    }
                }
            }
            catch /*(Exception ex)*/
            {
                return null;
            }
            return xmlAttribute;
        }

        #endregion

        #region XML文档创建和节点或属性的添加、修改

        /// <summary>
        /// 创建一个XML文档
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="rootNodeName">XML文档根节点名称(须指定一个根节点名称)</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateXmlDocument(string xmlFileName, string rootNodeName)
        {
            return CreateXmlDocument(xmlFileName, rootNodeName, "1.0", "utf-8", null);
        }

        /// <summary>
        /// 创建一个XML文档
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="rootNodeName">XML文档根节点名称(须指定一个根节点名称)</param>
        /// <param name="version">XML文档版本号(必须为:"1.0")</param>
        /// <param name="encoding">XML文档编码方式</param>
        /// <param name="standalone">该值必须是"yes"或"no",如果为null,Save方法不在XML声明上写出独立属性</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateXmlDocument(string xmlFileName, string rootNodeName, string version, string encoding, string standalone)
        {
            bool isSuccess = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration(version, encoding, standalone);
                XmlNode root = xmlDoc.CreateElement(rootNodeName);
                xmlDoc.AppendChild(xmlDeclaration);
                xmlDoc.AppendChild(root);
                isSuccess = xmlDoc.SaveFileEx(xmlFileName);//保存到XML文档
                isSuccess = true;
            }
            catch/* (Exception ex)*/
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建它的子节点(如果此节点已存在则追加一个新的同名节点)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
        /// <param name="innerText">节点文本值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText)
        {
            return CreateXmlNodeByXPath(xmlFileName, xpath, xmlNodeName, innerText, string.Empty, string.Empty);
        }

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建它的子节点(如果此节点已存在则追加一个新的同名节点)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
        /// <param name="innerText">节点文本值</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText, string xmlAttributeName, string value)
        {
            bool isSuccess = false;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadFileEx(xmlFileName); //加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //存不存在此节点都创建
                    XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                    subElement.InnerXml = innerText;

                    //如果属性和值参数都不为空则在此新节点上新增属性
                    if (!string.IsNullOrEmpty(xmlAttributeName) && !string.IsNullOrEmpty(value))
                    {
                        XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                        xmlAttribute.Value = value;
                        subElement.Attributes.Append(xmlAttribute);
                    }

                    xmlNode.AppendChild(subElement);
                }
                isSuccess = xmlDoc.SaveFileEx(xmlFileName); //保存到XML文档
            }
            catch/* (Exception ex)*/
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建或更新它的子节点(如果节点存在则更新,不存在则创建)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
        /// <param name="innerText">节点文本值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateOrUpdateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText)
        {
            bool isSuccess = false;
            bool isExistsNode = false;//标识节点是否存在
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadFileEx(xmlFileName); //加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //遍历xpath节点下的所有子节点
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        if (node.Name.ToLower() == xmlNodeName.ToLower())
                        {
                            //存在此节点则更新
                            node.InnerXml = innerText;
                            isExistsNode = true;
                            break;
                        }
                    }
                    if (!isExistsNode)
                    {
                        //不存在此节点则创建
                        XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                        subElement.InnerXml = innerText;
                        xmlNode.AppendChild(subElement);
                    }
                }
                xmlDoc.SaveFileEx(xmlFileName); //保存到XML文档
                isSuccess = true;
            }
            catch/* (Exception ex)*/
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建或更新它的属性(如果属性存在则更新,不存在则创建)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateOrUpdateXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName, string value)
        {
            bool isSuccess = false;
            bool isExistsAttribute = false;//标识属性是否存在
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadFileEx(xmlFileName); //加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //遍历xpath节点中的所有属性
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                        {
                            //节点中存在此属性则更新
                            attribute.Value = value;
                            isExistsAttribute = true;
                            break;
                        }
                    }
                    if (!isExistsAttribute)
                    {
                        //节点中不存在此属性则创建
                        XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                        xmlAttribute.Value = value;
                        xmlNode.Attributes.Append(xmlAttribute);
                    }
                }
                xmlDoc.SaveFileEx(xmlFileName); //保存到XML文档
                isSuccess = true;

            }
            catch /*(Exception ex)*/
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        #endregion

#if !NETSTANDARD1_6
        /// <summary>
        /// 通过xml文件路径加载XmlDocument对象 (.net core 中 XmlDocument的Load方法不能直接通过文件路径加载xml对象)
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="xmlFilePath">xml物理路径</param>
        /// <returns></returns>
        public static void LoadFromFile(this XmlDocument xmlDoc, string xmlFilePath)
        {
            xmlDoc.Load(xmlFilePath); //加载XML文档   
        }

        /// <summary>
        /// 保存XmlDocument到文件
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="xmlFilePath">xml物理路径</param>
        /// <returns></returns>
        public static bool SaveAsFile(this XmlDocument xmlDoc, string xmlFilePath)
        {
            string parentPath = Path.GetDirectoryName(xmlFilePath);
            if (!string.IsNullOrWhiteSpace(parentPath) && !Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }
            xmlDoc.Save(xmlFilePath);
            return true;
        }
#else
        /// <summary>
        /// 通过xml文件路径加载XmlDocument对象 (.net core 中 XmlDocument的Load方法不能直接通过文件路径加载xml对象)
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="xmlFilePath">xml物理路径</param>
        /// <returns></returns>
        public static void LoadFromFile(this XmlDocument xmlDoc, string xmlFilePath)
        {
            if (xmlFilePath.IsUrl())
            {
                Task<Stream> task = new HttpClient().GetStreamAsync(xmlFilePath);
                using (Stream stream = task.Result)
                {
                    xmlDoc.Load(stream); //加载XML文档   
                }

                return;
            }

            if (!File.Exists(xmlFilePath))
            {
                throw new FileNotFoundException(xmlFilePath);
            }
            using (Stream stream = File.OpenRead(xmlFilePath))
            {
                if (stream == null)
                {
                    throw new NotSupportedException(xmlFilePath + "不是合法的xml文件");
                }
                xmlDoc.Load(stream); //加载XML文档                   
            }
        }

        /// <summary>
        /// 保存XmlDocument到文件
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="xmlFilePath">xml物理路径</param>
        /// <returns></returns>
        public static bool SaveAsFile(this XmlDocument xmlDoc, string xmlFilePath)
        {
            string parentPath = Path.GetDirectoryName(xmlFilePath);
            if (!string.IsNullOrWhiteSpace(parentPath) && !Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }
            try
            {
                using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    xmlDoc.Save(fileStream);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

#endif

        #region XML文档节点或属性的删除

        /// <summary>
        /// 删除匹配XPath表达式的第一个节点(节点中的子元素同时会被删除)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool DeleteXmlNodeByXPath(string xmlFileName, string xpath)
        {
            bool isSuccess = false;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadFileEx(xmlFileName); //加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //删除节点
                    xmlNode.ParentNode.RemoveChild(xmlNode);
                }

                isSuccess = xmlDoc.SaveFileEx(xmlFileName); //保存到XML文档
            }
            catch /*(Exception ex)*/
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除匹配XPath表达式的第一个节点中的匹配参数xmlAttributeName的属性
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要删除的xmlAttributeName的属性名称</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool DeleteXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName)
        {
            bool isSuccess = false;
            bool isExistsAttribute = false;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadFileEx(xmlFileName); //加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                XmlAttribute xmlAttribute = null;
                if (xmlNode != null)
                {
                    //遍历xpath节点中的所有属性
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                        {
                            //节点中存在此属性
                            xmlAttribute = attribute;
                            isExistsAttribute = true;
                            break;
                        }
                    }
                    if (isExistsAttribute)
                    {
                        //删除节点中的属性
                        xmlNode.Attributes.Remove(xmlAttribute);
                    }
                }

                isSuccess = xmlDoc.SaveFileEx(xmlFileName); //保存到XML文档
            }
            catch/* (Exception ex)*/
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除匹配XPath表达式的第一个节点中的所有属性
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool DeleteAllXmlAttributeByXPath(string xmlFileName, string xpath)
        {
            bool isSuccess = false;
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadFileEx(xmlFileName); //加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                if (xmlNode != null)
                {
                    //遍历xpath节点中的所有属性
                    xmlNode.Attributes.RemoveAll();
                }
                isSuccess = xmlDoc.SaveFileEx(xmlFileName); //保存到XML文档
            }
            catch /*(Exception ex)*/
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        #endregion

        #endregion

        #region XML序列化和反序列化

        /// <summary>
        /// XML序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns>XML格式数据</returns>
        public static string Serialize<T>(T t)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(t.GetType());
                xz.Serialize(sw, t);
                return sw.ToString();
            }
        }

        /// <summary>
        /// XML序列化对象(Datatable)
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="modelName">对象名</param>
        /// <returns>XML格式数据</returns>
        public static string SerializeDataTable(DataTable dt, string modelName)
        {
#if NETSTANDARD1_6
            throw new NotSupportedException("不支持序列化成Datatable");
#else
            StringBuilder strBuilder = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(strBuilder);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            dt.TableName = modelName;
            serializer.Serialize(writer, dt);
            writer.Close();
            return strBuilder.ToString();
#endif
        }

        /// <summary>
        /// XML反序列化(支持泛型类型)
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">XML格式数据</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(string xml)
        {
#if NETSTANDARD1_6
            return DeserializeToEntity<T>(xml);
#else
            if (typeof(T).IsGenericType)
            {
                return DeserializeToEntityList<T>(xml);
            }
            else
            {
                return DeserializeToEntity<T>(xml);
            }
#endif
        }

        /// <summary> 
        /// 反序列化DataTable 
        /// </summary> 
        /// <param name="xml">XML格式数据</param> 
        /// <returns>DataTable</returns> 
        public static DataTable DeserializeDataTable(string xml)
        {
            StringReader strReader = new StringReader(xml);
            XmlReader xmlReader = XmlReader.Create(strReader);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            return serializer.Deserialize(xmlReader) as DataTable;

        }

        private static List<T> DeserializeToList<T>(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            string nodeName = typeof(T).Name.ToLower();
            List<T> list = new List<T>();
            foreach (XmlNode node in document.GetElementsByTagName(nodeName))
            {
                list.Add(Deserialize<T>(node.OuterXml));
            }
            return list;
        }

        private static T DeserializeToEntity<T>(string xml)
        {
            using (StringReader reader = new StringReader(xml))
            {
                XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                T obj = (T)xs.Deserialize(reader);
                return obj;
            }
        }

#if !NETSTANDARD1_6
        private static T DeserializeToEntityList<T>(string xml)
        {
            var method = typeof(XmlHelper).GetMethod("DeserializeToList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).MakeGenericMethod(typeof(T).GetGenericArguments()[0]);
            return (T)method.Invoke(null, new object[] { xml });
        }
#endif
        #endregion

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="xmlFileName"></param>
        private static void LoadFileEx(this XmlDocument xmlDoc, string xmlFileName)
        {
            xmlDoc.
#if NETSTANDARD1_6
            LoadFromFile(xmlFileName);
#else
            Load(xmlFileName); //加载XML文档
#endif
        }

        /// <summary>
        /// 储存文件
        /// </summary>
        /// <param name="xmlFileName"></param>
        private static bool SaveFileEx(this XmlDocument xmlDoc, string xmlFileName)
        {
#if NETSTANDARD1_6
            return xmlDoc.SaveAsFile(xmlFileName);
#else
            xmlDoc.Save(xmlFileName); //保存到XML文档
            return true;
#endif           
        }

    }
}
