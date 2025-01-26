using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPlayerSettings", menuName = "Game/DefaultPlayerSettings")]
public class DefaultPlayerSettings : ScriptableObject
{
   // Game Rules
   [Header("Game Rules")]
   [SerializeField] private int maxTurns = 5;
   [SerializeField] private int surplusFoodToGrowOneCitizenPerTurn = 10;
   [SerializeField] private int requiredUraniumForBarrier = 100;
   [SerializeField] private int increaseRequiredUraniumForBarrierPerTurn = 50;

   // Starting Resources
   [Header("Starting Resources")]
   [SerializeField] private int currentCitizenPopulation = 100;
   [SerializeField] private int currentFood = 1000;
   [SerializeField] private int currentUranium = 100;
   [SerializeField] private int currentWater = 0;
   [SerializeField] private int currentCoin = 0;

   // Resource Consumption Rates
   [Header("Resource Consumption Per Citizen")]
   [SerializeField] private int rateFoodPerCitizen = 1;
   [SerializeField] private int rateUraniumPerCitizen = 1;
   [SerializeField] private int rateWaterPerCitizen = 1;
   [SerializeField] private int rateWaterPerUranium = 1;
   [SerializeField] private int rateCoinPerTurn = 0;

   // Resource Generation Rates
   [Header("Resource Generation Per Assigned Citizen")]
   [SerializeField] private int foodGeneratedPerAssignedCitizen = 300;
   [SerializeField] private int coinGeneratedPerAssignedCitizen = 300;
   [SerializeField] private int waterGeneratedPerAssignedCitizen = 300;
   [SerializeField] private int uraniumGeneratedPerAssignedCitizen = 300;

   // Death Rates
   [Header("Death Rates")]
   [SerializeField] private int rateCitizenDeathPerNoResource = 1;
   [SerializeField] private int rateCitizenDeathByBarrier = 1;

   // Public Properties
   public int MaxTurns => maxTurns;
   public int SurplusFoodToGrowOneCitizenPerTurn => surplusFoodToGrowOneCitizenPerTurn;
   public int RequiredUraniumForBarrier => requiredUraniumForBarrier;

   public int IncreaseRequiredUraniumForBarrierPerTurn => increaseRequiredUraniumForBarrierPerTurn;
   
   public int CurrentCitizenPopulation => currentCitizenPopulation;
   public int CurrentFood => currentFood;
   public int CurrentUranium => currentUranium;
   public int CurrentWater => currentWater;
   public int CurrentCoin => currentCoin;
   
   public int RateFoodPerCitizen => rateFoodPerCitizen;
   public int RateUraniumPerCitizen => rateUraniumPerCitizen;
   public int RateWaterPerCitizen => rateWaterPerCitizen;
   public int RateWaterPerUranium => rateWaterPerUranium;
   public int RateCoinPerTurn => rateCoinPerTurn;
   
   public int FoodGeneratedPerAssignedCitizen => foodGeneratedPerAssignedCitizen;
   public int CoinGeneratedPerAssignedCitizen => coinGeneratedPerAssignedCitizen;
   public int WaterGeneratedPerAssignedCitizen => waterGeneratedPerAssignedCitizen;
   public int UraniumGeneratedPerAssignedCitizen => uraniumGeneratedPerAssignedCitizen;
   
   public int RateCitizenDeathPerNoResource => rateCitizenDeathPerNoResource;
   public int RateCitizenDeathByBarrier => rateCitizenDeathByBarrier;
}