using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    private string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    private char[] TRIM_CHARS = { '\"' };
    private TextAsset data;
    private TextAsset data_result;
    private string[] lines;
    private int totalStage;

    private static CSVReader instance;
    public static CSVReader Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new CSVReader("Data/Love/LoveTest");
            }
            return instance;
        }
    }

    public int TotalStage
    {
        get
        {
            return totalStage;
        }
    }

    public CSVReader(string fileName)
    {
        data = Resources.Load(fileName) as TextAsset;
        data_result = Resources.Load(fileName + "_result") as TextAsset;
        lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if(lines != null)
        {
            totalStage = (lines.Length - 1) / Constants.quizCountInStage;
            totalStage += ((lines.Length - 1) % Constants.quizCountInStage > 0) ? 1 : 0;
        }
        else
        {
            totalStage = 0;
        }

    }

    public List<Quiz_Result> ReadResult(int result)
    {
        var list = new List<Quiz_Result>();
        string[] lines = Regex.Split(data_result.text, LINE_SPLIT_RE);
        if (lines.Length > 1)
        {
            int startLineIndex = result * Constants.quizCountInStage + 1;
            int endLineIndex = Constants.quizCountInResult;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var i = startLineIndex; i < endLineIndex; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "") continue;

                Quiz_Result entry = new Quiz_Result();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                    switch (j)
                    {
                        case 0:
                            entry.title = value;
                            break;
                        case 1:
                            entry.description = value;
                            break;
                        case 2:
                            entry.image = GetImage(value);
                            break;
                    }
                }
                list.Add(entry);
            }
        }
        return list;
    }

    public List<Quiz> Read(int stageNum)
    {
        var list = new List<Quiz>();
        Constants.stageNum = stageNum;
        if (lines.Length > 1)
        {
            int startLineIndex = stageNum * Constants.quizCountInStage + 1;
            int endLineIndex = (lines.Length <= startLineIndex + Constants.quizCountInStage) ? lines.Length : startLineIndex + Constants.quizCountInStage;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var i = startLineIndex; i < endLineIndex; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "") continue;

                Quiz entry = new Quiz();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                    switch (j)
                    {
                        case 0:
                            entry.question = value;
                            break;
                        case 1:
                            entry.description = value;
                            break;
                        case 2:
                            entry.type = int.Parse(value);
                            break;
                        case 3:
                            entry.answer = int.Parse(value);
                            break;
                        case 4:
                            entry.e01 = value;
                            break;
                        case 5:
                            entry.e02 = value;
                            break;
                        case 6:
                            entry.e03 = value;
                            break;
                        case 7:
                            entry.image = GetImage(value);
                            break;
                    }
                }
                list.Add(entry);
            }
        }
        return list;
    }

    private Texture GetImage(string nameImage)
    {
        return Resources.Load<Texture>("Data/Love/" + nameImage);
    }
}