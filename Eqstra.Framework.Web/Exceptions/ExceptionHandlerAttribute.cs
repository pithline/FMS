﻿using System;
using System.ComponentModel.Composition;

namespace Eqstra.Framework.Web.Exceptions
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExceptionHandlerAttribute : ExportAttribute
    {
        public ExceptionHandlerAttribute()
            : base(typeof(IExceptionHandler))
        {

        }

        public string Type { get; set; }
    }
}
