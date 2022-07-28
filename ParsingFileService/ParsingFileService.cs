using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ParsingFileService
{
    public partial class ParsingFileService : ServiceBase
    {

        string path = @"D:\xml\LOIBOM_18622669.xml";
        public ParsingFileService()
        {
            InitializeComponent();
        }

        //protected override void OnStart(string[] args)
        //{
        //}

        private static readonly ILog log = LogManager.GetLogger(typeof(ParsingFileService));
        //private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // funzione per parsificare file xml utilizzando un xslt
        public void ParsingByXslt()
        {
            string path = @"D:\idoc\IDOC_202207\IDOC_14480344_20220705193956173.xml";
            string destPath = @"D:\idoc\IDOC_PARS_XSLT\JISORD_IDOC_TRANSFORM.html";
            //istanzio un nuovo oggetto xsltransform
            XslTransform xslt = new XslTransform();

            //carico il file stilistico xslt
            xslt.Load(@"D:\idoc\IDOC_202207\XSLT\MagnetiMarelli-Order-Job67.xslt");

            //carico il file xml da parsificare 
            //XPathDocument mydata = new XPathDocument(@"D:\idoc\IDOC_202207\MATMAS_IDOC_190115807_20220517082638316.xml");

            //creo un output tramite log4net
            //log.Debug(result);
            
            //creo un output tramite xmltextwriter
            //XmlWriter writer = XmlWriter.Create(destPath);

            //trasformo il file e mando l'output alla console
            //xslt.Transform(mydata, null, writer, null);

            //utilizzo il metodo transform ma passandogli due semplici url
            xslt.Transform(path,destPath);

        }

        
            
            
        // funzione ricorsiva per parsificare file xml estrapolando i soli valori
        public void ParsingDoc(XElement e)
        {
            foreach (XElement element in e.Elements())
            {
                if (element.Elements().Count() == 0)
                {
                    log.Debug(element.Value);
                }
                else
                {
                    ParsingDoc(element);
                }
            }
        }
        
        public void Start()
        {
            log.Debug(path);

            XDocument doc = XDocument.Load(path);

            foreach(XElement e in doc.Elements())
            {
                this.ParsingDoc(e);
            }

            this.ParsingByXslt();
        }

        protected override void OnStop()
        {
        }
    }
}
