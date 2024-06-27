using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StringCalculator.Source
{
    using num = Double;
    using order = Byte;
    using pos = Int16;

    public static class StringCalculator
    {
        #region Const strings
        private const string AllNumsConstants = ".1234567890epi";
        private const string AllNumsPoint = ".1234567890";
        private const string AllNums = "1234567890";

        private const string AllOperations = "+-*/^√!";
        private const string Type0Operations = "+-*/^";
        private const string Type1OperationsSpecial = "√(";
        private const string Type2OperationsSpecial = "!)";

        private const char PlusOperation = '+';
        private const string PlusOperationString = "+";
        private const char MinusOperation = '-';
        private const string MinusOperationString = "-";
        private const char MultiplyOperation = '*';
        private const string MultiplyOperationString = "*";
        private const char DivisionOperation = '/';
        private const string DivisionOperationString = "/";
        private const char DegreeOperation = '^';
        private const string DegreeOperationString = "^";
        private const char SqrtOperation = '√';
        private const string SqrtOperationString = "√";
        private const char FactOperation = '!';
        private const string FactOperationString = "!";
        private const char OpenParenthesis = '(';
        private const char ClosedParenthesis = ')';
        private const char Point = '.';
        private const char Comma = ',';
        private const string PI = "pi";
        private const char Zero = '0';
        private const string ZeroString = "0";
        private const char E = 'e';
        #endregion

        #region Static readonly data
        private delegate num OperationDelegate(num num1, num num2);

        private static readonly Dictionary<char, OperationDelegate> Operations = new Dictionary<char, OperationDelegate>()
        {
            { MinusOperation, (num1, num2) => num1 - num2 },
            { PlusOperation, (num1, num2) => num1 + num2 },
            { MultiplyOperation, (num1, num2) => num1 * num2 },
            { DivisionOperation, (num1, num2) => num1 / num2 },
            { FactOperation, (num1, num2) => Factorial((int)num1) },
            { SqrtOperation, (num1, num2) => Math.Sqrt(num2) }
        };
        private static readonly Dictionary<char, order> OrderSigns = new Dictionary<char, order>()
        { 
            { PlusOperation, 1 },
            { MinusOperation, 1 },
            { MultiplyOperation, 2 },
            { DivisionOperation, 2 },
            { DegreeOperation, 3 },
            { SqrtOperation, 4 },
            { FactOperation, 5 }
        };
        private static readonly Dictionary<char, pos> PosTypes = new Dictionary<char, pos>()
        {
            { PlusOperation, 0 },
            { MinusOperation, 0 },
            { MultiplyOperation, 0 },
            { DivisionOperation, 0 },
            { DegreeOperation, 0 },
            { SqrtOperation, 1 },
            { FactOperation, 2 }
        };
        #endregion

        #region Calculate operations
        private static num Factorial(int num)
        {
            if (num < 2)
                return 1;
            return num * Factorial(num - 1);
        }
        private static num Operation(char sign, num num1, num num2)
        {
            return Operations[sign].Invoke(num1, num2);
        }
        #endregion
        
        /// <summary>
        /// Main function of calculator
        /// </summary>
        /// <returns>
        /// Double number (if expression in int - without float digit problem)
        /// </returns>
        public static num Calculate(string expression)
        {
            if (expression == string.Empty)
                return 0;

            // Format input
            expression = expression.ToLower();
            expression = expression.Replace(Point, Comma);
            expression = expression.Replace(':', DivisionOperation);
            expression = expression.Replace('\\', DivisionOperation);
            expression = expression.Replace("**", "^");

            // Temporarity params
            var length = expression.Length;
            var stringNums = new List<string>();
            var localNum = string.Empty;
            
            pos power = 0;
            pos targetNum = 0;
            order nextError = 0;
            bool hasNums = false;

            // Lists (all return params)
            var nums = new List<num>();
            var signs = new List<char>();
            var order = new List<pos>();
            var target = new List<pos>();

            // Collect data and write lists
            for (int i = 0; i < length; i++)
            {
                char s = expression[i];
                bool isNum = false;
                bool notFirstChar = i > 0;
                bool notLastChar = i + 1 < length;

                if (Type0Operations.Contains(s))//+-*/^
                {
                    target.Add(targetNum);
                    targetNum++;

                    if (notFirstChar)
                    {
                        bool numsBool = AllNumsConstants.Contains(expression[i - 1]);//.1234567890epi
                        bool type2Bool = Type2OperationsSpecial.Contains(expression[i - 1]);//√(
                        if (!numsBool && !type2Bool)
                            nextError++;
                    }
                    else
                        nextError++;

                    if (notLastChar)
                    {
                        bool numsBool = AllNumsConstants.Contains(expression[i + 1]);//.1234567890epi
                        bool type1Bool = Type1OperationsSpecial.Contains(expression[i + 1]);//!)
                        if (!numsBool && !type1Bool)
                            nextError++;
                    }
                    else
                        nextError++;

                    signs.Add(s);
                    order.Add(power);
                }
                else if (s == FactOperation)//!
                {
                    target.Add(targetNum);
                    signs.Add(s);
                    order.Add(power);

                    if (notFirstChar)
                    {
                        bool numsBool = AllNumsConstants.Contains(expression[i - 1]);//.1234567890epi
                        bool type2Bool = Type2OperationsSpecial.Contains(expression[i - 1]);//!
                        if (!numsBool && !type2Bool)
                            nextError++;
                    }
                    else
                        nextError++;

                    if (notLastChar)
                    {
                        if (AllNumsConstants.Contains(expression[i + 1]))//.1234567890epi
                        {
                            signs.Add(MultiplyOperation);
                            order.Add(power);
                            target.Add(targetNum);
                            targetNum++;
                        }
                    }
                }
                else if (s == SqrtOperation)//√
                {
                    target.Add(targetNum);

                    if (notFirstChar)
                    {
                        if (AllNumsConstants.Contains(expression[i - 1]))//.1234567890epi
                        {
                            signs.Add(MultiplyOperation);
                            order.Add(power);
                            targetNum++;
                            target.Add(targetNum);
                        }
                    }

                    if (notLastChar)
                    {
                        bool numsBool = AllNumsConstants.Contains(expression[i + 1]);//.1234567890epi
                        bool type1Bool = Type1OperationsSpecial.Contains(expression[i + 1]);//√
                        if (!numsBool && !type1Bool)
                            nextError++;
                    }
                    else
                        nextError++;

                    signs.Add(s);
                    order.Add(power);
                }
                else if (s == OpenParenthesis)//(
                {
                    if (notFirstChar)
                    {
                        if (AllNumsConstants.Contains(expression[i - 1]))//.1234567890epi
                        {
                            signs.Add(MultiplyOperation);
                            order.Add(power);
                            target.Add(targetNum);
                            targetNum++;
                        }
                    }

                    if (!notLastChar)
                        nextError++;

                    power++;
                }
                else if (s == ClosedParenthesis)//)
                {
                    power--;

                    if (notFirstChar)
                    {
                        if (s == OpenParenthesis)
                            nextError++;
                    }
                    else
                        nextError++;

                    if (notLastChar)
                    {
                        if (AllNumsConstants.Contains(expression[i - 1]))//.1234567890epi
                        {
                            signs.Add(MultiplyOperation);
                            order.Add(power);
                            target.Add(targetNum);
                            targetNum++;
                        }
                    }
                }
                else
                {
                    localNum += s;
                    isNum = true;
                }

                if (!isNum)
                {
                    if (localNum != string.Empty && localNum != MinusOperationString)//-
                    {
                        stringNums.Add(localNum);
                        localNum = string.Empty;
                    }
                }
                if (nextError > 0)
                {
                    for (order j = 0; j < nextError; j++)
                        stringNums.Add(ZeroString);
                    nextError = 0;
                }

                // Logs
                // Console.WriteLine();
            }

            // Checkout on error and other
            if (localNum != string.Empty && localNum != MinusOperationString)//-
                stringNums.Add(localNum);
            if (stringNums.Count == 0)
                return 0;

            //Clean numbers and find constants
            var hasPoint = false;
            length = stringNums.Count;
            
            for (var i = 0; i < length; i++)
            {
                var num = stringNums[i];
                localNum = string.Empty;
                num multiplier = 1;

                if (num.Contains(PI))
                {
                    multiplier *= Math.Pow(Math.PI, Regex.Matches(num, PI).Count);
                    num = num.Replace(PI, PlusOperationString);
                    hasNums = true;
                }
                if (num.Contains(E))
                {
                    multiplier *= Math.Pow(Math.E, num.Count(x => x == E));
                    num = num.Replace(E, PlusOperation);
                    hasNums = true;
                }

                int lengthNum = num.Length;
                for (int n = 0; n < lengthNum; n++)
                {
                    if (AllNums.Contains(num[n]))
                    {
                        localNum += num[n];
                        hasNums = true;
                    }
                    else if (num[n] == Comma)
                    {
                        if (!hasPoint)
                        {
                            localNum += num[n];
                            hasNums = hasPoint = true;
                        }
                    }
                    else if (localNum != string.Empty)
                    {
                        if (localNum[0] == Comma)
                            localNum = Zero + localNum;
                        if (localNum.Last() == Comma)
                            localNum += Zero;
                        multiplier *= double.Parse(localNum);
                        localNum = string.Empty;
                    }
                }

                if (localNum != string.Empty)
                {
                    if (localNum[0] == Comma)
                        localNum = Zero + localNum;
                    if (localNum.Last() == Comma)
                        localNum += Zero;
                    multiplier *= double.Parse(localNum);
                }
                nums.Add(hasNums ? multiplier : 0);
            }

            // Calculate
            for (pos i = 0; i < signs.Count; i++)
            {
                pos nextID = 0;
                pos signPower = OrderSigns[signs[0]];
                pos signOrder = order[0];

                for (pos j = 1; j < signs.Count; j++)
                {
                    pos localOrder = order[j];
                    pos localPower = OrderSigns[signs[j]];

                    bool orderPriority = signOrder < localOrder;
                    bool signPriority = signPower < localPower;
                    bool degPriority = signPower == OrderSigns[DegreeOperation];
                    bool degLocPriority = localPower == OrderSigns[DegreeOperation];
                    if (orderPriority || signOrder == localOrder && (signPriority || degPriority && degLocPriority))
                    {
                        nextID = j;
                        signPower = localPower;
                        signOrder = localOrder;
                    }
                }

                pos posType = PosTypes[signs[nextID]];
                if (posType == 0)
                {
                    nums[target[nextID]] = Operation(signs[nextID], nums[target[nextID]], nums[target[nextID] + 1]);
                    int last = target[nextID];
                    signs.RemoveAt(nextID);
                    order.RemoveAt(nextID);
                    nums.RemoveAt(target[nextID] + 1);
                    target.RemoveAt(nextID);
                    for (int j = 0; j < target.Count; j++)
                        if (target[j] > last)
                            target[j]--;
                }
                else if (posType == 1)
                {
                    nums[target[nextID]] = Operation(signs[nextID], 0, nums[target[nextID]]);
                    if (target[nextID] + 1 < nums.Count && signs.Count < nums.Count)
                    {
                        nums[target[nextID]] = Operation(MultiplyOperation, nums[target[nextID]], nums[target[nextID] + 1]);
                        nums.RemoveAt(target[nextID] + 1);
                    }
                    signs.RemoveAt(nextID);
                    order.RemoveAt(nextID);
                    target.RemoveAt(nextID);
                }
                else if (posType == 2)
                {
                    nums[target[nextID]] = Operation(signs[nextID], nums[target[nextID]], 0);
                    signs.RemoveAt(nextID);
                    order.RemoveAt(nextID);
                    target.RemoveAt(nextID);
                }
            }
            return nums[0];
        }
    }
}