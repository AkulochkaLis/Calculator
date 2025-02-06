using Godot;
using System;
using System.Collections.Generic;

namespace Calculator;

enum CalculatorState
{
    LeftInput,
    LeftFractional,
    RightInput,
    RightFractional,
}

enum Action
{
    Plus,
    Minus,
    Division,
    Multiplication,
    Reset,
    Comma,
    Equal,
    Noop,
}

class MyNumber
{
    public int number = 0;
    public int fractional = 0;

    public void AddNumber(int num)
    {
        number = int.Parse($"{number}{num}");
    }

    public void AddFractional(int num)
    {
        fractional = int.Parse($"{fractional}{num}");
    }
}

public partial class Calc : Node2D
{
    private MyNumber leftNumber = new();
    private MyNumber rightNumber = new();

    private CalculatorState state = CalculatorState.LeftInput;

    private Action action;

    private Screen screen;

    public override void _Ready()
    {
        screen = GetNode<Screen>("/root/Node2D/PanelContainer/Screen");
    }

    public void ConvertingNumbers(double num)
    {
        switch (state)
        {
            case CalculatorState.LeftInput:
                {
                    leftNumber.AddNumber((int)num);
                    PrintNumber(leftNumber);
                    break;
                }
            case CalculatorState.LeftFractional:
                {
                    leftNumber.AddFractional((int)num);
                    PrintNumber(double.Parse($"{leftNumber},{leftFractional}"));
                    break;
                }
            case CalculatorState.RightInput:
                {
                    rightNumber.AddNumber((int)num);
                    PrintNumber(rightNumber);
                    break;
                }
            case CalculatorState.RightFractional:
                {
                    rightNumber.AddFractional((int)num);
                    PrintNumber(double.Parse($"{rightNumber},{rightFractional}"));
                    break;
                }
            default:
                {
                    throw new NotImplementedException("Unsupported state");
                }
        }
    }

    public void PerformingOperations(string operate)
    {
        Action currentAction = ConvertToAction(operate);

        if (currentAction == Action.Reset)
        {
            state = CalculatorState.LeftInput;
            leftFractional = 0;
            rightFractional = 0;
            leftNumber = 0;
            rightNumber = 0;
            action = Action.Noop;
            screen.PrintScreen(leftNumber.ToString());
            return;
        }

        if (currentAction == Action.Comma)
        {
            double numberToPrint = isLeftInput(state) ? leftNumber : rightNumber;
            screen.PrintScreen(numberToPrint.ToString() + ",");
            state = state switch
            {
                CalculatorState.RightInput => CalculatorState.RightFractional,
                CalculatorState.LeftInput => CalculatorState.LeftFractional,
                _ => throw new NotImplementedException("unsupported state"),
            };
            return;
        }

        if (isLeftInput(state) && currentAction == Action.Equal)
        {
            PrintNumber(leftNumber);
            return;
        }

        if (isLeftInput(state) && isMathAction(currentAction))
        {
            action = currentAction;
            screen.PrintScreen(ConvertActionToString(currentAction));
            state = CalculatorState.RightInput;
            return;
        }

        switch (action)
        {
            case Action.Plus:
                {
                    double result = double.Parse($"{leftNumber},{leftFractional}") + double.Parse($"{rightNumber},{rightFractional}");
                    PrintNumber(result);
                    SplitAndSaveNumber(result);
                    action = currentAction;
                    break;
                }
            case Action.Minus:
                {
                    double result = double.Parse($"{leftNumber},{leftFractional}") - double.Parse($"{rightNumber},{rightFractional}");
                    PrintNumber(result);
                    SplitAndSaveNumber(result);
                    action = currentAction;
                    break;
                }
            case Action.Multiplication:
                {
                    double result = double.Parse($"{leftNumber},{leftFractional}") * double.Parse($"{rightNumber},{rightFractional}");
                    PrintNumber(result);
                    SplitAndSaveNumber(result);
                    action = currentAction;
                    break;
                }
            case Action.Division:
                {
                    if (rightNumber == 0)
                    {
                        screen.PrintScreen("Error");
                    }
                    double result = double.Parse($"{leftNumber},{leftFractional}") / double.Parse($"{rightNumber},{rightFractional}");
                    PrintNumber(result);
                    SplitAndSaveNumber(result);
                    action = currentAction;
                    break;
                }
            case Action.Noop: break;
            default:
                {
                    throw new NotImplementedException($"Unsupported action {action}");
                }
        }

        if (currentAction == Action.Equal)
        {
            state = CalculatorState.LeftInput;
        }
        else
        {
            state = CalculatorState.RightInput;
        }
    }

    private Action ConvertToAction(string operate) => operate switch
    {
        "+" => Action.Plus,
        "-" => Action.Minus,
        "AC" => Action.Reset,
        "/" => Action.Division,
        "*" => Action.Multiplication,
        "=" => Action.Equal,
        "," => Action.Comma,
        _ => throw new NotImplementedException(),
    };

    private string ConvertActionToString(Action action) => action switch
    {
        Action.Plus => "+",
        Action.Minus => "-",
        Action.Reset => "AC",
        Action.Division => "/",
        Action.Multiplication => "*",
        _ => throw new NotImplementedException(),
    };

    private bool isServiceAction(Action action)
    {
        return new List<Action>() { Action.Comma, Action.Reset, Action.Equal }.Contains(action);
    }
    private bool isMathAction(Action action)
    {
        return !isServiceAction(action);
    }

    private bool isLeftInput(CalculatorState state)
    {
        return state == CalculatorState.LeftFractional || state == CalculatorState.LeftInput;
    }

    private bool isRightInput(CalculatorState state)
    {
        return !isLeftInput(state);
    }

    private void PrintNumber(double numStr)
    {
        if (numStr == 0)
        {
            screen.PrintScreen(leftNumber.ToString());
        }
        else if (state == CalculatorState.RightInput && rightNumber == 0)
        {
            screen.PrintScreen(rightNumber.ToString());
        }
        else
        {
            screen.PrintScreen(numStr.ToString("### ### ###.##"));
        }
    }

    private void SplitAndSaveNumber(double result)
    {
        string resultStr = result.ToString();
        string[] numberParts = resultStr.Split(",");
        leftNumber = double.Parse(numberParts[0]);
        leftFractional = numberParts.Length > 1 ? double.Parse(numberParts[1]) : 0;
        rightFractional = 0;
        rightNumber = 0;
    }
}

