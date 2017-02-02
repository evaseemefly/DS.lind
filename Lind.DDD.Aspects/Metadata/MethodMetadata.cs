﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lind.DDD.Aspects.Metadata
{
    public class MethodMetadata
    {
        private string _methodName;

        public MethodMetadata(string methodName)
        {
            _methodName = methodName;
        }

        public virtual string MethodName
        {
            get;
            set;
        }
    }
}
