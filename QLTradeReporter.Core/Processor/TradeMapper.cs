using QLTradeReporter.Core.Interfaces;
using QLTradeReporter.Core.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace QLTradeReporter.Core.Processor
{
    /// <summary>
    /// Trade Mapper - Responsible for mapping an array of fields to a model object given a file definition
    /// </summary>
    public class TradeMapper : ITradeMapper
    {
        FieldSchemaDictionary fieldMap;

        public TradeMapper(IDelimitedFileDefinition fileDefinition)
        {
            fieldMap = fileDefinition.FieldSchemaDictionary;
        }

        /// <summary>
        /// Maps an array of 
        /// </summary>
        /// <typeparam name="T">generic input type to be mapped to</typeparam>
        /// <param name="fields">string array of fields</param>
        /// <returns></returns>
        public T Map<T>(string[] fields) where T : new()
        {
            var fieldNames = fieldMap.Values.Select(schema => schema.FieldName);
            T objectInstance = new T();

            for (int i = 0; i < fields.Count(); i++)
            {
                FieldSchema fieldSchema;
                if (fieldMap.TryGetValue(i, out fieldSchema))
                {
                    var property = typeof(T).GetProperty(fieldSchema.FieldName);
                    var converter = TypeDescriptor.GetConverter(property.PropertyType);
                    var convertedValue = converter.ConvertFrom(fields[i]);
                    property.SetValue(objectInstance, convertedValue);
                }
            }
            return objectInstance;
        }
    }
}