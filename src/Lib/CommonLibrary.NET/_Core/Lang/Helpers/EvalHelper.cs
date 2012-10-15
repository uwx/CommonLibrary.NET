using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

using ComLib.Lang.Types;
using ComLib.Lang.Core;


namespace ComLib.Lang.Helpers
{
    /// <summary>
    /// Helper class for datatypes.
    /// </summary>
    public class EvalHelper
    {
        /// <summary>
        /// Evalulates a math expression of 2 numbers.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The number on the left hand side</param>
        /// <param name="rhs">The number on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LNumber CalcNumbers(AstNode node, LNumber lhs, LNumber rhs, Operator op)
        {
            var left = lhs.Value;
            var right = rhs.Value;
            var result = 0.0;
            if (op == Operator.Multiply)
            {
                result = left * right;
            }
            else if (op == Operator.Divide)
            {
                result = left / right;
            }
            else if (op == Operator.Add)
            {
                result = left + right;
            }
            else if (op == Operator.Subtract)
            {
                result = left - right;
            }
            else if (op == Operator.Modulus)
            {
                result = left % right;
            }
            return new LNumber(result);
        }


        /// <summary>
        /// Evaluates a math expression of 2 units.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="left">The unit on the left</param>
        /// <param name="right">The unit on the right</param>
        /// <param name="op">The math operator</param>
        /// <param name="units"></param>
        /// <returns></returns>
        public static LUnit CalcUnits(AstNode node, LUnit left, LUnit right, Operator op, Units units)
        {
            double baseUnitsValue = 0;
            if (op == Operator.Multiply)
            {
                baseUnitsValue = left.BaseValue * right.BaseValue;
            }
            else if (op == Operator.Divide)
            {
                baseUnitsValue = left.BaseValue / right.BaseValue;
            }
            else if (op == Operator.Add)
            {
                baseUnitsValue = left.BaseValue + right.BaseValue;
            }
            else if (op == Operator.Subtract)
            {
                baseUnitsValue = left.BaseValue - right.BaseValue;
            }
            else if (op == Operator.Modulus)
            {
                baseUnitsValue = left.BaseValue % right.BaseValue;
            }

            var relativeValue = units.ConvertToRelativeValue(baseUnitsValue, left.SubGroup, null);
            var result = new LUnit();
            result.BaseValue = baseUnitsValue;
            result.Group = left.Group;
            result.SubGroup = left.SubGroup;
            result.Value = relativeValue;

            return result;
        }


        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LTime CalcTimes(AstNode node, LTime lhs, LTime rhs, Operator op)
        {
            if (op != Operator.Add && op != Operator.Subtract)
                throw BuildRunTimeException(node, "Can only add/subtract times");

            var left = lhs.Value;
            var right = rhs.Value;
            var result = TimeSpan.MinValue;
            if (op == Operator.Add)
            {
                result = left + right;
            }
            else if (op == Operator.Subtract)
            {
                result = left - right;
            }
            return new LTime(result);
        }


        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LTime CalcDates(AstNode node, LDate lhs, LDate rhs, Operator op)
        {
            if (op != Operator.Subtract)
                throw BuildRunTimeException(node, "Can only subtract dates");

            var left = lhs.Value;
            var right = rhs.Value;
            var result = left - right;
            return new LTime(result);
        }


        /// <summary>
        /// Increments the number supplied.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="op"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static LNumber CalcUnary(LNumber num, Operator op, double increment)
        {
            var val = num.Value;
            if (op == Operator.PlusPlus)
            {
                val++;
            }
            else if (op == Operator.MinusMinus)
            {
                val--;
            }
            else if (op == Operator.PlusEqual)
            {
                val = val + increment;
            }
            else if (op == Operator.MinusEqual)
            {
                val = val - increment;
            }
            else if (op == Operator.MultEqual)
            {
                val = val * increment;
            }
            else if (op == Operator.DivEqual)
            {
                val = val / increment;
            }
            return new LNumber(val);
        }


        /// <summary>
        /// Compares null values.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool CompareNull(object left, object right, Operator op)
        {
            // Both null
            if (left == LNull.Instance && right == LNull.Instance && op == Operator.EqualEqual) return true;
            if (left == LNull.Instance && right == LNull.Instance && op == Operator.NotEqual) return false;
            // Both not null
            if (left != LNull.Instance && right != LNull.Instance && op == Operator.EqualEqual) return left == right;
            if (left != LNull.Instance && right != LNull.Instance && op == Operator.NotEqual) return left != right;
            // Check for one
            if (op == Operator.NotEqual) return true;

            return false;
        }



        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LBool CompareNumbers(AstNode node, LNumber lhs, LNumber rhs, Operator op)
        {
            var left = lhs.Value;
            var right = rhs.Value;
            var result = false;
            if (op == Operator.LessThan)            result = left < right;
            else if (op == Operator.LessThanEqual)  result = left <= right;
            else if (op == Operator.MoreThan)       result = left > right;
            else if (op == Operator.MoreThanEqual)  result = left >= right;
            else if (op == Operator.EqualEqual)     result = left == right;
            else if (op == Operator.NotEqual)       result = left != right;
            return new LBool(result);
        }


        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LBool CompareBools(AstNode node, LBool lhs, LBool rhs, Operator op)
        {
            var left = Convert.ToInt32(lhs.Value);
            var right = Convert.ToInt32(rhs.Value);
            var result = false;
            if (op == Operator.LessThan)        result = left < right;
            else if (op == Operator.LessThanEqual)   result = left <= right;
            else if (op == Operator.MoreThan)        result = left > right;
            else if (op == Operator.MoreThanEqual)   result = left >= right;
            else if (op == Operator.EqualEqual)      result = left == right;
            else if (op == Operator.NotEqual)        result = left != right;
            return new LBool(result);
        }


        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LBool CompareStrings(AstNode node, LString lhs, LString rhs, Operator op)
        {
            var left = lhs.Value;
            var right = rhs.Value;
            var result = false;
            if (op == Operator.EqualEqual)
            {
                result = left == right;
                return new LBool(result);
            }
            else if (op == Operator.NotEqual)
            {
                result = left != right;
                return new LBool(result);
            }

            int compareResult = string.Compare(left, right, StringComparison.InvariantCultureIgnoreCase);
            if (op == Operator.LessThan) result = compareResult == -1;
            else if (op == Operator.LessThanEqual) result = compareResult != 1;
            else if (op == Operator.MoreThan) result = compareResult == 1;
            else if (op == Operator.MoreThanEqual) result = compareResult != -1;
            return new LBool(result);
        }


        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LBool CompareDates(AstNode node, LDate lhs, LDate rhs, Operator op)
        {
            var left = lhs.Value;
            var right = rhs.Value;
            var result = false;
            if (op == Operator.LessThan)             result = left < right;
            else if (op == Operator.LessThanEqual)   result = left <= right;
            else if (op == Operator.MoreThan)        result = left > right;
            else if (op == Operator.MoreThanEqual)   result = left >= right;
            else if (op == Operator.EqualEqual)      result = left == right;
            else if (op == Operator.NotEqual)        result = left != right;
            return new LBool(result);
        }


        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LBool CompareTimes(AstNode node, LTime lhs, LTime rhs, Operator op)
        {
            var left = lhs.Value;
            var right = rhs.Value;
            var result = false;
            if (op == Operator.LessThan)             result = left < right;
            else if (op == Operator.LessThanEqual)   result = left <= right;
            else if (op == Operator.MoreThan)        result = left > right;
            else if (op == Operator.MoreThanEqual)   result = left >= right;
            else if (op == Operator.EqualEqual)      result = left == right;
            else if (op == Operator.NotEqual)        result = left != right;
            return new LBool(result);
        }


        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LBool CompareUnits(AstNode node, LUnit lhs, LUnit rhs, Operator op)
        {
            var result = false;
            if (op == Operator.LessThan)             result = lhs.BaseValue < rhs.BaseValue;
            else if (op == Operator.LessThanEqual)   result = lhs.BaseValue <= rhs.BaseValue;
            else if (op == Operator.MoreThan)        result = lhs.BaseValue > rhs.BaseValue;
            else if (op == Operator.MoreThanEqual)   result = lhs.BaseValue >= rhs.BaseValue;
            else if (op == Operator.EqualEqual)      result = lhs.BaseValue == rhs.BaseValue;
            else if (op == Operator.NotEqual)        result = lhs.BaseValue != rhs.BaseValue;
            return new LBool(result);
        }


        
        /// <summary>
        /// Evaluates a math expression of 2 time spans.
        /// </summary>
        /// <param name="node">The AST node the evaluation is a part of.</param>
        /// <param name="lhs">The time on the left hand side</param>
        /// <param name="rhs">The time on the right hand side</param>
        /// <param name="op">The math operator.</param>
        /// <returns></returns>
        public static LBool CompareDays(AstNode node, LObject lhs, LObject rhs, Operator op)
        {
            var left = LangTypeHelper.ConverToLangDayOfWeekNumber(lhs);
            var right = LangTypeHelper.ConverToLangDayOfWeekNumber(rhs);
            var res = CompareNumbers(node, left, right, op);
            return res;
        }



        /// <summary>
        /// Evaluate the result of indexing an object e.g. users[0] or users["admins"]
        /// </summary>
        /// <param name="regmethods"></param>
        /// <param name="node"></param>
        /// <param name="target"></param>
        /// <param name="ndxObj"></param>
        /// <returns></returns>
        public static LObject AccessIndex(RegisteredMethods regmethods, AstNode node, LObject target, LObject ndxObj)
        {
            var result = LObjects.Empty;
            // Case 1: Array access users[0];
            if (target.Type == LTypes.Array)
            {
                var ndx = ((LNumber)ndxObj).Value;
                var methods = regmethods.Get(LTypes.Array);
                result = (LObject)methods.GetByNumericIndex(target, (int)ndx);
            }
            // Case 2: Map access. users["kishore"];
            else if (target.Type == LTypes.Map)
            {
                var memberName = ((LString)ndxObj).Value;
                var methods = regmethods.Get(LTypes.Map);
                if (!methods.HasProperty(target, memberName))
                    throw EvalHelper.BuildRunTimeException(node, "Property does not exist : '" + memberName + "'");

                result = (LObject)methods.GetByStringMember(target, memberName);
            }
            return result;
        }


        /*
        /// <summary>
        /// Gets an array value.
        /// </summary>
        /// <param name="regmethods"></param>
        /// <param name="node"></param>
        /// <param name="ListObject"></param>
        /// <param name="ndx"></param>
        /// <returns></returns>
        public static LObject AccessListIndex(RegisteredMethods regmethods, AstNode node, LArray ListObject, int ndx)
        {
            MethodInfo method = null;
            object result = null;
            var list = (LArray)ListObject;

            // Case 1: Fluentscript LArray
            if (list.Type == LTypes.Array)
            {
                var methods = regmethods.Get(LTypes.Array);
                result = methods.GetByNumericIndex(list, ndx);
                return (LObject)result;
            }
            // Case 2: C# IList
            else if (list.Value is IList)
            {
                method = ListObject.GetType().GetMethod("get_Item");
            }
            // Getting value?                
            try
            {
                result = method.Invoke(ListObject, new object[] { ndx });
            }
            catch (Exception)
            {
                throw BuildRunTimeException(node, "Access of list item at position " + ndx + " is out of range");
            }
            return null;
        }
        */

        /// <summary>
        /// Build a language exception due to the current token being invalid.
        /// </summary>
        /// <returns></returns>
        public static LangException BuildRunTimeException(AstNode node, string message)
        {
            return new LangException("Runtime Error", message, node.Ref.ScriptName, node.Ref.Line, node.Ref.CharPos);
        }
    }
}
