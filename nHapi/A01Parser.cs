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

namespace NHapiSampleApplication.nHapi
{
    public class A01Parser
    {
        public ADT_A01 Parse(string message)
        {
            IMessage hl7Message = null;
            ADT_A01 adtMessage = null;
            try
            {
                var parser = new PipeParser();

                message = message.Replace("\\r\\n", "\r\n");
                hl7Message = parser.Parse(message, "2.5");

                adtMessage = (ADT_A01)hl7Message;
            }
            catch (HL7Exception ex)
            {
                // Handle error, probably by sending a NACK message
                MessageBox.Show(ex.Message);
            }

            return adtMessage;
        }

        public Patient Convert(ADT_A01 message)
        {
            var patient = new Patient();
            patient.FirstName = message.PID.GetPatientName().First().GivenName.Value;
            patient.MiddleName = message.PID.GetPatientName().First().SecondAndFurtherGivenNamesOrInitialsThereof.Value;
            patient.LastName = message.PID.GetPatientName().First().FamilyName.Surname.Value;

            return patient;
        }
    }
}
