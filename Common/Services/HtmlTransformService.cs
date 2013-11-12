namespace Common.Services
{
    using Windows.Data.Xml.Dom;
    using Windows.Data.Xml.Xsl;
    using Common.Interfaces;

    public class HtmlTransformService : IHtmlTransformService
    {
        private XsltProcessor _transformer;

        public XsltProcessor Transformer
        {
            get
            {
                if (_transformer == null)
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(
                        @"<?xml version='1.0'?><xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"" exclude-result-prefixes=""xsl""><xsl:output method=""html"" indent=""yes"" version=""4.0"" omit-xml-declaration=""yes"" encoding=""UTF-8"" /><xsl:template match=""/""><xsl:value-of select=""."" /></xsl:template></xsl:stylesheet>");
                    _transformer = new XsltProcessor(xmlDocument);
                }

                return _transformer;
            }
        }

        public string ToText(string html)
        {
            var htmlDocument = new XmlDocument();
            htmlDocument.LoadXml(string.Format("<div>{0}</div>", html));
            return Transformer.TransformToString(htmlDocument);
        }
    }
}
