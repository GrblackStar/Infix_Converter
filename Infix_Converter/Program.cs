using System;

namespace Infix_Converter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Infix expression:   ");
            string inputExpression = Console.ReadLine();
            
            
            Console.WriteLine();
            Console.WriteLine("Reverse Polish Notation  --->  POSTFIX:  STACK IMPLEMENTATION");
            Console.WriteLine(PolishNotationConverter.PostFixStack(inputExpression));
            Console.WriteLine();
            


            inputExpression = "(" + inputExpression;
            inputExpression += ")";

            ExpressionTree root = PolishNotationConverter.BuildTree(inputExpression);
            PolishNotationConverter.VisualizeTree(root);


            Console.WriteLine();
            Console.WriteLine("Reverse Polish Notation  --->  POSTFIX:  TREE IMPLEMENTATION");
            PolishNotationConverter.PostFix(root);
            Console.WriteLine();


            Console.WriteLine();
            Console.WriteLine("Normal Polish Notation  --->  PREFIX:  TREE IMPLEMENTATION");
            PolishNotationConverter.PreFix(root);
            Console.WriteLine();



        }

        
    }
}
