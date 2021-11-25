using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTradeReporter.Core.Models
{
    public class FieldSchema
    {
        private FieldSchema() { }

        public FieldSchema(string fieldName, Type fieldType)
        {
            FieldName = fieldName;
            FieldType = fieldType;
        }

        public FieldSchema(int fieldPosition, string fieldName, Type fieldType)
        {
            FieldPosition = fieldPosition;
            FieldName = fieldName;
            FieldType = fieldType;
        }

        

        public int FieldPosition { get; private set; }

        public string FieldName { get; private set; }

        public Type FieldType { get; private set; }

        public int RequiredLength { get; set; }

        public bool AllowAlphaOnly { get; set; }
    }
}
