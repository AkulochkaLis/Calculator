using Godot;
using System;
using System.Collections;

namespace Calculator;

enum CalculatorState
{
    Default,
    LeftFractional,
    RightInput,
    RightFranctional,
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
}

public partial class Calc : Node2D
{
    private double leftNumber;
    private double rightNumber;
    private double fractional;

    private double rightFractional;

    private CalculatorState state = CalculatorState.Default;

    private Action _operator;

    private Screen screen;

    public override void _Ready()
    {
        screen = GetNode<Screen>("/root/Node2D/PanelContainer/Screen");
    }

    public void ConvertingNumbers(double num)
    {
        switch (state)
        {
            case CalculatorState.Default:
                {
                    leftNumber = double.Parse(leftNumber.ToString() + num.ToString());
                    PrintNumber(leftNumber);
                    break;
                }
            case CalculatorState.LeftFractional:
                {
                    fractional = double.Parse(fractional.ToString() + num.ToString());
                    PrintNumber(double.Parse($"{leftNumber},{fractional}"));
                    break;
                }
            case CalculatorState.RightInput:
                {
                    rightNumber = double.Parse(rightNumber.ToString() + num.ToString());
                    PrintNumber(rightNumber);
                    break;
                }
            case CalculatorState.RightFranctional:
                {
                    rightFractional = double.Parse(rightFractional.ToString() + num.ToString());
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
        Action action = ConvertToAction(operate);

        if (action == Action.Reset)
        {
            state = CalculatorState.Default;
            fractional = 0;
            rightFractional = 0;
            leftNumber = 0;
            rightNumber = 0;
            _operator = action;
            screen.PrintScreen(leftNumber.ToString());
            return;
        }

        if (state == CalculatorState.RightInput && action == Action.Comma)
        {
            screen.PrintScreen(rightNumber.ToString() + ",");
            state = CalculatorState.RightFranctional;
            return;
        }

        if (state == CalculatorState.Default || state == CalculatorState.LeftFractional)
        {
            if (action != Action.Equal && action != Action.Comma)
            {
                _operator = action;
                screen.PrintScreen(ConvertActionToString(action));
                state = CalculatorState.RightInput;
                return;
            }

            if (action == Action.Comma)
            {
                screen.PrintScreen(leftNumber.ToString() + ",");
                state = CalculatorState.LeftFractional;
                return;
            }
        }

        else
        {
            if (action == Action.Equal)
            {
                state = CalculatorState.Default;
            }

            if (action != Action.Equal)
            {
                _operator = action;
                screen.PrintScreen(ConvertActionToString(action));
                return;
            }

            switch (_operator)
            {
                case Action.Plus:
                    {
                        double result = double.Parse($"{leftNumber},{fractional}") + double.Parse($"{rightNumber},{rightFractional}");
                        PrintNumber(result);
                        SplitAndSaveNumber(result);
                        _operator = action;
                        break;
                    }
                case Action.Minus:
                    {
                        double result = double.Parse($"{leftNumber},{fractional}") - double.Parse($"{rightNumber},{rightFractional}");
                        PrintNumber(result);
                        SplitAndSaveNumber(result);
                        _operator = action;
                        break;
                    }
                case Action.Multiplication:
                    {
                        double result = double.Parse($"{leftNumber},{fractional}") * double.Parse($"{rightNumber},{rightFractional}");
                        PrintNumber(result);
                        SplitAndSaveNumber(result);
                        _operator = action;
                        break;
                    }
                case Action.Division:
                    {
                        if (rightNumber == 0)
                        {
                            screen.PrintScreen("Error");
                        }
                        double result = double.Parse($"{leftNumber},{fractional}") / double.Parse($"{rightNumber},{rightFractional}");
                        PrintNumber(result);
                        SplitAndSaveNumber(result);
                        _operator = action;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Unsupported action");
                    }
            }
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
        fractional = numberParts.Length > 1 ? double.Parse(numberParts[1]) : 0;
        rightFractional = 0;
        rightNumber = 0;
        state = CalculatorState.RightInput;
    }
}

