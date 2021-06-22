using System;

[Serializable]
public class Quiz
{
    public int type;
    public string description;
    public string question;
    public int answer;
    public string e01;
    public string e02;
    public string e03;
    public UnityEngine.Texture image;
}

[Serializable]

public class Quiz_Result
{
    public string description;
    public UnityEngine.Texture image;
}
