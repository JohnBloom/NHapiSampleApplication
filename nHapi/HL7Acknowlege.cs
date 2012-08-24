using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Base.Util;
using NHapi.Model.V25.Message;
using NHapiSampleApplication.Models;
using System.Configuration;

namespace NHapiSampleApplication.nHapi
{
    public class HL7Acknowlege : IAcknowlege
    {
        #region Private properties
        private static string communicationName;
        private static string environmentIdentifier;

        private static string CommunicationName
        {
            get
            {
                if (communicationName == null)
                    communicationName = ConfigurationSettings.AppSettings["ApplicationCommunicationName"];

                return communicationName;
            }
        }

        private static string EnvironmentIdentifier
        {
            get
            {
                if (environmentIdentifier == null)
                    environmentIdentifier = ConfigurationSettings.AppSettings["EnvironmentIdentifier"];

                return environmentIdentifier;
            }
        }
        #endregion

        public string GetAcknowlegement(IMessage message)
        {
            var ackMessage = (ACK)MakeACK(message, "AA");

            var parser = new PipeParser();
            return parser.Encode(ackMessage);
        }

        public static IMessage MakeACK(IMessage inboundMessage, string ackCode)
        {
            Terser t = new Terser(inboundMessage);
            ISegment inboundHeader = null;
            try
            {
                inboundHeader = t.getSegment("MSH");
            }
            catch (NHapi.Base.HL7Exception)
            {
                throw new NHapi.Base.HL7Exception("Need an MSH segment to create a response ACK");
            }
            return MakeACK(inboundHeader, ackCode);
        }

        public static IMessage MakeACK(ISegment inboundHeader, string ackCode)
        {
            if (!inboundHeader.GetStructureName().Equals("MSH"))
                throw new NHapi.Base.HL7Exception(
                    "Need an MSH segment to create a response ACK (got " + inboundHeader.GetStructureName() + ")");

            // Find the HL7 version of the inbound message:
            //
            string version = null;
            try
            {
                version = Terser.Get(inboundHeader, 12, 0, 1, 1);
            }
            catch (NHapi.Base.HL7Exception)
            {
                // I'm not happy to proceed if we can't identify the inbound
                // message version.
                throw new NHapi.Base.HL7Exception("Failed to get valid HL7 version from inbound MSH-12-1");
            }

            IMessage ackMessage = new ACK();
            // Create a Terser instance for the outbound message (the ACK).
            Terser terser = new Terser(ackMessage);

            // Populate outbound MSH fields using data from inbound message
            ISegment outHeader = (ISegment)terser.getSegment("MSH");
            DeepCopy.copy(inboundHeader, outHeader);

            // Now set the message type, HL7 version number, acknowledgement code
            // and message control ID fields:
            string sendingApp = terser.Get("/MSH-3");
            string sendingEnv = terser.Get("/MSH-4");
            terser.Set("/MSH-3", CommunicationName);
            terser.Set("/MSH-4", EnvironmentIdentifier);
            terser.Set("/MSH-5", sendingApp);
            terser.Set("/MSH-6", sendingEnv);
            terser.Set("/MSH-7", DateTime.Now.ToString("yyyyMMddmmhh"));
            terser.Set("/MSH-9", "ACK");
            terser.Set("/MSH-12", version);
            terser.Set("/MSA-1", ackCode == null ? "AA" : ackCode);
            terser.Set("/MSA-2", Terser.Get(inboundHeader, 10, 0, 1, 1));

            return ackMessage;
        }
    }
}
