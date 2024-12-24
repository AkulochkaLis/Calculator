using Godot;
using System;
using System.Data;
using System.Diagnostics;
// как работает калькулятор: 
// 1. получает число
// 2. запоминает число  как leftNumber
// 3. выведи leftNumber
// 4. получает действие
// 5. запоминает действие  как operator
// 6. если operator =, то:
//   6.1. выведи leftNumber
// 7. если не =, то:
//   7.1 получает число 
//   7.2 запоминает число как rightNumber 
//   7.3 выведи rightNumber 
//   7.4 получает действие   
//   7.5 применяет operator к leftNumber и rightNumber
//   7.6 запоминает как leftNumber 
//   7.7 запоминает действие  как operator
//   7.8 если operator не =, то:
//     7.8.1 перейди к пункту 7 
//   7.9 если operator =, то:
//     7.9.1  выводит leftNumber

public partial class Calc : Node2D
{
	private double leftNumber;
	private double rightNumber;
	private double fractional;

	private double result;

	private string _operator = "";
	// - данное свойствое описывает состояние кал-ра, где default - стандартное состояние
	private string state = "default";

	private string numScreen = "";

	private string numScreenR = "";

	private Screen screen;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screen = GetNode<Screen>("/root/Node2D/PanelContainer/Screen");
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// принимает число, запоминает и выводит его 
	public void Number(double num)
	{

		if (state == "default")
		{
			leftNumber = Double.Parse(leftNumber.ToString() + num.ToString());
			NumberStr(leftNumber);
		}
		if (state == "fractional")
		{
			fractional = Double.Parse(fractional.ToString() + num.ToString());
			result = Double.Parse($"{leftNumber.ToString()},{fractional.ToString()}");
			NumberStr(result);
		}
		if (state == "rightInput")
		{
			rightNumber = Double.Parse(rightNumber.ToString() + num.ToString());
			NumberStr(rightNumber);
		}

	}

	public void NumberStr(double numStr)
	{
		if (state == "default" && leftNumber == 0)
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
			numScreen = "";
			leftNumber = 0;
			rightNumber = 0;
			_operator = "";
			screen.PrintScreen(leftNumber.ToString());
			return;
		}


		if (state == "default")
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
				leftNumber = leftNumber + rightNumber;
				NumberStr(leftNumber);
				GD.Print(leftNumber.ToString("### ### ###.##"));
			}

			if (_operator == "-")
			{
				leftNumber = leftNumber - rightNumber;
				NumberStr(leftNumber);
			}

			if (_operator == "*")
			{
				leftNumber = leftNumber * rightNumber;
				NumberStr(leftNumber);
			}

			if (_operator == "/")
			{
				if (rightNumber == 0)
				{
					screen.PrintScreen("Error");
				}
				leftNumber = leftNumber / rightNumber;
				NumberStr(leftNumber);
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
}
