﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHapi.Base.Model;

namespace NHapiSampleApplication.Models
{
    public interface IAcknowlege
    {
        string GetAcknowlegement(IMessage message);
    }
}
