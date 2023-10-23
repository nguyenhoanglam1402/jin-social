using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

delegate void RandomSpawnCallback(GameObject character);

public class SpawnNPC : MonoBehaviour
{
    [Header("Character Configurations")]
    [SerializeField] List<CharacterAppearanceObject> npcAppearanceObjects;
    [SerializeField] List<CharacterBehaviorObject> npcBehaviourObjects;
    [SerializeField] List<ItemObject> npcItemObjects;
    [SerializeField] List<AccessoryObject> npcAccessoryObjects;

    [Header("Spawning Configurations")]
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] float timeOfSpawn = 5f;
    [SerializeField] float countDownTime;

    [Header("Number of People alive at the same time")]
    [Min(1)]
    [SerializeField] int NOP = 5;

    private ResidentManager residentManager;

    void Awake()
    {
        residentManager = ResidentManager.Instance;
    }


    // Start is called before the first frame update
    void Start()
    {
        countDownTime = timeOfSpawn;
    }

    void SetupScripAndComponent(GameObject character)
    {
        character.GetComponent<SimpleCharacterControl>().enabled = false;

        //character.AddComponent<VisibleBehaviour>();
        character.AddComponent<NPCAnimatorController>();
        character.AddComponent<NavMeshAgent>();
    }

    void RegisterResidentForCharacter(GameObject character)
    {
        residentManager.RegisterResident(character,
            onDone: (identification) => {
                Debug.Log(string.Format("Welcome ${0} come to this world!", identification.Name));
            },
            onError: (error) =>{
                Destroy(character);
            });
    }

    void RandomSpawn(RandomSpawnCallback callback)
    {
        Transform spawnPointTransform = spawnPoints[Random.Range(0, spawnPoints.Count)];
        AccessoryObject[] npcAccessory = { npcAccessoryObjects[Random.Range(0, npcAccessoryObjects.Count)] };

        bool holdItemLeftHand = Random.Range(0, 2) != 0;
        bool holdItemRightHand = Random.Range(0, 2) != 0;

        GameObject character = CharacterMakerWizard.CreateCharacter(
            npcAppearanceObjects?[Random.Range(0, npcAppearanceObjects.Count)],
            npcBehaviourObjects?[Random.Range(0, npcBehaviourObjects.Count)],
            holdItemLeftHand? npcItemObjects?[Random.Range(0, npcItemObjects.Count)] : null,
            holdItemRightHand? npcItemObjects?[Random.Range(0, npcItemObjects.Count)]: null,
            npcAccessory,
            new Vector3(
                spawnPointTransform.position.x + Random.Range(-2, 2),
                spawnPointTransform.position.y,
                spawnPointTransform.position.z + Random.Range(-2, 2))
            );

        callback(character);
    }

    // Update is called once per frame
    void Update()
    {
        countDownTime -= Time.deltaTime;
        if(countDownTime <= 0 )
        {
            Debug.Log("Resident Number: " + ResidentManager.Instance.residentsIndentifications.Count);
            RandomSpawn((character) => {
                SetupScripAndComponent(character);
                RegisterResidentForCharacter(character);
                countDownTime = timeOfSpawn;
            });

        }
    }
}