using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlTool
{
    public enum XmlValueType
    {
        Value,
        InnerText,
        InnerXml,
        OuterXml
    }

    /// <summary>
    /// Xml文件操作对象
    /// </summary>
    public sealed class XmlUtil
    {

        /// <summary>
        /// 通过xPath查询相应的值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xPath">xPath查询</param>
        /// <param name="xmlValueType">值类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public static string GetXmlNodeValue(XmlNode node, string xPath, XmlValueType xmlValueType, string defaultValue)
        {
            if (xPath == "")
                return defaultValue;
            else
                return GetXmlNodeValue(node.SelectSingleNode(xPath), xmlValueType, defaultValue);
        }

        /// <summary>
        /// 通过xPath查询相应的值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xPath">xPath查询</param>
        /// <param name="xmlValueType">值类型</param>
        /// <returns>值</returns>
        public static string GetXmlNodeValue(XmlNode node, string xPath, XmlValueType xmlValueType)
        {
            if (xPath == "")
                return null;
            else
                return GetXmlNodeValue(node.SelectSingleNode(xPath), xmlValueType, null);
        }

        /// <summary>
        /// 通过xPath查询相应的值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xPath">xPath查询</param>
        /// <param name="xmlNamespace"></param>
        /// <param name="xmlValueType">值类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public static string GetXmlNodeValue(XmlNode node, string xPath, XmlNamespaceManager xmlNamespace, XmlValueType xmlValueType, string defaultValue)
        {
            if (xPath == "")
                return defaultValue;
            else
                return GetXmlNodeValue(node.SelectSingleNode(xPath, xmlNamespace), xmlValueType, defaultValue);
        }

        /// <summary>
        /// 通过xPath查询相应的值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xPath">xPath查询</param>
        /// <param name="xmlNamespace"></param>
        /// <param name="xmlValueType">值类型</param>
        /// <returns>值</returns>
        public static string GetXmlNodeValue(XmlNode node, string xPath, XmlNamespaceManager xmlNamespace, XmlValueType xmlValueType)
        {
            if (xPath == "")
                return null;
            else
                return GetXmlNodeValue(node.SelectSingleNode(xPath, xmlNamespace), xmlValueType, null);
        }

        /// <summary>
        /// 获取指定节点的指定值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xmlValueType">值类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public static string GetXmlNodeValue(XmlNode node, XmlValueType xmlValueType, string defaultValue)
        {
            string nodeValue = defaultValue;
            if (node != null)
                try
                {
                    switch (xmlValueType)
                    {
                        case XmlValueType.Value:
                            nodeValue = node.Value;
                            break;
                        case XmlValueType.InnerText:
                            nodeValue = node.InnerText;
                            break;
                        case XmlValueType.InnerXml:
                            nodeValue = node.InnerXml;
                            break;
                        case XmlValueType.OuterXml:
                            nodeValue = node.OuterXml;
                            break;
                    }
                }
                catch
                {
                    nodeValue = defaultValue;
                }

            return nodeValue;
        }

        /// <summary>
        /// 获取指定的多个节点的指定值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xPath">xPath查询</param>
        /// <param name="xmlValueType">值类型</param>
        /// <returns></returns>
        public static List<string> GetXmlNodesValue(XmlNode node, string xPath, XmlValueType xmlValueType)
        {
            return GetXmlNodesValue(node, xPath, null, xmlValueType);
        }

        /// <summary>
        /// 获取指定的多个节点的指定值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xPath">xPath查询</param>
        /// <param name="xmlValueType">值类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static List<string> GetXmlNodesValue(XmlNode node, string xPath, XmlValueType xmlValueType, string defaultValue)
        {
            return GetXmlNodesValue(node, xPath, null, xmlValueType, defaultValue);
        }

        /// <summary>
        /// 获取指定的多个节点的指定值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xPath">xPath查询</param>
        /// <param name="xmlNamespace"></param>
        /// <param name="xmlValueType">值类型</param>
        /// <returns></returns>
        public static List<string> GetXmlNodesValue(XmlNode node, string xPath, XmlNamespaceManager xmlNamespace, XmlValueType xmlValueType)
        {
            var result = new List<string>();
            if (String.IsNullOrWhiteSpace(xPath))
                return result;
            var subNodeList = node.SelectNodes(xPath, xmlNamespace)?.Cast<XmlNode>().ToList();
            if (subNodeList == null || !subNodeList.Any())
                return result;
            result.AddRange(subNodeList.Select(subNode => GetXmlNodeValue(subNode, xmlValueType, null)));
            return result;
        }

        /// <summary>
        /// 获取指定的多个节点的指定值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xPath">xPath查询</param>
        /// <param name="xmlNamespace"></param>
        /// <param name="xmlValueType">值类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static List<string> GetXmlNodesValue(XmlNode node, string xPath, XmlNamespaceManager xmlNamespace,
            XmlValueType xmlValueType, string defaultValue)
        {
            var result = new List<string>();
            if (String.IsNullOrWhiteSpace(xPath))
                return result;
            var subNodeList = node.SelectNodes(xPath, xmlNamespace)?.Cast<XmlNode>().ToList();
            if (subNodeList == null || !subNodeList.Any())
                return result;
            result.AddRange(subNodeList.Select(subNode => GetXmlNodeValue(subNode, xmlValueType, defaultValue)));
            return result;
        }

        /// <summary>
        /// 获取指定节点的指定值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="xmlValueType">值类型</param>
        /// <returns>值</returns>
        public static string GetXmlNodeValue(XmlNode node, XmlValueType xmlValueType)
        {
            return GetXmlNodeValue(node, xmlValueType, null);
        }

        /// <summary>
        /// 获取指定节点的指定值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <returns>值</returns>
        public static string GetXmlNodeValue(XmlNode node)
        {
            return GetXmlNodeValue(node, XmlValueType.InnerText, null);
        }

        /// <summary>
        /// 获取指定节点的指定值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public static string GetXmlNodeValue(XmlNode node, string defaultValue)
        {
            return GetXmlNodeValue(node, XmlValueType.InnerText, defaultValue);
        }

        /// <summary>
        /// 设置指定节点的属性值
        /// </summary>
        /// <param name="node">Xml节点</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="value">值</param>
        public static void SetXmlAttributeValue(XmlNode node, string attributeName, string value)
        {
            SetXmlAttributeValue(node, attributeName, value, false);
        }

        /// <summary>
        /// 设置指定节点的属性值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        /// <param name="isEnabled"></param>
        public static void SetXmlAttributeValue(XmlNode node, string attributeName, string value, bool isEnabled)
        {
            XmlAttribute nodeAttribute = null;
            foreach (XmlAttribute attribute2 in node.Attributes)
            {
                if (attribute2.Name == attributeName)
                {
                    nodeAttribute = attribute2;
                    break;
                }
            }
            if (nodeAttribute == null)
            {
                nodeAttribute = node.OwnerDocument.CreateAttribute(attributeName);
                nodeAttribute.Value = isEnabled ? value : XmlEncoding(value);
                node.Attributes.Append(nodeAttribute);
            }
            else
            {
                nodeAttribute.Value = isEnabled ? value : XmlEncoding(value);
            }
        }

        /// <summary>
        /// 将字符串转换为符合Xml标准的字符串编码
        /// </summary>
        /// <param name="Source">源字符串</param>
        /// <returns>Xml编码的字符串</returns>
        public static string XmlEncoding(string Source)
        {
            string m_xml = "";
            if (Source != null && Source != "")
            {
                m_xml = Source;
                m_xml = m_xml.Replace("&", "&amp;");
                m_xml = m_xml.Replace("\"", "&quot;");
                m_xml = m_xml.Replace("<", "&lt;");
                m_xml = m_xml.Replace(">", "&gt;");
            }
            return m_xml;
        }
    }
}
