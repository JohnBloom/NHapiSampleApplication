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
using NHapiSampleApplication.Models;
using NHapi.Model.V23.Message;

namespace NHapiSampleApplication.nHapi
{
    public class A01Parser : IParser<ADT_A08>
    {
        public ADT_A08 Parse(string message)
        {
            IMessage hl7Message = null;
            ADT_A08 adtMessage = null;
            try
            {
                var parser = new PipeParser();

                hl7Message = parser.Parse(message, "2.3");

                adtMessage = (ADT_A08)hl7Message;
            }
            catch (HL7Exception ex)
            {
                // Handle error, probably by sending a NACK message
                MessageBox.Show(ex.Message);
            }

            return adtMessage;
        }

        public Patient Convert(ADT_A08 message)
        {
            var patient = new Patient();
            patient.FirstName = message.PID.PatientName.GivenName.Value;
            patient.MiddleName = message.PID.PatientName.MiddleInitialOrName.Value;
            patient.LastName = message.PID.PatientName.FamilyName.Value;

            return patient;
        }
    }
}
