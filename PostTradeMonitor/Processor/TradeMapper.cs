using PostTradeMonitor.Core.Interfaces;
using PostTradeMonitor.Core.Models;
using PostTradeMonitor.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PostTradeMonitor.Core.Processor
{
    /// <summary>
    /// Trade Mapper - Responsible for mapping an array of fields to a model object given a file definition
    /// </summary>
    public class TradeMapper : ITradeMapper
    {

        FieldSchemaDictionary fieldMap;
        private readonly Action<Trade, int> setPrice;
        private readonly Action<Trade, string> setSymbol;
        private readonly Action<Trade, int> setQty;
        StringBuilder str = new StringBuilder();
        Dictionary<string, Action<Trade, string>> funcCache = new Dictionary<string, Action<Trade, string>>();


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
        public T Map<T>(string message) where T : Trade
        {
            //T objectInstance = new T();
            //T objectInstance = new T();
            var parsedString = message.AsSpan().Split(',');

            int i = 0;
            //var action = SetProperty(objectInstance, "TimeStamp", "234567");
            //Action<Trade, int> intSetter = SetTradeProperty<int>(nameof(Trade.Price));

            Trade trade = new Trade();
            TradeInfo tradeInfo = new TradeInfo();

            while (parsedString.MoveNext())
            {
                var currentvalue = parsedString.Current;
                FieldSchema fieldSchema;

                if (fieldMap.TryGetValue(i, out fieldSchema))
                {
                    Action<Trade, string> action;
                    if (funcCache.TryGetValue(fieldSchema.FieldName, out action))
                        action(trade, currentvalue.ToString());
                    else
                    {
                        var setProperty = SetTradePropertyValue(fieldSchema.FieldName);
                        setProperty(trade, currentvalue.ToString());

                        funcCache.Add(fieldSchema.FieldName, setProperty);
                    }
                }
                i++;
            }

            return (T)trade;
        }
  

        private static Action<Trade, string> SetTradePropertyValue(string propertyName)
        {
            // reflective 
            PropertyInfo property = typeof(Trade).GetProperty(propertyName);
            
            ParameterExpression targetObjectExp = Expression.Parameter(typeof(Trade), "target");
            ParameterExpression valueExpParam = Expression.Parameter(typeof(string), "value");

            MemberExpression propertyExp = Expression.Property(targetObjectExp, property);

            //var converter = TypeDescriptor.GetConverter(property.PropertyType);
            //var propertyValue = converter.ConvertFromInvariantString(value);
            //var argumentExpression = Expression.Constant(propertyValue, property.PropertyType);
            //Expression valueExpression = Expression.Convert(argumentExpression, property.PropertyType);

            Expression convertedValueExp = Expression.Convert(valueExpParam, property.PropertyType, ConvertToType(propertyExp, Type.GetTypeCode(property.PropertyType)));

            var @delegate = Expression.Lambda(Expression.Call(Expression.Convert(targetObjectExp, typeof(Trade)), property.SetMethod, convertedValueExp), targetObjectExp, valueExpParam).Compile();

            return (Action<Trade, string>)@delegate;
        }

        public static MethodInfo ConvertToType(MemberExpression propertyExpression, TypeCode typeCode)
        {
            string conversionMethod = nameof(Convert.ToString);
            switch (typeCode)
            {
                case TypeCode.Int16:
                    conversionMethod = nameof(Convert.ToInt16);
                    break;
                case TypeCode.Int32:
                    conversionMethod = nameof(Convert.ToInt32);
                    break;
                case TypeCode.Int64:
                    conversionMethod = nameof(Convert.ToInt64);
                    break;
                case TypeCode.Single:
                    conversionMethod = nameof(Convert.ToSingle);
                    break;
                case TypeCode.Byte:
                    conversionMethod = nameof(Convert.ToByte);
                    break;
                default:
                    break;
            }

            //var changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(TypeCode) });
            var changeTypeMethod = typeof(Convert).GetMethod(conversionMethod, new Type[] { typeof(string) });

            return changeTypeMethod;

        }

        //public static MethodCallExpression ConvertToType1(MemberExpression propertyExpression, TypeCode typeCode)
        //{
        //    string conversionMethod = nameof(Convert.ToString);
        //    switch (typeCode)
        //    {
        //        case TypeCode.Int16:
        //            conversionMethod = nameof(Convert.ToInt16);
        //            break;
        //        case TypeCode.Int32:
        //            conversionMethod = nameof(Convert.ToInt32);
        //            break;
        //        case TypeCode.Int64:
        //            conversionMethod = nameof(Convert.ToInt64);
        //            break;
        //        case TypeCode.Single:
        //            conversionMethod = nameof(Convert.ToSingle);
        //            break;
        //        case TypeCode.Byte:
        //            conversionMethod = nameof(Convert.ToByte);
        //            break;
        //        default:
        //            break;
        //    }

        //    //var changeTypeMethod = typeof(Convert).GetMethod("ChangeType", new Type[] { typeof(object), typeof(TypeCode) });
        //    var changeTypeMethod = typeof(Convert).GetMethod(conversionMethod, new Type[] { typeof(string) });
            
        //    var callExpressionReturningObject = Expression.Call(changeTypeMethod, propertyExpression, Expression.Constant(typeCode));
        //    return callExpressionReturningObject;
        //}
        // build expression and store it

        //private static Action<T, string, string> SetProperty<T>(T trade, string propertyName, string value)
        //{
        //    var propertyNameParameterExpr = Expression.Parameter(typeof(string), "propertyName");
        //    var valueParameterExpr = Expression.Parameter(typeof(string), "value");

        //    // what happens if property name does not exist?
        //    // can be passed in
        //    PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);//.GetMethod("set_TimeStamp", BindingFlags.Public);
        //    if (propertyInfo == null)
        //        throw new NotImplementedException($"Property {propertyName} Not Implemented On {trade} object");

        //    MethodInfo methodInfo = propertyInfo.GetSetMethod();
        //    Type propertyType = propertyInfo.PropertyType;
        //    var typeInstanceExpression = Expression.Parameter(typeof(T));

        //    var converter = TypeDescriptor.GetConverter(propertyType);
        //    var propertyValue = converter.ConvertFromInvariantString(value);
        //    var argumentExpression = Expression.Constant(propertyValue, propertyType);
        //    UnaryExpression conversionExpression = Expression.Convert(argumentExpression, propertyType);

        //    var setPropertyValueExpression = Expression.Call(typeInstanceExpression, methodInfo, new Expression[] { argumentExpression });

        //    Expression<Action<T, string, string>> actionExecuteExpression = Expression.Lambda<Action<T, string, string>>(setPropertyValueExpression, typeInstanceExpression, propertyNameParameterExpr, valueParameterExpr);

        //    Action<T, string, string> setProperty = actionExecuteExpression.Compile();

        //    return setProperty;
        //    //setProperty(trade);
        //}

        //private static Action<Trade, V> SetTradeProperty<V>(string propertyName)
        //{
        //    PropertyInfo property = typeof(Trade).GetProperty(propertyName);
        //    MethodInfo propertySetMethod = typeof(Trade).GetMethod($"set_{propertyName}", BindingFlags.Public);

        //    ParameterExpression targetObjectExp = Expression.Parameter(typeof(Trade), "target");
        //    ParameterExpression valueExp = Expression.Parameter(property.PropertyType, "value");

        //    // Expression.Property can be used here as well
        //    MemberExpression propertyExp = Expression.Property(targetObjectExp, property);

        //    Expression valueExpression = Expression.Convert(valueExp, property.PropertyType);


        //    var @delegate = Expression.Lambda(Expression.Call(Expression.Convert(targetObjectExp, typeof(Trade)), property.SetMethod, valueExpression), targetObjectExp, valueExp).Compile();

        //    return (Action<Trade, V>)@delegate;
        //}

    }
}