using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] GameObject normalButton;
    [SerializeField] GameObject clearButton;
    [SerializeField] GameObject lockButton;

    [SerializeField] Text[] levelIndexTexts;
    [SerializeField] Text levelTitle;


    private Texture cardTexture;

    private int currentIndex;

    public enum LevelButtonType { NORMAL, CLEAR, LOCK, LAST }
    private LevelButtonType type;
    private LevelButtonType Type
    {
        get
        {
            return this.type;
        }
        set
        {
            this.type = value;

            switch(type)
            {
                case LevelButtonType.NORMAL:
                    this.normalButton.SetActive(true);
                    this.clearButton.SetActive(false);
                    this.lockButton.SetActive(false);
                    break;
                case LevelButtonType.LOCK:
                    this.normalButton.SetActive(false);
                    this.clearButton.SetActive(false);
                    this.lockButton.SetActive(true);
                    break;
                case LevelButtonType.CLEAR:
                    this.normalButton.SetActive(false);
                    this.clearButton.SetActive(true);
                    if (cardTexture != null)
                    {
                        Texture2D tex2D = (Texture2D)cardTexture;
                        this.clearButton.GetComponent<Image>().sprite = Sprite.Create(tex2D, new Rect(0.0f, 0.0f, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }
                    this.lockButton.SetActive(false);
                    break;
                case LevelButtonType.LAST:
                    this.normalButton.SetActive(true);
                    this.clearButton.SetActive(false);
                    this.lockButton.SetActive(false);
                    break;
            }
        }
    }

    public void Show(LevelButtonType type, int index, Texture texture = null)
    {
        cardTexture = texture;
        this.Type = type;
        this.currentIndex = index;

        foreach (Text levelIndexText in levelIndexTexts)
        {
            //levelIndexText.text = (index + 1).ToString();
            levelIndexText.text = "";
        }

        if (type == LevelButtonType.LAST)
        {

            foreach (Text levelIndexText in levelIndexTexts)
            {
                levelIndexText.text = "Soon new tests";
            }
            levelTitle.gameObject.SetActive(false);
        }
    }

    public void OnClickNormalButton() { }

    public void OnClickClearButton()
    {
        MainCanvasController mainCanvasController = GameObject.Find("/Main Canvas").GetComponent<MainCanvasController>();
        mainCanvasController.ShowDescriptions(this.currentIndex);
    }

}
