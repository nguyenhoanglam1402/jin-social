using System.Collections.Generic;
using System;
using UnityEngine;


public class ResidentManager : MonoBehaviour, IObserver
{
    private static ResidentManager instance;

    [SerializeField]
    private ObserverSubject spawnManager;

    [SerializeField]
    private readonly string observerRegistryName = "residentObserver";

    public readonly List<ResidentIdentification> residentsIndentifications = new();

    public int ResidentCount
    {
        get => residentsIndentifications.Count;
    }

    public delegate void OnRegisterDone(ResidentIdentification identification);
    public delegate void OnRegisterError(Exception error);



    public List<ResidentIdentification> ResidentsIndentifications
    {
        get => residentsIndentifications;
    }

    public static ResidentManager Instance {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<ResidentManager>();
                if(!instance)
                {
                    instance = new GameObject(name: "ResidentManager (Singleton)").AddComponent<ResidentManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        GameObject spawnManagerObject = GameObject.Find("SpawnManager");
        if (spawnManager == null)
        {
            Debug.LogWarning("Cannot found Observer Subject of SpawnManager");
            return;
        }
        spawnManager = spawnManagerObject.GetComponent<ObserverSubject>();
        spawnManager.AddObserver(observerRegistryName, this);
    }

    /// <summary>
    /// Register a character to the resident management system of the world.
    /// Every people just have one resident management system, which can handle all resident from other city
    /// </summary>
    /// <param name="character">The game object of character you want to assign</param>
    /// <param name="onDone">Trigger when register successfully</param>
    /// <param name="onError">Trigger when register failed</param>

    public void RegisterResident(GameObject character, OnRegisterDone onDone, OnRegisterError onError)
    {
        try
        {
            string idString = RandomStringUtil.RandomString(12);
            string fileContent = LoadFileUtil.LoadFile<string>(path: "NameList/name-list");
            CharacterNameModel characterName = JsonUtility.FromJson<CharacterNameModel>(fileContent);
            int randomFirstName = UnityEngine.Random.Range(0, characterName.firstname.Length);
            int randomLastName = UnityEngine.Random.Range(0, characterName.lastname.Length);
            ResidentIdentification identification = new(
                id: idString,
                name: characterName.firstname[randomFirstName] + " " + characterName.lastname[randomLastName],
                age: UnityEngine.Random.Range(16, 60),
                characterObject: character);

            if (identification != null && identification.ID != "")
            {
                residentsIndentifications.Add(identification);
                onDone(identification);
                return;
            }
            Exception exception = new(
                message: "Register this character to Resident failed!");
            onError(exception);
        } catch(Exception error)
        {
            onError(error);
        }


    }

    public void OnNotify<T>(T character)
    {
        if(typeof(T) == typeof(GameObject))
        {
            RegisterResident(character as GameObject,
                onDone: (identification) => {
                    Debug.Log(string.Format("Welcome ${0} come to this world!", identification.Name));
                },
                onError: (error) => {
                    Destroy(character as GameObject);
                });
        }
    }
}

