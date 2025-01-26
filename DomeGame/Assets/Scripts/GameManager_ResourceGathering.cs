using MoonlitSystem.UI.Immediate;
using TMPro;
using UnityEngine;

public partial class GameManager
{
    int totalPopulation;
    int unusedPopulation;

    int numCoinAssignments = 0;
    int numFoodAssignments = 0;
    int numUraniumAssignments = 0;
    int numWaterAssignments = 0;

    bool isMovingFromUnallocated;
    bool isMovingFromCoin;
    bool isMovingFromFood;
    bool isMovingFromUranium;
    bool isMovingFromWater;

    [Space]
    [Header("Resource Gathering")]
    [SerializeField] GameObject[] citizenObjects;

    [SerializeField] TMP_Text numUnassignedCitizensText;

    [SerializeField] TMP_Text coinTooltipText;
    [SerializeField] TMP_Text foodTooltipText;
    [SerializeField] TMP_Text uraniumTooltipText;
    [SerializeField] TMP_Text waterTooltipText;

    [SerializeField] GameObject endTurnButton;

    [SerializeField] AudioClip pickupSound1;
    [SerializeField] AudioClip pickupSound2;
    [SerializeField] AudioClip droppedSound;

    string defaultCoinTooltipString = "BubbleCoin can be used during random events.\n\nPlace a citizen here to mine BubbleCoin.";
    string defaultFoodTooltipString = "Food is necessary for your population to survive. Every 10 surplus of food you have creates an extra person.\n\nPlace a citizen here to collect food.";
    string defaultUraniumTooltipString = "Uranium is used to power your bubble. You need 1 uranium per person in your population.\n\nPlace a citizen here to collect uranium.";
    string defaultWaterTooltipString = "Water is necessary for your population to survive.\n\nPlace a citizen here to collect water.";

    void StartCore()
    {
        Screen = GameScreens.Core;
        totalPopulation = GetCurrentCitizenPopulation();
        unusedPopulation = totalPopulation;
        SetUpUsableCitizenObjects();
        numCoinAssignments = 0;
        numFoodAssignments = 0;
        numUraniumAssignments = 0;
        numWaterAssignments = 0;

        isMovingFromUnallocated = false;
        isMovingFromCoin = false;
        isMovingFromFood = false;
        isMovingFromUranium = false;
        isMovingFromWater = false;

        coinTooltipText.text = defaultCoinTooltipString;
        foodTooltipText.text = defaultFoodTooltipString;
        uraniumTooltipText.text = defaultUraniumTooltipString;
        waterTooltipText.text = defaultWaterTooltipString;

        numUnassignedCitizensText.text = (unusedPopulation / gameSettings.citizenUnit).ToString();
    }

    void SetUpUsableCitizenObjects()
    {
        for (int i = 0; i < citizenObjects.Length; i++) {
            citizenObjects[i].SetActive(false);
            citizenObjects[i].transform.position = Reference.Find<RectTransform>(this, "/Canvas/Bottom Bar/Unassignedcc19").position;
        }
        int numCitizenObjectsNeeded = totalPopulation / gameSettings.citizenUnit;
        for (int i = 0; i < numCitizenObjectsNeeded; i++) {
            citizenObjects[i].SetActive(true);
        }
    }

    void HandleCore()
    {
        ImmediateStyle.CanvasGroup("/Canvas/ResourceGathering7d9d");

        string[] allCitizenGUIDs = new[] {
            "/Canvas/Bottom Bar/Unassigned/Citizen1a3e",
            "/Canvas/Bottom Bar/Unassigned/Citizen29af0",
            "/Canvas/Bottom Bar/Unassigned/Citizen352d5",
            "/Canvas/Bottom Bar/Unassigned/Citizen4b866",
            "/Canvas/Bottom Bar/Unassigned/Citizen58c27",
            "/Canvas/Bottom Bar/Unassigned/Citizen6ba83",
            "/Canvas/Bottom Bar/Unassigned/Citizen7405f",
            "/Canvas/Bottom Bar/Unassigned/Citizen8efa3",
            "/Canvas/Bottom Bar/Unassigned/Citizen9ce93",
            "/Canvas/ResourceGathering/Bottom Bar/Unassigned/Citizen108e16",
            "/Canvas/ResourceGathering/Bottom Bar/Unassigned/Citizen11f2e7",
            "/Canvas/ResourceGathering/Bottom Bar/Unassigned/Citizen1243a0",
            "/Canvas/ResourceGathering/Bottom Bar/Unassigned/Citizen139826",
            "/Canvas/ResourceGathering/Bottom Bar/Unassigned/Citizen148f4f",
            "/Canvas/ResourceGathering/Bottom Bar/Unassigned/Citizen153f10"
        };

        int numCitizenObjectsNeeded = totalPopulation / gameSettings.citizenUnit;
        for (int i = 0; i < numCitizenObjectsNeeded; i++) {
            string guid = allCitizenGUIDs[i];

            var dragDropObject = ImmediateStyle.DragDrop(guid, out var component);

            var unassignedSlot = Reference.Find<RectTransform>(this, "/Canvas/Bottom Bar/Unassignedcc19");
            var coinResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/CoinResource3e73");
            var foodResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/FoodResourcea194");
            var uraniumResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/UraniumResource8ffd");
            var waterResourceSlot = Reference.Find<RectTransform>(this, "/Canvas/City/WaterResourceeb63");

            var startedDrag = component.IsMouseDown;
            var hasDropped = dragDropObject.IsMouseUp;

            if (startedDrag) {
                var pickupSound = Random.Range(0, 2) == 0 ? pickupSound1 : pickupSound2;
                gameObject.GetComponent<AudioSource>().PlayOneShot(pickupSound);

                isMovingFromUnallocated = false;
                isMovingFromCoin = false;
                isMovingFromFood = false;
                isMovingFromUranium = false;
                isMovingFromWater = false;

                if (RectTransformUtility.RectangleContainsScreenPoint(unassignedSlot, component.transform.position)) {
                    isMovingFromUnallocated = true;
                } else if (RectTransformUtility.RectangleContainsScreenPoint(coinResourceSlot, component.transform.position)) {
                    isMovingFromCoin = true;
                } else if (RectTransformUtility.RectangleContainsScreenPoint(foodResourceSlot, component.transform.position)) {
                    isMovingFromFood = true;
                } else if (RectTransformUtility.RectangleContainsScreenPoint(uraniumResourceSlot, component.transform.position)) {
                    isMovingFromUranium = true;
                } else if (RectTransformUtility.RectangleContainsScreenPoint(waterResourceSlot, component.transform.position)) {
                    isMovingFromWater = true;
                }
            }

            if (hasDropped) {
                if (RectTransformUtility.RectangleContainsScreenPoint(unassignedSlot, component.transform.position)) {
                    component.PinnedPosition = unassignedSlot.position;
                    UpdateUnallocatedCitizenAllocation();
                    UpdatePreviousResourceAllocation();
                }

                // Coin resource logic
                if (RectTransformUtility.RectangleContainsScreenPoint(coinResourceSlot, component.transform.position)) {
                    component.PinnedPosition = coinResourceSlot.position;
                    UpdateCoinAllocation();
                    UpdatePreviousResourceAllocation();
                }

                // Food resource logic
                if (RectTransformUtility.RectangleContainsScreenPoint(foodResourceSlot, component.transform.position)) {
                    component.PinnedPosition = foodResourceSlot.position;
                    UpdateFoodAllocation();
                    UpdatePreviousResourceAllocation();
                }

                // Uranium resource logic
                if (RectTransformUtility.RectangleContainsScreenPoint(uraniumResourceSlot, component.transform.position)) {
                    component.PinnedPosition = uraniumResourceSlot.position;
                    UpdateUraniumAllocation();
                    UpdatePreviousResourceAllocation();
                }

                // Water resource logic
                if (RectTransformUtility.RectangleContainsScreenPoint(waterResourceSlot, component.transform.position)) {
                    component.PinnedPosition = waterResourceSlot.position;
                    UpdateWaterAllocation();
                    UpdatePreviousResourceAllocation();
                }

                component.transform.position = component.PinnedPosition;
                gameObject.GetComponent<AudioSource>().PlayOneShot(droppedSound);

                // Reset all start drag bools
                isMovingFromUnallocated = false;
                isMovingFromCoin = false;
                isMovingFromFood = false;
                isMovingFromUranium = false;
                isMovingFromWater = false;

                UpdateTooltips();
            }
        }

        if (ImmediateStyle.Button("/Prefab Mode in Context/ResourceGathering/EndTurnButton7b3d").IsMouseDown) {
            gameObject.GetComponent<AudioSource>().PlayOneShot(clickSound);
            Debug.Log("Ended turn with the following allocations:");
            Debug.Log(" Coin allocation: " + numCoinAssignments +
            "; Food allocation: " + numFoodAssignments +
            "; Uranium allocation: " + numUraniumAssignments +
            "; Water allocation: " + numWaterAssignments);
            endTurnClicked = true;
        }
    }

    void UpdateUnallocatedCitizenAllocation()
    {
        Debug.Log("Unassigning 1 citizen");
        unusedPopulation += gameSettings.citizenUnit;
    }

    void UpdateCoinAllocation()
    {
        Debug.Log("Assigning 1 citizen to mine bubble coin");
        numCoinAssignments += gameSettings.citizenUnit;
    }

    void UpdateFoodAllocation()
    {
        Debug.Log("Assigning 1 citizen to farm food");
        numFoodAssignments += gameSettings.citizenUnit;
    }

    void UpdateUraniumAllocation()
    {
        Debug.Log("Assigning 1 citizen to collect uranium");
        numUraniumAssignments += gameSettings.citizenUnit;
    }

    void UpdateWaterAllocation()
    {
        Debug.Log("Assigning 1 citizen to collect water");
        numWaterAssignments += gameSettings.citizenUnit;
    }

    void UpdatePreviousResourceAllocation()
    {
        if (isMovingFromUnallocated) {
            unusedPopulation -= gameSettings.citizenUnit;
            Debug.Log("Still have " + unusedPopulation + " people left to allocate");
        } else if (isMovingFromCoin) {
            numCoinAssignments -= gameSettings.citizenUnit;
        } else if (isMovingFromFood) {
            numFoodAssignments -= gameSettings.citizenUnit;
        } else if (isMovingFromUranium) {
            numUraniumAssignments -= gameSettings.citizenUnit;
        } else if (isMovingFromWater) {
            numWaterAssignments -= gameSettings.citizenUnit;
        }
    }

    void UpdateTooltips()
    {
        numUnassignedCitizensText.text = (unusedPopulation / gameSettings.citizenUnit).ToString();

        if (numCoinAssignments == 0) {
            coinTooltipText.text = defaultCoinTooltipString;
        } else {
            int coinRate = CalculateRateCoinGivenCitizen(numCoinAssignments);
            coinTooltipText.text = numCoinAssignments + " people allocated to mining BubbleCoin\n\nCoin rate = " + coinRate;
        }

        if (numFoodAssignments == 0) {
            foodTooltipText.text = defaultFoodTooltipString;
        } else {
            int foodRate = CalculateRateFoodGivenCitizen(numFoodAssignments);
            foodTooltipText.text = numFoodAssignments + " people allocated to farming\n\nFood rate = " + foodRate;
        }

        if (numUraniumAssignments == 0) {
            uraniumTooltipText.text = defaultUraniumTooltipString;
        } else {
            int uraniumRate = CalculateRateUraniumGivenCitizen(numUraniumAssignments);
            uraniumTooltipText.text = numUraniumAssignments + " people allocated to collecting uranium\n\nUranium rate = " + uraniumRate;
        }

        if (numWaterAssignments == 0) {
            waterTooltipText.text = defaultWaterTooltipString;
        } else {
            int waterRate = CalculateRateWaterGivenCitizen(numWaterAssignments);
            waterTooltipText.text = numWaterAssignments + " people allocated to collecting water\n\nWater rate = " + waterRate;
        }
    }
}