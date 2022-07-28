using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SAP.Middleware.Connector;

namespace ParsingFileService
{
    public class ECCDestinationConfig : IDestinationConfiguration
    {
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public bool ChangeEventsSupported()
        {
            return true;
        }

        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();

                if (destinationName.Equals("mySAPdestination"))
            {
                parms.Add(RfcConfigParameters.AppServerHost, "IPAddress");
                parms.Add(RfcConfigParameters.SystemNumber, "00");
                parms.Add(RfcConfigParameters.SystemID, "ID");
                parms.Add(RfcConfigParameters.User, "Username");
                parms.Add(RfcConfigParameters.Password, "Password");
                parms.Add(RfcConfigParameters.RepositoryPassword, "Password");
                parms.Add(RfcConfigParameters.Client, "100");
                parms.Add(RfcConfigParameters.Language, "EN");
                parms.Add(RfcConfigParameters.PoolSize, "5");
            }
            return parms;
        }

        internal static void CallingSAP()
        {
            ECCDestinationConfig cfg = null;
            RfcDestination dest = null;

            try
            {
                cfg = new ECCDestinationConfig();
                RfcDestinationManager.RegisterDestinationConfiguration(cfg);

                dest = RfcDestinationManager.GetDestination("mySAPdestination");

                RfcRepository repo = dest.Repository;

                IRfcFunction fnpush = repo.CreateFunction("ZFUNCTION");
                // Send Data with RFC Structure
                IRfcStructure data = fnpush.GetStructure("IM_STRUCTURE");
                data.SetValue("ITEM1", "VALUE1");
                data.SetValue("ITEM2", "VALUE2");
                data.SetValue("ITEM3", "VALUE3");
                data.SetValue("ITEM4", "VALUE4");

                fnpush.SetValue("IM_STRUCTURE", data);

                // Send data with RFC table
                IRfcTable dataTbl = fnpush.GetTable("IM_TABLE");

                //foreach(var item in ListItem)
                {
                    dataTbl.Append();
                    dataTbl.SetValue("ITEM1", "VALUE1");
                    dataTbl.SetValue("ITEM2", "VALUE2");
                    dataTbl.SetValue("ITEM3", "VALUE3");
                    dataTbl.SetValue("ITEM4", "VALUE4");
                }

                fnpush.Invoke(dest);

                var exObject = fnpush.GetObject("EX_OBJECT");
                IRfcStructure exStructure = fnpush.GetStructure("EX_STRUCTURE");

                RfcSessionManager.EndContext(dest);

                RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
            }
            catch (Exception ex)
            {
                RfcSessionManager.EndContext(dest);

                RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
                Thread.Sleep(1000);
            }
        }
    }
}
