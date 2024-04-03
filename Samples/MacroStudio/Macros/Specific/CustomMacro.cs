using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Enumerations;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Scripting;
using Genetec.Sdk.Scripting.Interfaces.Attributes;
using System;
using System.Data;
using System.Linq;
using System.Xml;

[MacroParameters()]
public sealed class CustomMacro : UserMacro
{
    /// <summary>
    /// Entry point of the macro. Provide an implementation of this method.
    /// </summary>
    public override void Execute()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Called when the macro needs to clean up. 
    /// </summary>
    protected override void CleanUp()
    {
        // Release objects created by the Execute method, unhook events, and so on.
    }
}