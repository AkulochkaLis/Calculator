using Godot;
using System;

namespace Calculator;

public partial class CalcButton : Button
{
    public double TextInt { get; set; }
    public override void _Ready()
    {
        Pressed += OnClick;
    }

    public void OnClick()
    {

        if (Text == "+" || Text == "-" || Text == "/" || Text == "*" || Text == "=" || Text == "AC" || Text == ",")
        {
            Calc calc = GetNode<Calc>("/root/Node2D");
            calc.Operator(Text);
        }
        else
        {
            TextInt = TextDouble();
            Calc calc = GetNode<Calc>("/root/Node2D");
            calc.Number(TextInt);
        }
    }

    private void AC()
    {
        Screen screen = GetNode<Screen>("/root/Node2D/PanelContainer/Screen");
        screen.PrintScreen("0");
    }

    private double TextDouble()
    {
        return Convert.ToDouble(Text);
    }
}
