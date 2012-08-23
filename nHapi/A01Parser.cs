using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using NHapi.Base;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25.Message;

namespace NHapiSampleApplication.nHapi
{
    public class A01Parser
    {
        public ADT_A01 Message { get; set; }

        public void Parse(string message)
        {
            IMessage hl7Message = null;

            try
            {
                var parser = new PipeParser();

                message = message.Replace("\\r\\n", "\r\n");
                hl7Message = parser.Parse(message, "2.5");

                Message = (ADT_A01)hl7Message;
            }
            catch (HL7Exception ex)
            {
                // Handle error, probably by sending a NACK message
                MessageBox.Show(ex.Message);
            }
        }
    }
}
