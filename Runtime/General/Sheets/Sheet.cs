using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DollUtil.Sheets
{
    public enum DownloadType
    {
        None,
        InProject,
        AsPersistent
    }

    [Serializable]
    public class SheetData
    {
        public List<string> Rows = new List<string>();
        public string[] Cells(string Row) => Row.Split(',');

        public string GetCell(int row = 0, int cell = 0)
        {
            try
            {
                return Rows[0].Split(',')[cell];
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError($"Row:{row}, Cell:{cell} does not exist.");
                return "";
            }
        }
    }

    public class Sheet : MonoBehaviour
    {
        public static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        public static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        public static readonly char[] TRIM_CHARS = { '\"' };

        [Header("Sheet Info")]
        public string SheetId;
        public string TabId;
        [Header("Properties")]
        public string FileName;
        public DownloadType Download;
        public bool LoadOnAwake = true;
        [Header("Sheet Data")]
        public SheetData Data;

        [HideInInspector] public bool IsLoaded = false;

        private void Awake()
        {
            if (LoadOnAwake) LoadData();
        }

        public void LoadData()
        {
            string data = "";

            if (Download == DownloadType.InProject)
            {
                TextAsset tmp = Resources.Load(FileName) as TextAsset;
                if (tmp)
                {
                    data = tmp.text;
                }
            }
            else if (Download == DownloadType.AsPersistent)
            {
                if (System.IO.File.Exists(Path.Combine(Application.persistentDataPath, FileName + ".csv").ToString()))
                {
                    using (StreamReader readtext = new StreamReader(Path.Combine(Application.persistentDataPath, FileName + ".csv")))
                    {
                        data = readtext.ReadToEnd();
                    }
                }
            }

            if (data == "")
            {
                Action<string> commCallback = (csv) =>
                {
                    PlaceData(csv);
                    IsLoaded = true;
                };
                StartCoroutine(DownloadCSVCoroutine(SheetId, commCallback, Download, FileName, TabId));
            }
            else
            {
                PlaceData(data);
                IsLoaded = true;
            }
        }

        [ContextMenu("Delete/Persistent")]
        public void DeletePersistent()
        {
            if (System.IO.File.Exists(Path.Combine(Application.persistentDataPath, FileName + ".csv").ToString()))
            {
                System.IO.File.Delete(Path.Combine(Application.persistentDataPath, FileName + ".csv").ToString());
            }
        }

        [ContextMenu("Delete/Project")]
        public void DeleteInProject()
        {
            if (System.IO.File.Exists("Assets/Resources/" + FileName + ".csv"))
            {
                System.IO.File.Delete("Assets/Resources/" + FileName + ".csv");
            }
        }

        public void PlaceData(string data)
        {
            Data.Rows = new List<string>();

            var lines = data.Split('\n');

            foreach (var line in lines)
            {
                Data.Rows.Add(line);
            }
        }

        public static List<List<string>> ParseCSV(string text)
        {
            text = CleanReturnInCsvTexts(text);

            var list = new List<List<string>>();
            var lines = Regex.Split(text, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;

            var header = Regex.Split(lines[0], SPLIT_RE);

            bool jumpedFirst = false;

            foreach (var line in lines)
            {
                if (!jumpedFirst)
                {
                    jumpedFirst = true;
                    continue;
                }
                var values = Regex.Split(line, SPLIT_RE);

                var entry = new List<string>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    var value = values[j];
                    value = DecodeSpecialCharsFromCSV(value);
                    entry.Add(value);
                }
                list.Add(entry);
            }
            return list;
        }

        public static string CleanReturnInCsvTexts(string text)
        {
            text = text.Replace("\"\"", "'");

            if (text.IndexOf("\"") > -1)
            {
                string clean = "";
                bool insideQuote = false;
                for (int j = 0; j < text.Length; j++)
                {
                    if (!insideQuote && text[j] == '\"')
                    {
                        insideQuote = true;
                    }
                    else if (insideQuote && text[j] == '\"')
                    {
                        insideQuote = false;
                    }
                    else if (insideQuote)
                    {
                        if (text[j] == '\n')
                            clean += "<br>";
                        else if (text[j] == ',')
                            clean += "<c>";
                        else
                            clean += text[j];
                    }
                    else
                    {
                        clean += text[j];
                    }
                }
                text = clean;
            }
            return text;
        }

        public static IEnumerator DownloadCSVCoroutine(string docId, Action<string> callback, DownloadType downloadType,
                                                       string assetName = null, string sheetId = null)
        {
            string url =
                "https://docs.google.com/spreadsheets/d/" + docId + "/export?format=csv";

            if (!string.IsNullOrEmpty(sheetId))
                url += "&gid=" + sheetId;

            WWWForm form = new WWWForm();
            WWW download = new WWW(url, form);

            yield return download;

            if (!string.IsNullOrEmpty(download.error))
            {
                UnityEngine.Debug.LogError("Error downloading: " + download.error);
            }
            else
            {
                callback(download.text);
                if (downloadType != DownloadType.None)
                {
                    if (!string.IsNullOrEmpty(assetName))
                    {
                        if (downloadType == DownloadType.InProject)
                        {
                            File.WriteAllText("Assets/Resources/" + assetName + ".csv", download.text);
                        }
                        else
                        {
                            File.WriteAllText(Path.Combine(Application.persistentDataPath, assetName + ".csv"), download.text);
                        }
                    }
                    else
                    {
                        throw new Exception("assetName is null");
                    }
                }
            }
        }

        public static List<List<string>> ReadCSV(string file)
        {
            var data = Resources.Load(file) as TextAsset;
            return ParseCSV(data.text);
        }

        public static string DecodeSpecialCharsFromCSV(string value)
        {
            value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "").Replace("<br>", "\n").Replace("<c>", ",");
            return value;
        }
    }
}