using QLTradeReporter.Core.Interfaces;
using QLTradeReporter.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace QLTradeReporter.Core.Processor
{
    public class TradeValidator : ITradeValidator
    {
        FieldSchemaDictionary fieldMap;
        public TradeValidator(IDelimitedFileDefinition fileDefinition)
        {
            if (fileDefinition == null) throw new ArgumentNullException("fileDefinition");

            fieldMap = fileDefinition.FieldSchemaDictionary;
        }

        public bool IsValid(string[] fields)
        {
            bool isValid = false;

            isValid = fields != null && fields.Count() == fieldMap.Count;

            if (isValid)
            {
                for (int i = 0; i < fields.Count(); i++)
                {
                    string value = fields[i];

                    FieldSchema fieldSchema;
                    if (fieldMap.TryGetValue(i, out fieldSchema))
                    {
                        var converter = TypeDescriptor.GetConverter(fieldSchema.FieldType);
                        isValid = converter.IsValid(value);

                        if (isValid && fieldSchema.FieldType == typeof(string) && fieldSchema.RequiredLength > 0)
                        {
                            isValid = value.Length == fieldSchema.RequiredLength;

                            if (isValid && fieldSchema.AllowAlphaOnly)
                             isValid = value.All(Char.IsLetter);
                        }
                        if (!isValid) break;
                    }
                }
            }

            return isValid;
        }
    }
}
