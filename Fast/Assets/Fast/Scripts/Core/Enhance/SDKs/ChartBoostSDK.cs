using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Enhance.ChartBoostSDK
{    
    public class InterstitialAds : SDK
    {
        private string app_id = "";
        private string app_signature = "";

        public InterstitialAds(string app_id, string app_signature) : base("chartboost", "interstitial_ads")
        {
            this.app_id = app_id;
            this.app_signature = app_signature;
        }

        public override string GenerateData()
        {
            string data = "{\"fgl.enhance.charboost.appid\":\"" + app_id + "\"," +
                "\"fgl.enhance.charboost.appsignature\": \"" + app_signature + "\"}";

            return data;
        }
    }
    
}
