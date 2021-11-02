using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    private string _commandId;
    private string _commandDesc;
    private string _commandFormat;

    public string commandId { get { return _commandId; } }
    public string commnadDesc { get { return _commandDesc; } }
    public string commandFormat { get { return _commandFormat; } }

    public DebugCommandBase(string id, string desc, string format)
    {
        _commandId = id;
        _commandDesc = desc;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    Action command;

    public DebugCommand(string id, string desc, string format, Action command) : base (id, desc, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    Action<T1> command;

    public DebugCommand(string id, string desc, string format, Action<T1> command) : base(id, desc, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}
