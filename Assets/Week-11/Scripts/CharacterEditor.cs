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

        [SerializeField] Character character;

        int id;
        BodyTypes bodyType = BodyTypes.Head;

        private void Awake()
        {
            nextMaterial.onClick.AddListener(NextMaterial);
            nextBodyPart.onClick.AddListener(NextBodyPart);
            loadGame.onClick.AddListener(LoadGame);
        }

        void NextMaterial()
        {
            id++;
            if (id > 2) id = 0;

            //TODO: Make a switch case for each BodyType and save the value of id to the correct PlayerPref
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

            character.Load();
        }

        void NextBodyPart()
        {
            //TODO: Setup a switch case that will go through the different body types
            //      ie if the current type is Head and we click next then set it to Body
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

            //TODO: Then setup another switch case that will get the current saved value
            //      from the player prefs for the current body type and set it to id
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
        }

        void LoadGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}