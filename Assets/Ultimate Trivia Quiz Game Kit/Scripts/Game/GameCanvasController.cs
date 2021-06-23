using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]

public class GameCanvasController : MonoBehaviour
{
    public static GameCanvasController instance;
    [SerializeField] RectTransform gamePanel;
    [SerializeField] GameObject levelBoxPrefab;
    [SerializeField] GameObject quizCardPrefab;
    [SerializeField] GameObject descriptionCardPrefab;
    [SerializeField] GameObject stageClearPrefab;
    [SerializeField] GameObject finishQuizPrefab;

    [SerializeField] AudioClip finishGameSFX;

    [SerializeField] GameObject confirmPopupPrefab;

    private AudioSource audioSource;

    private Image gamePanelImage;

    private LevelBox levelBox;

    private CardController firstQuizCard = null;
    private CardController secondQuizCard = null;
    private CardController tempQuizCard = null;

    private int currentQuizSequence;

    private List<Quiz> quizList;

    [SerializeField] private Color32 answerQuestion;

    public Color32 colorFinish;

    public Color32 dimPanel;

    [HideInInspector] public int[] counterQuiz;

    private void Awake()
    {
        instance = this;
        counterQuiz = new int[3];
        gamePanelImage = this.gamePanel.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGameCanvas(int levelIndex)
    {
        currentQuizSequence = 0;
        quizList = CSVReader.Instance.Read(levelIndex);
        counterQuiz = new int[3];
        if (quizList.Count > 0)
        {
            levelBox = Instantiate(levelBoxPrefab, gamePanel).GetComponent<LevelBox>();
            levelBox.gameObject.SetActive(false);
            levelBox.ShowAndHide(levelIndex, 0.01f, () =>
            {
                Destroy(levelBox.gameObject);
                InitializeQuizCard(levelIndex);
            });
        }
    }

    public void StopCanvas(DidFinishedStopCanvas callback)
    {
        callback?.Invoke();
    }

    private void InitializeQuizCard(int levelIndex)
    {
        GameObject cardPrefab = levelIndex != Informations.StageIndex ? descriptionCardPrefab : quizCardPrefab;

        if (this.quizList.Count > 0)
        {
            this.firstQuizCard = Instantiate(cardPrefab, gamePanel).GetComponent<CardController>();
            this.firstQuizCard.SetQuiz(this.quizList[currentQuizSequence], currentQuizSequence, this.quizList.Count);

            this.firstQuizCard.SetNextPosition(QuizCardController.PositionType.FIRST, () =>
            {
                if (this.quizList.Count > 1)
                {
                    this.secondQuizCard = Instantiate(cardPrefab, gamePanel).GetComponent<CardController>();
                    this.secondQuizCard.SetNextPosition(QuizCardController.PositionType.SECOND, null);
                }
            });
        }
    }

    public int GetResultQuiz()
    {
        int temp = 0;
        int temp_it = 0;
        for (int i = 0; i < counterQuiz.Length; i++)
        {
            if (counterQuiz[i] > temp)
            {
                temp = counterQuiz[i];
                temp_it = i;
            }
        }
        Constants.stageNum++;
        //temp_it = ((Constants.stageNum - 1) * Constants.stageNum) + temp_it;
        temp_it = ((Constants.stageNum - 1) * 3) + temp_it;
        return temp_it;
    }



    public void PreviousQuiz()
    {
        if (this.currentQuizSequence - 1 >= 0)
        {
            if (this.secondQuizCard)
            {
                this.tempQuizCard = this.secondQuizCard.MoveToPrevious(() =>
                {
                    this.secondQuizCard = null;
                    this.secondQuizCard = this.firstQuizCard.MoveToPrevious(() =>
                    {
                        this.firstQuizCard = null;
                        this.currentQuizSequence--;

                        this.tempQuizCard.SetQuiz(this.quizList[this.currentQuizSequence], this.currentQuizSequence, this.quizList.Count);

                        this.firstQuizCard = this.tempQuizCard.MoveToPrevious(() =>
                        {
                            this.tempQuizCard = null;
                        });
                    });
                });
            }
            else
            {
                this.secondQuizCard = this.firstQuizCard.MoveToPrevious(() => {
                    this.firstQuizCard = null;

                    this.currentQuizSequence--;
                    this.tempQuizCard.SetQuiz(this.quizList[this.currentQuizSequence], this.currentQuizSequence, this.quizList.Count);

                    this.firstQuizCard = this.tempQuizCard.MoveToPrevious(() => {
                        this.tempQuizCard = null;
                    });
                });
            }
        }
    }

    public void NextQuiz()
    {
        if (this.firstQuizCard)
        {
            if (this.tempQuizCard) Destroy(this.tempQuizCard.gameObject);

            this.tempQuizCard = this.firstQuizCard.MoveToNext(() =>
            {
                this.firstQuizCard = null;
                if (this.secondQuizCard)
                {
                    this.currentQuizSequence++;   
                    this.secondQuizCard.SetQuiz(this.quizList[this.currentQuizSequence], this.currentQuizSequence, this.quizList.Count);

                    this.firstQuizCard = this.secondQuizCard.MoveToNext(() =>
                    {
                        this.secondQuizCard = null;

                        if (this.currentQuizSequence + 1 < this.quizList.Count)
                        {
                            this.secondQuizCard = this.tempQuizCard.MoveToNext(() =>
                            {
                                this.tempQuizCard = null;
                            });
                        }
                    });
                }
                else
                {
                    // Destroy unused quiz card
                    Destroy(this.tempQuizCard.gameObject);

                    // Increase stage index
                    Informations.StageIndex++;
                    //Constants.stageNum = Informations.StageIndex;
                    // Show the "Stage Clear" panel

                    FinishQuiz finishQuiz = Instantiate(finishQuizPrefab, this.gamePanel).GetComponent<FinishQuiz>();

                    finishQuiz.Show(10, StartGameCanvas, Informations.StageIndex);    
                    /*StageClear stageClear = Instantiate(stageClearPrefab, this.gamePanel).GetComponent<StageClear>();

                    stageClear.Show(3f, () =>
                    {
                        // Game Over
                        if (Informations.StageIndex < CSVReader.Instance.TotalStage)
                        {
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                Destroy(stageClear.gameObject);
                                this.StartGameCanvas(Informations.StageIndex);
                            });
                            long score = Informations.StageIndex * 5;
                        }
                        else
                        {
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                Destroy(stageClear.gameObject);
                                this.AllClearGame();
                            });
                        }
                    });*/
                }
            });
        }
    }

    public void ChangeBackgroundColor (Color32 color)
    {
        float duration = 3f;
        float startDuration = 0.2f;
        float endDuration = 1f;
        this.gamePanelImage.DOBlendableColor(color, startDuration);

        DOVirtual.DelayedCall(duration, () =>
        {
            this.gamePanelImage.DOBlendableColor(color, endDuration).OnComplete(() =>
            {

            });
        });
    }

    public void ChangeBackgrounColor(bool isCorrect)
    {
        float duration = 3f;
        float startDuration = 0.2f;
        float endDuration = 1f;

        if (isCorrect)
        {
            this.gamePanelImage.DOBlendableColor(answerQuestion, startDuration);

            DOVirtual.DelayedCall(duration, () =>
            {
                this.gamePanelImage.DOBlendableColor(answerQuestion, endDuration).OnComplete(() =>
                {

                });
            });
        }
        else
        {
            this.gamePanelImage.DOBlendableColor(new Color32(44, 55, 89, 255), startDuration);

            DOVirtual.DelayedCall(duration, () =>
            {
                this.gamePanelImage.DOBlendableColor(new Color32(242, 68, 149, 255), endDuration);
            });
        }
    }

    public void AllClearGame()
    {
        if (firstQuizCard) Destroy(firstQuizCard.gameObject);
        if (secondQuizCard) Destroy(secondQuizCard.gameObject);
        if (tempQuizCard) Destroy(tempQuizCard.gameObject);

        ConfirmPopupController confirmPopupController = Instantiate(confirmPopupPrefab, transform).GetComponent<ConfirmPopupController>();
        confirmPopupController.Show("All stages have been cleared.\nWe will create new quiz content soon.", ConfirmPopupController.ConfirmType.NORMAL, false, () =>
        {
            CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
            canvasManager.ShowMain();
        });
    }

    public void FinishGame()
    {
        if (this.secondQuizCard)
        {
            DOVirtual.DelayedCall(0.2f, () =>
            {
                this.secondQuizCard.SetNextPosition(QuizCardController.PositionType.NONE, () =>
                {
                    CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
                    canvasManager.ShowMain();

                    Destroy(this.secondQuizCard.gameObject);
                });
            });

            if (this.firstQuizCard)
            {
                this.firstQuizCard.SetNextPosition(QuizCardController.PositionType.NONE, () =>
                {
                    Destroy(this.firstQuizCard.gameObject);
                });
            }
        }
        else
        {
            if (this.firstQuizCard)
            {
                this.firstQuizCard.SetNextPosition(QuizCardController.PositionType.NONE, () =>
                {
                    CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
                    canvasManager.ShowMain();

                    Destroy(this.firstQuizCard.gameObject);
                });
            }
        }

        if (this.tempQuizCard) Destroy(this.tempQuizCard.gameObject);

        if (Informations.IsPlaySFX) audioSource.PlayOneShot(finishGameSFX);
    }
}
