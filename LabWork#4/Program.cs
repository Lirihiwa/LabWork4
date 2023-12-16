using System;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;

class LabWork4
{
    public static Dictionary<char, int> operatorsDict = new Dictionary<char, int>()
    {
        { '(', 1 },
        { '+', 2 },
        { '-', 2 },
        { '*', 3 },
        { '/', 3 },
        { ')', 1 }
    };

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("Введите пример: ");

            string userInput = Console.ReadLine().Replace(" ", "");
            Console.Write("\nОбратная польская запись: ");

            List<string> example = GetExample(userInput);
            List<string> rpn = GetRPN(example);

            foreach (string output in rpn) { Console.Write(output + " "); }
            Console.WriteLine("\nОтвет: " + GetResult(rpn) + "\n<<Нажмите любую клавишу>>");
            Console.ReadKey();
            Console.Clear();
        }

    }

    public static double GetResult(List<string> rpn)
    {
        double result = 0;

        Stack<string> numbers = new Stack<string>();
        foreach (string s in rpn)
        {
            if (IsNum(s))
            {
                numbers.Push(s);
            }
            else
            {
                double operand2 = Convert.ToDouble(numbers.Pop());
                double operand1 = Convert.ToDouble(numbers.Pop());

                switch (s)
                {
                    case "+": result = operand1 + operand2; break;
                    case "-": result = operand1 - operand2; break;
                    case "*": result = operand1 * operand2; break;
                    case "/": result = operand1 / operand2; break;
                }
                numbers.Push(Convert.ToString(result));
            }
        }

        return result;
    }

    public static List<string> GetExample(string userInput)
    {
        List<string> example = new List<string>();

        string currNum = null;
        int counter = 0;
        foreach (char currSymbol in userInput)
        {
            if ("0123456789,".Contains(currSymbol) == true)
            {
                currNum += currSymbol;
                if (counter == userInput.Length - 1)
                {
                    example.Add(currNum);
                }
            }
            else
            {
                if (currNum != null)
                {
                    example.Add(currNum);
                    currNum = null;
                }
            }
            if ("()+-*/".Contains(currSymbol) == true)
            {
                example.Add(Convert.ToString(currSymbol));
            }
            counter++;
        }

        return example;
    }

    public static List<string> GetRPN(List<string> example)
    {
        List<string> rpn = new List<string>();
        Stack<string> operators = new Stack<string>();

        int counter = 0;
        foreach (string item in example)
        {
            if (IsNum(item) == true)
            {
                rpn.Add(item);
                if (counter == example.Count - 1)
                {
                    while (operators.Count > 0)
                    {
                        rpn.Add(operators.Pop());
                    }
                }
                counter++;
                continue;
            }
            else
            {
                if (operators.Count == 0)
                {
                    operators.Push(item);

                    counter++;
                    continue;
                }   
                else if (operatorsDict[Convert.ToChar(operators.Peek())] < operatorsDict[Convert.ToChar(item)] || item == "(")
                {
                    operators.Push(item);

                    counter++;
                    continue;
                }
                else if (operatorsDict[Convert.ToChar(operators.Peek())] >= operatorsDict[Convert.ToChar(item)] || item == ")")
                {
                    while (operators.Count() != 0 && operatorsDict[Convert.ToChar(operators.Peek())] >= operatorsDict[Convert.ToChar(item)])
                    {
                        if (operators.Peek() != "(") rpn.Add(operators.Pop());
                        else
                        { operators.Pop(); break; }
                    }

                    if(operators.Count() != 0 & counter == example.Count() - 1)
                    {
                        while (operators.Count() > 0)
                        {
                            rpn.Add(operators.Pop());
                        }

                        counter++;
                        continue;
                    }
                    else if (item != ")")
                        operators.Push(item);

                    counter++;
                    continue;
                }
                
            }
        }
        return rpn;
    }
    public static void PoppingStack(Stack<string> operators, List<string> rpn)
    {
        while (operators.Peek() != "(")
        {
            rpn.Add(operators.Pop());
        }
            operators.Pop();
        }

        public static bool IsNum(string item)
        {
            return Double.TryParse(item, out double number);
        }
}
