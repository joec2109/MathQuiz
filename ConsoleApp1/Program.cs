using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MathQuiz
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Math Quiz!");

            // Get student's name
            Console.Write("Enter your name: ");
            string studentName = Console.ReadLine();

            // Set time limit
            int timeLimitMinutes = 45; // Default time limit
            Console.Write("Enter the time limit in minutes (45 or 60): ");
            int.TryParse(Console.ReadLine(), out timeLimitMinutes);

            // Set number of questions
            int numQuestions = 20;

            // Generate and present questions
            int score = 0;
            int firstHalfScore = 0;
            int secondHalfScore = 0;
            Random rnd = new Random();

            int additionCount = 0;
            int subtractionCount = 0;
            int multiplicationCount = 0;
            int divisionCount = 0;

            string question;
            string questionToCalculate = "";

            for (int i = 1; i <= numQuestions; i++)
            {
                
                if (i <= 10)
                {
                    // Generate question with one arithmetic symbol
                    char op = GetRandomOperator(rnd);
                    while ((op == '+' && additionCount == 3) || (op == '-' && subtractionCount == 3) || (op == '*' && multiplicationCount == 2) || (op == '/' && divisionCount == 2)) {
                        op = GetRandomOperator(rnd);
                    }
                    int num1 = rnd.Next(-50, 51);
                    int num2 = rnd.Next(-50, 51);
                    while (num2 == num1) {
                        num2 = rnd.Next(-50, 51);
                    }
                    question = $"{num1} {op} {num2}";

                    // Update count of arithmetic symbols
                    switch (op)
                    {
                        case '+':
                            additionCount++;
                            break;
                        case '-':
                            subtractionCount++;
                            break;
                        case '*':
                            multiplicationCount++;
                            break;
                        case '/':
                            divisionCount++;
                            break;
                    }
                }

                else
                {
                    // Generate question with a combo of two arithmetic symbols
                    char op1 = GetRandomOperator(rnd);
                    char op2 = GetRandomOperator(rnd);
                    int num1 = rnd.Next(-50, 51);
                    int num2 = rnd.Next(-50, 51);
                    int num3 = rnd.Next(-50, 51);
                    question = $"{num1} {op1} {num2} {op2} {num3}";
                    questionToCalculate = $"{num1} {op1} {num2} {op2} {num3}";

                    if (op1 == '/' && (op2 == '*' || op2 == '+' || op2 == '-')) {
                        questionToCalculate = $"({num1} {op1} {num2}) {op2} {num3}";
                    } else if (op1 == '*' && (op2 == '+' || op2 == '-')) {
                        questionToCalculate = $"({num1} {op1} {num2}) {op2} {num3}";
                    } else if (op1 == '+' && op2 == '-') {
                        questionToCalculate = $"({num1} {op1} {num2}) {op2} {num3}";
                    } else if (op2 == '/' && (op1 == '*' || op1 == '+' || op1 == '-')) {
                        questionToCalculate = $"{num1} {op1} ({num2} {op2} {num3})";
                    } else if (op2 == '*' && (op1 == '+' || op1 == '-')) {
                        questionToCalculate = $"{num1} {op1} ({num2} {op2} {num3})";
                    } else if (op2 == '+' && op1 == '-') {
                        questionToCalculate = $"{num1} {op1} ({num2} {op2} {num3})";
                    }
                }

                double answer = EvaluateQuestionFirst10(question);

                if (i > 10) {
                    answer = EvaluateQuestion(questionToCalculate);
                }
                
                
                Console.Write($"Question {i}: {question} = ");
                double userAnswer;
                while (!double.TryParse(Console.ReadLine(), out userAnswer))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.Write($"Question {i}: {question} = ");
                }

                if (userAnswer == answer)
                {
                    Console.WriteLine("Correct!");
                    score++;
                    if (i <= 10) {
                        firstHalfScore += 1;
                    } else {
                        secondHalfScore += 1;
                    }
                }
                else
                {
                    Console.WriteLine($"Incorrect! The correct answer is {answer}.");
                }
            }

            // Check if there are two of each arithmetic symbol in the first 10 questions
            bool hasTwoOfEachSymbol = additionCount >= 2 && subtractionCount >= 2 && multiplicationCount == 2 && divisionCount == 2;

            // Calculate final score
            Console.WriteLine($"Your overall score: {score}");

            // Determine performance level
            string performanceLevel = GetPerformanceLevel(score);
            Console.WriteLine($"Performance level: {performanceLevel}");

            // Generate and save report
            GenerateReport(studentName, score, firstHalfScore, secondHalfScore);
        }

        static char GetRandomOperator(Random rnd)
        {
            char[] operators = { '+', '-', '*', '/' };
            return operators[rnd.Next(0, operators.Length)];
        }

        static double EvaluateQuestion(string questionToCalculate)
        {
            double result = Convert.ToDouble(new System.Data.DataTable().Compute(questionToCalculate, null));
            return Math.Round(result, 2);
        }

        static double EvaluateQuestionFirst10(string question)
        {
            double result = Convert.ToDouble(new System.Data.DataTable().Compute(question, null));
            return Math.Round(result, 2);
        }


        static string GetPerformanceLevel(int score)
        {
            if (score >= 17)
                return "Distinction";
            else if (score >= 11)
                return "Merit";
            else if (score >= 5)
                return "Pass";
            else
                return "Fail";
        }

        static void GenerateReport(string studentName, int score, int firstHalfScore, int secondHalfScore)
        {
            // Generate and save report to a text file
            string fileName = $"{studentName}_report.txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.WriteLine($"Student Name: {studentName}");
                file.WriteLine($"Overall Score: {score}");
                file.WriteLine($"Performance Level: {GetPerformanceLevel(score)}");
                file.WriteLine($"First 10 questions score: {firstHalfScore}\tSecond 10 questions score: {secondHalfScore}");
                // Additional details or breakdown of performance can be added here
            }

            Console.WriteLine($"Report saved as {fileName}");
        }
    }
}