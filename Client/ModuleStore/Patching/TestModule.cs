﻿using Harmony;
using LunaClient.Events;
using System.Reflection;

namespace LunaClient.ModuleStore.Patching
{
    /// <summary>
    /// This is a test class, we use the method "ActionCallMethodInfo/EventCallMethodInfo/MethodCallMethodInfo" to take the IL codes and put them on the real part module methods
    /// </summary>
    internal class TestModule : PartModule
    {
        public static readonly MethodInfo ExampleFieldChangeCallMethod = typeof(TestModule).GetMethod(nameof(ExampleFieldChangeCall), AccessTools.all);
        public static readonly MethodInfo AfterMethodCallMethodInfo = typeof(TestModule).GetMethod(nameof(AfterMethodCall), AccessTools.all);

        private void ExampleFieldChangeCall() => PartModuleEvent.onPartModuleFieldChanged.Fire(this, "FIELDNAME");

        private void AfterMethodCall() => PartModuleEvent.onPartModuleMethodCalled.Fire(this, "METHODNAME");
    }
}