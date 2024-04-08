using System;

namespace RakeModed.events
{
    public class LevelEvents
    {
        public static Action OnLevelLoaded;


        public static void SendOnLevelLoaded()
        {
            if(OnLevelLoaded != null) OnLevelLoaded.Invoke();
        }
    }
}