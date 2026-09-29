namespace SurvivalGame.World
{
    public static class TravelContext
    {
        public static string TargetLocationId { get; private set; }
        public static string SpawnPointId { get; private set; } = "default";

        public static void SetTarget(string locationId, string spawnPointId = "default")
        {
            TargetLocationId = locationId;
            SpawnPointId = spawnPointId;
        }
    }
}
