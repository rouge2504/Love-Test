using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FinishQuiz : MonoBehaviour
{
    public Text title;
    public RawImage image;
    public Text description;
    public Button continueButton;
    public Button exitButton;
    public Button restartButton;
    protected GameCanvasController gameCanvasController;
    public void Show(float duration, Action<int> action, int count)
    {


        int it = GameCanvasController.instance.GetResultQuiz();
        title.text = CSVReader.Instance.ReadResult(0)[it].title;
        description.text = CSVReader.Instance.ReadResult(0)[it].description;
        image.texture = CSVReader.Instance.ReadResult(0)[it].image;

        GameCanvasController.instance.ChangeBackgroundColor(GameCanvasController.instance.colorFinish);
        continueButton.onClick.AddListener(delegate () {
            if (Informations.StageIndex < CSVReader.Instance.TotalStage)
            {
                action(count);
            }
            else
            {
                Destroy(this.gameObject);
                GameCanvasController.instance.AllClearGame();
            }
            Destroy(this.gameObject);
            GameCanvasController.instance.ChangeBackgroundColor(GameCanvasController.instance.dimPanel);

        });

        exitButton.onClick.AddListener(delegate ()
        {
            CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
            canvasManager.ShowMain();
            Destroy(this.gameObject);
        });

        restartButton.onClick.AddListener(delegate ()
        {
            GameCanvasController.instance.StartGameCanvas(Constants.stageNum);
            Destroy(this.gameObject);
        });
    }
}
