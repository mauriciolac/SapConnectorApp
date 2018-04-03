using System;
using System.Collections.Generic;
using System.Text;
using SAP.Middleware.Connector;



namespace SapConnector
{
    public class SapConnection
    {

        private RfcConfigParameters oConfigParameters;
        private RfcDestination oRFC;

        public SapConnection()
        {
            oConfigParameters = new RfcConfigParameters();
            oConfigParameters.Add(RfcConfigParameters.Name, "D01");
            oConfigParameters.Add(RfcConfigParameters.Client, "200");
            oConfigParameters.Add(RfcConfigParameters.AppServerHost, "177.20.235.229");
            oConfigParameters.Add(RfcConfigParameters.SystemNumber, "00");
            oConfigParameters.Add(RfcConfigParameters.SystemID, "D01");
            oConfigParameters.Add(RfcConfigParameters.User, "its01");
            oConfigParameters.Add(RfcConfigParameters.Password, "ams@2015");
            
            oConfigParameters.Add(RfcConfigParameters.Language, "PT");
            oConfigParameters.Add(RfcConfigParameters.PoolSize, "10");
        }

        public SapConnection(string name, string server, string systemNumber, string systemID, string user, string password, string client, string router = "", string language = "PT", string poolsize = "10")
        {

            oConfigParameters = new RfcConfigParameters();
            oConfigParameters.Add(RfcConfigParameters.Name, name);
            oConfigParameters.Add(RfcConfigParameters.AppServerHost, server);
            oConfigParameters.Add(RfcConfigParameters.SystemNumber, systemNumber);
            oConfigParameters.Add(RfcConfigParameters.SystemID,systemID);
            oConfigParameters.Add(RfcConfigParameters.User, user);
            oConfigParameters.Add(RfcConfigParameters.Password, password);
            oConfigParameters.Add(RfcConfigParameters.Client, client);
            oConfigParameters.Add(RfcConfigParameters.Language, language);
            oConfigParameters.Add(RfcConfigParameters.PoolSize, poolsize);

        }

        private bool Connect()
        {
            bool result = false;
            try
            {
                oRFC = RfcDestinationManager.GetDestination(oConfigParameters);
                if (oRFC != null)
                {
                    oRFC.Ping();
                    result = true;
                } else
                {
                    throw new Exception("Error trying to connect. RFCDestination Null");
                }
            } catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;            
        }     
        

        public string test()
        {
            try
            {
                if (Connect())
                {
                    RfcRepository repo = oRFC.Repository;

                    IRfcFunction testfn = repo.CreateFunction("BAPI_COMPANYCODE_GETLIST");

                    testfn.Invoke(oRFC);

                    var companyCodeList = testfn.GetTable("COMPANYCODE_LIST");
                    
                    /*
                    // turn it into a DataTable..
                    var companyDataTable = companyCodeList.ToDataTable("companycodelist");

                    //consultaTabela
                    IRfcFunction funcaoRT = repo.CreateFunction("RFC_READ_TABLE");
                    // Define parametros da função
                    funcaoRT.SetValue("query_table", "T001");
                    funcaoRT.SetValue("delimiter", "|");
                    // Chama função
                    funcaoRT.Invoke(oRFC);

                    // Recupera Dados cru, você precisa trata-los para
                    // que sejam humanamente legivel
                    IRfcTable tabela = funcaoRT.GetTable("DATA");
                    var retornoConsulta = tabela.ToDataTable("T001");
                    */
                    return "ok";
                }

                throw new Exception("nao rolou");
            } catch (Exception ex)
            {
                return ex.Message;
            }

           

        }

    }
}
