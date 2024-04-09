using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CharacterEditor
{
    public class CharacterEditor : MonoBehaviour
    {
        [SerializeField] Button nextMaterial;
        [SerializeField] Button nextBodyPart;
        [SerializeField] Button loadGame;

        [SerializeField] TextMeshProUGUI currentBodyPartLabel;
        [SerializeField] TextMeshProUGUI currentMaterialLabel;

        [SerializeField] Character character;

        int id;
        BodyTypes bodyType = BodyTypes.Head;

        private void Awake()
        {
            nextMaterial.onClick.AddListener(NextMaterial);
            nextBodyPart.onClick.AddListener(NextBodyPart);
            loadGame.onClick.AddListener(LoadGame);
            UpdateLabelTexts();
        }

        void NextMaterial()
        {
            id++;
            if (id > 2) id = 0;

            //saving id to PlayerPrefs to load the colors back in
            switch(bodyType)
            {
                case BodyTypes.Arm:
                    PlayerPrefs.SetInt("Arm", id);
                    break;
                case BodyTypes.Leg:
                    PlayerPrefs.SetInt("Leg", id);
                    break;
                case BodyTypes.Head: 
                    PlayerPrefs.SetInt("Head", id);
                    break;
                case BodyTypes.Body:
                    PlayerPrefs.SetInt("Body", id);
                    break;
            }
            PlayerPrefs.Save();
            UpdateLabelTexts();
            character.Load();
        }

        void NextBodyPart()
        {
            //cycles through the different body parts when button is clicked
            switch (bodyType)
            {
                case BodyTypes.Body:
                    bodyType = BodyTypes.Head;
                    break;
                case BodyTypes.Head:
                    bodyType = BodyTypes.Arm;
                    break;
                case BodyTypes.Arm:
                    bodyType = BodyTypes.Leg;
                    break;
                case BodyTypes.Leg:
                    bodyType = BodyTypes.Body;
                    break;
            }

            //gets and sets current saved value to id
            switch (bodyType)
            {
                case BodyTypes.Body:
                    id = PlayerPrefs.GetInt("Body", 0);
                    break;
                case BodyTypes.Head:
                    id = PlayerPrefs.GetInt("Head", 0);
                    break;
                case BodyTypes.Arm:
                    id = PlayerPrefs.GetInt("Arm", 0);
                    break;
                case BodyTypes.Leg:
                    id = PlayerPrefs.GetInt("Leg", 0);
                    break;
            }
            UpdateLabelTexts();
        }

        void LoadGame()
        {
            SceneManager.LoadScene("Game");
        }

        void UpdateLabelTexts()
        {
            currentBodyPartLabel.text = $"{bodyType}";

            switch (bodyType)
            {
                case BodyTypes.Body:
                    currentMaterialLabel.text = $"Color {PlayerPrefs.GetInt("Body", 0) + 1}";
                    break;
                case BodyTypes.Head:
                    currentMaterialLabel.text = $"Color {PlayerPrefs.GetInt("Head", 0) + 1}";
                    break;
                case BodyTypes.Arm:
                    currentMaterialLabel.text = $"Color {PlayerPrefs.GetInt("Arm", 0) + 1}";
                    break;
                case BodyTypes.Leg:
                    currentMaterialLabel.text = $"Color {PlayerPrefs.GetInt("Leg", 0) + 1}";
                    break;
            }
        }
    }
}