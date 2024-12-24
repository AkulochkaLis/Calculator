using Godot;
using System;
using System.Dynamic;

public partial class Screen : Label
{
    public void PrintScreen(string textInt)
    {
        Text = textInt;
    }
    public void OnScreen()
    {
        HFlowContainer hFlowContainer = GetNode<HFlowContainer>("/root/Node2D/HBoxContainer/Buttons/Buttons/HFlowContainer");
        GD.Print(hFlowContainer);
        GD.Print("Here");
        //OnScreen();
    }
}

