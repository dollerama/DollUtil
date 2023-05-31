using System.Collections;
using System.Collections.Generic;
using DollUtil.Buggy.Scribble;
using DollUtil.Sheets;
using UnityEngine;

public class DrawSheetData : MonoBehaviour
{
    public Sheet sheet;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => sheet.IsLoaded);
        Scribble.Draw += new Label(Vector3.up, sheet.FileName);
        Vector3 pos = Vector3.zero;
        foreach(var row in sheet.Data.Rows)
        {
            foreach(var cell in sheet.Data.Cells(row))
            {
                Scribble.Draw += new Label(pos, cell);
                pos.x ++;
            }
            pos.x = 0;
            pos.y--;
        }
    }
}
