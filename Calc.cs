using Godot;
using System;

namespace Calculator;

public partial class Calc : Node2D
{
	private double leftNumber;
	private double rightNumber;
	private double fractional;

	private double rightFractional;

	private string _operator = "";
	private string state = "default";

	private Screen screen;

	public override void _Ready()
	{
		screen = GetNode<Screen>("/root/Node2D/PanelContainer/Screen");
	}

	public void Number(double num)
	{

		if (state == "default")
		{
			leftNumber = Double.Parse(leftNumber.ToString() + num.ToString());
			PrintNumber(leftNumber);
		}
		if (state == "fractional")
		{
			fractional = Double.Parse(fractional.ToString() + num.ToString());
			PrintNumber(Double.Parse($"{leftNumber.ToString()},{fractional.ToString()}"));
		}
		if (state == "rightInput")
		{
			rightNumber = Double.Parse(rightNumber.ToString() + num.ToString());
			PrintNumber(rightNumber);
		}
		if (state == "rightFractional")
		{
			rightFractional = Double.Parse(rightFractional.ToString() + num.ToString());
			PrintNumber(Double.Parse($"{rightNumber.ToString()},{rightFractional.ToString()}"));
		}
	}

	public void PrintNumber(double numStr)
	{
		if (numStr == 0)
		{
			screen.PrintScreen(leftNumber.ToString());
		}
		else if (state == "rightInput" && rightNumber == 0)
		{
			screen.PrintScreen(rightNumber.ToString());
		}
		else
		{
			screen.PrintScreen(numStr.ToString("### ### ###.##"));
		}
	}


	public void Operator(string operate)
	{
		if (operate == "AC")
		{
			state = "default";
			fractional = 0;
			rightFractional = 0;
			leftNumber = 0;
			rightNumber = 0;
			_operator = "";
			screen.PrintScreen(leftNumber.ToString());
			return;
		}

		if (state == "rightInput" && operate == ",")
		{
			screen.PrintScreen(rightNumber.ToString() + ",");
			state = "rightFractional";
			return;
		}

		if (state == "default" || state == "fractional")
		{
			if (operate != "=" && operate != ",")
			{
				_operator = operate;
				screen.PrintScreen(_operator);
				state = "rightInput";
			}

			if (operate == ",")
			{
				screen.PrintScreen(leftNumber.ToString() + ",");
				state = "fractional";
			}
		}

		else
		{
			if (_operator == "+")
			{
				double result = double.Parse($"{leftNumber},{fractional}") + double.Parse($"{rightNumber},{rightFractional}");
				PrintNumber(result);
				SplitAndSafeNumber(result);
			}

			if (_operator == "-")
			{
				double result = double.Parse($"{leftNumber},{fractional}") - double.Parse($"{rightNumber},{rightFractional}");
				PrintNumber(result);
				SplitAndSafeNumber(result);
			}

			if (_operator == "*")
			{
				double result = double.Parse($"{leftNumber},{fractional}") * double.Parse($"{rightNumber},{rightFractional}");
				PrintNumber(result);
				SplitAndSafeNumber(result);
			}

			if (_operator == "/")
			{
				if (rightNumber == 0)
				{
					screen.PrintScreen("Error");
				}
				double result = double.Parse($"{leftNumber},{fractional}") / double.Parse($"{rightNumber},{rightFractional}");
				PrintNumber(result);
				SplitAndSafeNumber(result);
			}

			if (operate != "=")
			{
				_operator = operate;
			}

			if (operate == "=")
			{
				state = "default";
			}
		}
	}

	private void SplitAndSafeNumber(double result)
	{
		string resultStr;
		resultStr = result.ToString();
		string[] numberParts = resultStr.Split(",");
		leftNumber = double.Parse(numberParts[0]);
		if (numberParts.Length > 1)
		{
			fractional = double.Parse(numberParts[1]);
		}
		else
		{
			fractional = 0;
		}
		rightFractional = 0;
		rightNumber = 0;
		state = "rightInput";
	}
}
