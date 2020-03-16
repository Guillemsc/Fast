using System;
using System.Collections.Generic;

namespace Fast.Cinematics
{
    public class CinematicsController : Fast.IController
    {
        private List<Cinematic> cinematics_to_play = new List<Cinematic>();

        public void PlayCinematic(Cinematic cin)
        {
            cinematics_to_play.Add(cin);
        }

        public void Start()
        {

        }

        public void Update()
        {
            if (cinematics_to_play.Count > 0)
            {
                Cinematic curr_cinematic = cinematics_to_play[0];

                if(!curr_cinematic.Started)
                {
                    curr_cinematic.Start();
                }

                if(!curr_cinematic.Finished)
                {
                    curr_cinematic.Update();
                }
                else
                {
                    cinematics_to_play.RemoveAt(0);
                }
            }
        }
    }
}
