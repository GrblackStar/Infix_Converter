using System;
using System.Collections.Generic;

namespace Infix_Converter
{
    internal class PolishNotationConverter
    {
        public static ExpressionTree NewNode (char c)
        {
            ExpressionTree node = new ExpressionTree ();
            node.value = c;
            node.Left = null;
            node.Right = null;
            return node;
        }

        public static ExpressionTree BuildTree(string inputString)
        {
            // stack to hold the notes:
            Stack<ExpressionTree> stackNodes = new Stack<ExpressionTree>();

            // stack for the characters:
            Stack<char> stackChars = new Stack<char>();
            ExpressionTree t, t1, t2, t3;


            // stacks in case we have assignment:
            Stack<char> assignmentOps = new Stack<char>();
            Stack<ExpressionTree> assignmentChars = new Stack<ExpressionTree>();


            // priority/precedence
            int[] priority = new int[123];
            //priority['='] = 1;
            priority['+'] = priority['-'] = 2;
            priority['/'] = priority['*'] = 3;
            priority['^'] = 4;
            priority[')'] = 0;
            

            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == '(')
                {
                    stackChars.Push(inputString[i]);  // Pushing to the stack 
                }
                else if ((Char.IsLetter(inputString[i]) || Char.IsDigit(inputString[i]))   && inputString[i+1] == '=')
                {
                    t = NewNode(inputString[i]);
                    assignmentChars.Push(t);
                }
                else if (inputString[i] == '=')
                {
                    assignmentOps.Push(inputString[i]);
                }
                else if (Char.IsLetter(inputString[i])  || Char.IsDigit(inputString[i]))
                {
                    // pushing to the node stack:
                    t = NewNode(inputString[i]);
                    stackNodes.Push(t);
                }
                else if (priority[inputString[i]] > 0)
                {
                    // if an operand with the same priority or lower appears:
                    while (stackChars.Count != 0    &&    stackChars.Peek() != '('
                            &&   ((inputString[i] != '^'    &&    priority[stackChars.Peek()] >= priority[inputString[i]])
                            ||    (inputString[i] == '^'    &&    priority[stackChars.Peek()] > priority[inputString[i]]) ))
                    {

                        // Get and remove the top element
                        // from the character stack
                        t = NewNode(stackChars.Peek());
                        stackChars.Pop();

                        t1 = stackNodes.Peek();
                        stackNodes.Pop();

                        t2 = stackNodes.Peek();
                        stackNodes.Pop();

                        // Update the tree
                        t.Left = t2;
                        t.Right = t1;

                        // Push the node to the node stack
                        stackNodes.Push(t);
                    }

                    stackChars.Push(inputString[i]);
                }
                else if (inputString[i] == ')')
                {
                    while (stackChars.Count != 0  &&  stackChars.Peek() != '(')
                    {
                        t = NewNode(stackChars.Peek());
                        stackChars.Pop();
                        t1 = stackNodes.Peek();
                        stackNodes.Pop();
                        t2 = stackNodes.Peek();
                        stackNodes.Pop();
                        t.Left = t2;
                        t.Right = t1;
                        stackNodes.Push(t);

                    }
                    stackChars.Pop();
                }

            }

            t = stackNodes.Peek();

            while (assignmentOps.Count != 0)
            {
                if (assignmentOps.Count != 0 && assignmentChars.Count != 0)
                {
                    t3 = NewNode(assignmentOps.Pop());
                    t3.Right = t;
                    t3.Left = assignmentChars.Pop();
                    stackNodes.Push(t3);
                    
                }
                t = stackNodes.Peek();
            }
            
            return t;

        }

        public static void PostFix(ExpressionTree root)
        {
            if (root != null)
            {
                PostFix(root.Left);
                PostFix(root.Right);
                Console.Write(root.value + " ");
            }
        }

        public static void PreFix(ExpressionTree root)
        {
            if (root != null)
            {
                Console.Write(root.value + " ");
                PreFix(root.Left);
                PreFix(root.Right);
            }
        }

        public static void VisualizeTree(ExpressionTree root, string prefix = "", bool isLeft = true)
        {
            if (root == null)
            {
                return;
            }

            if (root.Right != null)
            {
                VisualizeTree(root.Right, prefix + (isLeft ? "|   " : "    "), false);
            }

            Console.Write(prefix);
            Console.Write(isLeft ? "└── " : "┌── ");
            Console.WriteLine("(" + root.value + ")");

            if (root.Left != null)
            {
                VisualizeTree(root.Left, prefix + (isLeft ? "    " : "|   "), true);
            }
        }

        private static int GetPrecedence(char op)
        {
            // Method to determine the precedence/priority of operators
            switch (op)
            {
                case '=':
                    return 1;
                case '+':
                case '-':
                    return 2;
                case '*':
                case '/':
                    return 3;
                case '^':
                    return 4;
                default:
                    return -1;
            }

            
        }

        public static string PostFixStack(string expression)
        {
            // Method to convert infix expression to reverse Polish notation   ->   POSTFIX
            // Implemented with stack  -->  shunting yard algorithm  -->   Dijkstra's algorithm
            Stack<char> stack = new Stack<char>();
            string output = "";

            Console.WriteLine("    Current Symbol     Operator Stack    Postfix String  ");

            foreach (char c in expression)
            {
                Console.Write("          {0, -2}", c);

                if (Char.IsLetter(c) || Char.IsDigit(c))
                {
                    output += c;
                    output += " ";

                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    while (stack.Count > 0 && stack.Peek() != '(' && GetPrecedence(c) <= GetPrecedence(stack.Peek()))
                    {
                        output += stack.Pop();
                        output += " ";
                    }

                    stack.Push(c);
                }
                else if (c == '(')
                {
                    stack.Push(c);
                }
                else if (c == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        output += stack.Pop();
                        output += " ";
                    }
                    stack.Pop();
                }
                else if (c == '=')
                {
                    stack.Push((char)c);
                }
                else if (c == '^')
                {
                    stack.Push((char)c);
                }

                string stackchars = "";
                foreach (char ch in stack)
                {
                    //Console.Write(ch + " ");
                    stackchars += ch;
                    stackchars += " ";
                }

                Console.Write("                 {0, -20}", stackchars);

                string outputchars = "";
                foreach (char ch in output)
                {
                    outputchars += ch;
                    outputchars += " ";
                }
                Console.Write("{0, -30}", outputchars);

                Console.WriteLine();
            }

            while (stack.Count > 0)
            {
                output += stack.Pop();
                output += " ";
            }
            Console.WriteLine();
            Console.WriteLine();

            return (expression + "   --->>>   " + output);
        }
        
    }
}
