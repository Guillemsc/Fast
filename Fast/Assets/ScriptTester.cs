using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum EnumTest
{
    TEST1,
    TEST2,
}

public class ScriptTester : MonoBehaviour
{
    //private Fast.Flow.FlowController flow = new Fast.Flow.FlowController();

    //private Fast.EventsController events = new Fast.EventsController();

    //[SerializeField] private Fast.UI.Form curr_form = null;

    // Start is called before the first frame update
    void Start()
    {
        Fast.GoogleDrive.DownloadSpreadsheet request = new Fast.GoogleDrive.DownloadSpreadsheet(
            "599653651007-097sgjq4ialisovl9c5d82vaq13hvg1l.apps.googleusercontent.com", "lD8oKNHZg8Y-nEJ50Wd5RPt8",
            "16DrdMG_Hz9RHUbBYd1W52E6tkjvJ9Oown_LRyw17bPs", "Sheet1");

        request.RunRequest(
        delegate (Fast.GoogleDrive.DownloadSpreadsheetSuccessObject success)
        {
            string tsv_data = Fast.Parsers.TSVParser.Compose(success.data);

            Fast.Serializers.TextAssetSerializer.SerializeAssetsPath("data.txt", tsv_data );
        }
        , delegate (Fast.GoogleDrive.DownloadSpreadsheetErrorObject error)
        {

        });

        //flow.CreateContainer(0).FlowSetCurrForm(curr_form).FlowNextStartAtEndOfLast().FlowSetCurrForm(curr_form);

        //events.Subscribe(0, CallbackEvet);
        //events.UnSubscribe(0, CallbackEvet);

        //TextAsset asset = Resources.Load<TextAsset>("test");

        //List<string> lines = Fast.Parsers.TextAssetParser.Parse(asset);

        //List<Fast.Parsers.TSVParsedData> data = Fast.Parsers.TSVParser.Parse(lines);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void CallbackEvet(int event_id)
    {

    }
}
