using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossPlatformInput
{
    public static class CrossPlatformInputManager
    {
        static private ObjectPreferences<AxisInput> axisInputPrefs;
        static private ObjectPreferences<ButtonInput> buttonInputPrefs;

        public class AxisInput
        {
            public string AxisName { get; private set; }
        }

        public class ButtonInput
        {
            public string KeyName { get; private set; }
        }

    }
}