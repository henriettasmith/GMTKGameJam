#if TOOLS
using Godot;
using System;

[Tool]
public class SaveLevelButton : Button
{
    public override void _EnterTree()
    {
        Connect("pressed", this, "clicked");
    }

    public void clicked()
    {
        GD.Print("You clicked me!");
    }
}
#endif