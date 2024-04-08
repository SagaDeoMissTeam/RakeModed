using System;

namespace RakeModed.events
{
    public class GlobalEvents
    {
        public static Action OnAssetsLoaded;

        public static void SendOnAssetsLoaded()
        {
            if(OnAssetsLoaded != null) OnAssetsLoaded.Invoke();
        }
    }
}