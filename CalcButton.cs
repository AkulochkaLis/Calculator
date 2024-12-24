using Godot;
using System;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class CalcButton : Button
{
    public double textInt { get; set; }
    public override void _Ready()
    {
        // Code in this block run once for every button instance
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
            textInt = TextDouble();
            Calc calc = GetNode<Calc>("/root/Node2D");
            calc.Number(textInt);
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
