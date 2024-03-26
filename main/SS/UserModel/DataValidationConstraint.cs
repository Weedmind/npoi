/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */
namespace NPOI.SS.UserModel
{
    using System;


    public interface IDataValidationConstraint
    {
        /// <summary>
        /// return data validation type of this constraint
        /// </summary>
        /// <returns></returns>
        int GetValidationType();


        /// <summary>
        /// get or set then comparison operator for this constraint
        /// </summary>
        int Operator { get; set; }
        
        /// <summary>
        /// If validation type is {@link ValidationType#LIST} 
        /// and <code>formula1</code> was comma-separated literal values rather than a range or named range,
        /// returns list of literal values.
        /// Otherwise returns <code>null</code>.
        /// </summary>
        String[] ExplicitListValues { get; set; }

        /// <summary>
        /// get or set the formula for expression 1. May be <code>null</code>
        /// </summary>
        string Formula1 { get; set; }


        /// <summary>
        /// get or set the formula for expression 2. May be <code>null</code>
        /// </summary>
        string Formula2 { get; set; }


        
    }
    /// <summary>
/// ValidationType enum
/// </summary>
    public static class ValidationType
    {
        /// <summary>
/// 'Any value' type - value not restricted */
/// </summary>
        public const int ANY = 0x00;
        /// <summary>
/// int ('Whole number') type */
/// </summary>
        public const int INTEGER = 0x01;
        /// <summary>
/// Decimal type */
/// </summary>
        public const int DECIMAL = 0x02;
        /// <summary>
/// List type ( combo box type ) */
/// </summary>
        public const int LIST = 0x03;
        /// <summary>
/// Date type */
/// </summary>
        public const int DATE = 0x04;
        /// <summary>
/// Time type */
/// </summary>
        public const int TIME = 0x05;
        /// <summary>
/// String length type */
/// </summary>
        public const int TEXT_LENGTH = 0x06;
        /// <summary>
/// Formula ( 'Custom' ) type */
/// </summary>
        public const int FORMULA = 0x07;
    }
    /// <summary>
/// Condition operator enum
/// </summary>
    public static class OperatorType
    {
       
        public const int BETWEEN = 0x00;
        public const int NOT_BETWEEN = 0x01;
        public const int EQUAL = 0x02;
        public const int NOT_EQUAL = 0x03;
        public const int GREATER_THAN = 0x04;
        public const int LESS_THAN = 0x05;
        public const int GREATER_OR_EQUAL = 0x06;
        public const int LESS_OR_EQUAL = 0x07;
        /// <summary>
/// default value to supply when the operator type is not used */
/// </summary>
        public const int IGNORED = BETWEEN;

        /* package */
        public static void ValidateSecondArg(int comparisonOperator, String paramValue)
        {
            switch (comparisonOperator)
            {
                case BETWEEN:
                    if (paramValue == null)
                    {
                        throw new ArgumentException("expr2 must be supplied for 'between' comparisons");
                    }
                    break;
                case NOT_BETWEEN:
                    if (paramValue == null)
                    {
                        throw new ArgumentException("expr2 must be supplied for 'between' comparisons");
                    }
                    break;
                // all other operators don't need second arg
            }
        }
    }
}